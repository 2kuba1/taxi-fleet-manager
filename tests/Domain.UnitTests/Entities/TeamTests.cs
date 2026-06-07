using Domain.Entities;
using Shouldly;

namespace Domain.UnitTests.Entities;

public class TeamTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateTeam()
    {
        //Areange
        string expectedName = "Cieszyn";
        Guid expectedOwnerId =  Guid.NewGuid();
        
        //Act
        var team = Team.Create(expectedName, expectedOwnerId);
        
        //Assert
        team.ShouldNotBeNull();
        team.Name.ShouldBe(expectedName);
        team.OwnerId.ShouldBe(expectedOwnerId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string invalidName)
    {
        //Arrange
        Guid ownerId = Guid.NewGuid();
        
        //Act
        var team = () => Team.Create(invalidName, ownerId);
        
        //Assert
        Assert.Throws<ArgumentException>(team);
    }

    [Fact]
    public void Create_WithEmptyOwnerId_ShouldThrowArgumentException()
    {
        //Arrange
        string expectedName = "Cieszyn";
        Guid ownerId = Guid.Empty;
        
        //Act
        var team = () => Team.Create(expectedName, ownerId);
        
        //Assert
        Assert.Throws<ArgumentException>(team);
    }

    [Fact]
    public void AssignCarFleet_WithValidCarFleet_ShouldSetCarFleetAndCarFleetId()
    {
        // Arrange
        Guid teamId = Guid.NewGuid();
        Team team = Team.Create("Cieszyn", Guid.NewGuid());
        
        team.Id = teamId; 

        var carFleet = CarFleet.Create(team.Id);
        
        Guid expectedCarFleetId = Guid.NewGuid();
        carFleet.Id = expectedCarFleetId; 

        // Act
        team.AssignCarFleet(carFleet);

        // Assert
        team.CarFleet.ShouldBe(carFleet);
        team.CarFleetId.ShouldBe(expectedCarFleetId);
    }
    
    [Fact]
    public void AssignCarFleet_WithNullCarFleet_ShouldThrowArgumentNullException()
    {
        // Arrange
        string expectedName = "Cieszyn";
        Guid ownerId = Guid.NewGuid();
        
        // Act
        var team = Team.Create(expectedName, ownerId);
        var act = () => team.AssignCarFleet(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(act);
    }
}