using Domain.Entities;
using Shouldly;

namespace Domain.UnitTests.Entities;

public class WorkShiftTests
{
    [Fact]
    public void Create_ForValidData_CreatesWorkShift()
    {
        //Arrange 
        Guid userId = Guid.NewGuid();
        DateTime startTime =  DateTime.Now;
        DateTime endTime = startTime.AddHours(1);
        
        //Act
        var workShift = WorkShift.Create(userId, startTime, endTime);
        
        //Assert
        workShift.ShouldNotBeNull();
        workShift.UserId.ShouldBe(userId);
        workShift.StartTime.ShouldBe(startTime);
        workShift.EndTime.ShouldBe(endTime);
    }

    [Fact]
    public void Create_WithEmptyUserId_ShouldThrowArgumentException()
    {
        //Arrange
        Guid emptyUserId = Guid.Empty;
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime.AddHours(1);
        
        //Act
        var workShift = () => WorkShift.Create(emptyUserId, startTime, endTime);
        
        //Assert
        Assert.Throws<ArgumentException>(workShift);
    }
    
    [Fact]
    public void Create_WithStartTimeEqualToEndTime_ShouldThrowArgumentException()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        DateTime startTime = DateTime.Now;
        DateTime endTime = startTime;
        
        //Act
        var workShift = () => WorkShift.Create(userId, startTime, endTime);
        
        //Assert
        Assert.Throws<ArgumentException>(workShift);
    }
}