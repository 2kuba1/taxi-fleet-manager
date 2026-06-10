using Application.Contracts.Persistence;
using Application.Features.Auth.Queries.Login;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shouldly;

namespace Application.UnitTests.Features.Auth;

public class LoginQueryTests
{
    private readonly IIdentityService _identityService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly LoginQueryHandler _handler;
    
    public LoginQueryTests()
    {
        _identityService = Substitute.For<IIdentityService>();
        _userService = Substitute.For<IUserService>();
        _tokenService = Substitute.For<ITokenService>();

        _handler = new LoginQueryHandler(
            _identityService,
            _userService,
            _tokenService);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnTokenResponseDto_WhenCredentialsAreValidAndUserExists()
    {
        //Arrange
        LoginQuery query = new LoginQuery("jakwo", "StrongPassword123!");
        
        Role role = Role.Create("Driver");
        User expectedUser = User.Create(Guid.NewGuid(), "test@test.com", "jakwo", "123456789", "48", "Jakub", "Wojtyna", 1.2f, ContractType.Mandate, role.Id);

        string expectedAccessToken = "access_token";
        RefreshToken  expectedRefreshToken = RefreshToken.Create("refresh_token_hash", expectedUser.Id, DateTime.UtcNow.AddDays(30));
        string expectedRawRefreshToken = "refresh_token_raw";
        
        _identityService.CheckLoginCredentialsAsync(query.Login, query.Password).Returns(true);
        
        _userService.GetUserByLoginAsync(query.Login).Returns(expectedUser);
        
        _tokenService.CreateAccessToken(expectedUser)
            .Returns(expectedAccessToken);
        
        _tokenService.CreateRefreshToken(expectedUser.Id)
            .Returns((expectedRefreshToken, expectedRawRefreshToken));
        
        //Act
        var act = await _handler.Handle(query, CancellationToken.None);
        
        //Assert
        act.ShouldNotBeNull();
        act.AccessToken.ShouldBe(expectedAccessToken);
        act.RefreshToken.ShouldBe(expectedRefreshToken.TokenHash);
        
        await _identityService.Received(1).CheckLoginCredentialsAsync(query.Login, query.Password);
        await _userService.Received(1).GetUserByLoginAsync(query.Login);
        _tokenService.Received(1).CreateAccessToken(expectedUser);
        await _tokenService.Received(1).CreateRefreshToken(expectedUser.Id);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidCredentialsException_WhenIdentityServiceReturnsFalse()
    {
        //Arrange
        LoginQuery query = new LoginQuery("wrong", "credentials");
        
        _identityService.CheckLoginCredentialsAsync(query.Login, query.Password).Returns(false);
        
        //Act
        var act = async () => await  _handler.Handle(query, CancellationToken.None);
        
        //Assert
        await Should.ThrowAsync<InvalidCredentialsException>(act);
        
        await _userService.DidNotReceiveWithAnyArgs().GetUserByLoginAsync(Arg.Any<string>());
        _tokenService.DidNotReceiveWithAnyArgs().CreateAccessToken(Arg.Any<User>());
        await _tokenService.DidNotReceiveWithAnyArgs().CreateRefreshToken(Arg.Any<Guid>());
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidCredentialsException_WhenUserServiceReturnsNull()
    {
        //Arrange
        LoginQuery query = new LoginQuery("wrong", "credentials");
        
        _identityService.CheckLoginCredentialsAsync(query.Login, query.Password).Returns(true);
        _userService.GetUserByLoginAsync(query.Login).ReturnsNull();
        
        //Act
        var act = async () => await  _handler.Handle(query, CancellationToken.None);
        
        //Assert
        await Should.ThrowAsync<InvalidCredentialsException>(act);

        await _identityService.Received(1).CheckLoginCredentialsAsync(query.Login, query.Password);
        await _userService.Received(1).GetUserByLoginAsync(query.Login);
        _tokenService.DidNotReceiveWithAnyArgs().CreateAccessToken(Arg.Any<User>());
        await _tokenService.DidNotReceiveWithAnyArgs().CreateRefreshToken(Arg.Any<Guid>());
    }
}