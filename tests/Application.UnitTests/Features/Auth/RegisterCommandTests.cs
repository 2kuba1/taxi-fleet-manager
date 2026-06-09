using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Application.Features.Auth.Commands;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace Application.UnitTests.Features.Auth;

public class RegisterCommandTests
{
    private readonly IIdentityService _identityService;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly IRoleService _roleService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RegisterCommandHandler _handler;
    
    public RegisterCommandTests()
    {
        _identityService = Substitute.For<IIdentityService>();
        _userService = Substitute.For<IUserService>();
        _emailService = Substitute.For<IEmailService>();
        _roleService = Substitute.For<IRoleService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new RegisterCommandHandler(
            _identityService,
            _userService,
            _emailService,
            _roleService,
            _unitOfWork
        );
    }

    private readonly RegisterCommand _command = new RegisterCommand("test@test.com",
        "Test",
        "Test",
        "123456789",
        "48",
        1.2f,
        ContractType.Mandate,
        Guid.NewGuid());
    
    [Fact]
    public async Task Handle_WhenSuccess_ShouldCreateUserGenerateLoginSendEmailAndNotRollback()
    {
        //Arrange
        string expectedToken = "reset-token-123";
        
        string login = "teste1";
    
        Role role = Role.Create("Driver");
        
        _userService.CheckIfEmailExistsAsync(_command.Email).Returns(false);
        
        _roleService.GetRoleByNameAsync("Driver").Returns(role);
        
        _identityService.UserExistsByNameAsync("teste").Returns(true);
        _identityService.UserExistsByNameAsync("teste1").Returns(false);    
        _identityService.CreateUserAsync(Arg.Any<Guid>(), Arg.Any<string>(), _command.Email, Arg.Any<string>())
            .Returns((true, string.Empty));
        _identityService.GeneratePasswordResetTokenAsync(_command.Email).Returns(expectedToken);
        
        _emailService.SendWelcomeEmailAsync(_command.Email, login, Arg.Any<string>(), expectedToken).Returns(Task.CompletedTask);
    
        //Act
        await _handler.Handle(_command, CancellationToken.None);
    
        //Assert
        await _identityService.Received(1).CreateUserAsync(
            Arg.Any<Guid>(),
            login,
            _command.Email,
            Arg.Any<string>());
        
        await _userService.Received(1).SaveUserAsync(Arg.Is<User>(u => 
            u.Email == _command.Email &&
            u.Login == login &&
            u.RoleId == role.Id));
        
        await _userService.Received(1).SaveUserAsync(Arg.Is<User>(u =>
            u.Email == _command.Email &&
            u.Login == login &&
            u.RoleId == role.Id));
        
        await _unitOfWork.Received(1).BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().RollbackTransactionAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_WhenDomainUserSaveFails_ShouldRollbackTransactionAndThrowException()
    {
        //Arrange
        Role role = Role.Create("Driver");
        
        _userService.CheckIfEmailExistsAsync(_command.Email).Returns(false);
        _userService.SaveUserAsync(Arg.Any<User>()).Throws(new Exception());

        _identityService.UserExistsByNameAsync(Arg.Any<string>()).Returns(false);
        _identityService.CreateUserAsync(Arg.Any<Guid>(), Arg.Any<string>(), _command.Email, Arg.Any<string>())
            .Returns((true, string.Empty));
        
        _roleService.GetRoleByNameAsync("Driver").Returns(role);
        
        //Act
        var act = async () => await _handler.Handle(_command, CancellationToken.None);
        
        //Assert
        await Should.ThrowAsync<Exception>(act);

        await _unitOfWork.Received(1).BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).RollbackTransactionAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_WhenIdentityCreationFails_ShouldRollbackTransactionAndThrowException()
    {
        //Arrange
        Role role = Role.Create("Driver");
        
        _userService.CheckIfEmailExistsAsync(_command.Email).Returns(false);
        _identityService.UserExistsByNameAsync(Arg.Any<string>()).Returns(false);
        _roleService.GetRoleByNameAsync("Driver").Returns(role);

        _identityService.CreateUserAsync(Arg.Any<Guid>(), Arg.Any<string>(), _command.Email, Arg.Any<string>())
            .Returns((false, "Password too weak"));

        //Act
        var act = async () => await _handler.Handle(_command, CancellationToken.None);

        //Assert
        var exception = await Should.ThrowAsync<UserCreationFailedException>(act);
        
        exception.Message.ShouldBe("User creation failed: Password too weak");
        
        await _unitOfWork.Received(1).BeginTransactionAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).RollbackTransactionAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ShouldThrowEmailAlreadyExistsException()
    {
        //Arrange
        _userService.CheckIfEmailExistsAsync(_command.Email).Returns(true);

        //Act
        var act = async () => await _handler.Handle(_command, CancellationToken.None);
        
        //Assert
        var exception = await Should.ThrowAsync<EmailAlreadyExistsException>(act);

        exception.Message.ShouldBe($"Email {_command.Email} is already registered.");
        await _unitOfWork.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Handle_WhenRoleDriverNotFound_ShouldThrowRoleNotFoundException()
    {
        //Arrange
        _userService.CheckIfEmailExistsAsync(_command.Email).Returns(false);
        _roleService.GetRoleByNameAsync("Driver").Returns((Role)null!);

        //Act 
        var act = async () => await _handler.Handle(_command, CancellationToken.None);
        
        //Assert
        var exception = await Should.ThrowAsync<RoleNotFoundException>(act);
        exception.Message.ShouldBe("Role 'Driver' not found.");
        await _unitOfWork.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
    }
    
}