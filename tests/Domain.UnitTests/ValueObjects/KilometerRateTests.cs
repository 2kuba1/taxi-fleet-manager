using Domain.ValueObjects;
using Shouldly;

namespace Domain.UnitTests.ValueObjects;

public class KilometerRateTests
{
    [Fact]
    public void Create_WithValidKilometerRate_CreatesKilometerRate()
    {
        //Arrange
        float value = 20;
        
        //Act
        var kilometerRate = KilometerRate.Create(value);
        
        //Assert
        kilometerRate.ShouldNotBeNull();
        kilometerRate.Value.ShouldBe(value);
    }

    [Fact]
    public void Create_WithNegativeValue_ThrowsArgumentException()
    {
        //Arrange
        float value = -99.99f;
        
        //Act
        var kilometerRateFun = () => KilometerRate.Create(value);
        
        //Assert
        Assert.Throws<ArgumentException>(kilometerRateFun);
    }
}