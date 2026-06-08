using Domain.Entities;
using Domain.Enums;
using Shouldly;

namespace Domain.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateUser()
    {
        //Arrange
        string expectedEmail = "test@test.com";
        string expectedLogin = "Login";
        string expectedPhone = "123456789";
        string expectedAreaCode = "48";
        string expectedFirstName = "Test";
        string expectedLastName = "Test";
        float expectedRate = 1.2f;
        ContractType expectedContract = ContractType.Mandate;
        Guid expectedRoleId = Guid.NewGuid();
        Guid expectedTeamId = Guid.NewGuid();
       
        //Act
        var user = User.Create(
            Guid.NewGuid(),
            expectedEmail,
            expectedLogin,
            expectedPhone,
            expectedAreaCode,
            expectedFirstName,
            expectedLastName,
            expectedRate,
            expectedContract,
            expectedRoleId,
            expectedTeamId
        );
        
        //Assert
        
        user.ShouldNotBeNull();
        
        user.Email.ShouldBe(expectedEmail);
        user.Login.ShouldBe(expectedLogin);
        user.FirstName.ShouldBe(expectedFirstName);
        user.LastName.ShouldBe(expectedLastName);
        user.ContractType.ShouldBe(expectedContract);
        user.RoleId.ShouldBe(expectedRoleId);
        user.TeamId.ShouldBe(expectedTeamId);
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidEmails))]
    public void Create_WithInvalidEmail_ShouldThrowArgumentException(string invalidEmail)
    {
        var user = () => User.Create(
            Guid.NewGuid(),
            invalidEmail,
            "Driver1",
            "123456789",
            "48",
            "Test",
            "Test",
            1.2f,
            ContractType.Mandate,
            Guid.NewGuid(),
            Guid.NewGuid());
        
        Assert.Throws<ArgumentException>(user);
    }

    [Theory]
    [MemberData(nameof(GetInvalidLogins))]
    public void Create_WithInvalidLogin_ShouldThrowArgumentException(string invalidLogin)
    {
        Assert.Throws<ArgumentException>(() => User.Create(
            Guid.NewGuid(),
            "test@test.com",
            invalidLogin,
            "123456789",
            "48",
            "Test",
            "Test",
            1.2f,
            ContractType.Mandate,
            Guid.NewGuid(),
            Guid.NewGuid()));
    }
    
    public static IEnumerable<object[]> GetInvalidEmails()
    {
        yield return new object[] { "invalid-email" };
        yield return new object[] { "" };
        yield return new object[] { "    " };
    }
    
    public static IEnumerable<object[]> GetInvalidLogins()
    {
        yield return new object[] { "to-long-login-credential" };
        yield return new object[] { "l" };
        yield return new object[] { "" };
        yield return new object[] { "   " };
    }
}