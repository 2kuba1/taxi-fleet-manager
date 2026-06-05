using Domain.ValueObjects;
using Shouldly;

namespace Domain.UnitTests.ValueObjects;

public class LicensePlateTests
{
    [Fact]
    public void Create_WithValidLicensePlate_CreatesLicensePlate()
    {
        //Arrange
        string validLicensePlate = "SCI 6968M";
        string expectedOutput = "SCI6968M";
        //Act
        var licensePlate = LicensePlate.Create(validLicensePlate);
        
        //Assert
        licensePlate.ShouldNotBeNull();
        licensePlate.Value.ShouldBe(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(GetInvalidLicensePlates))]
    public void Create_WithInvalidLicensePlate_ThrowsArgumentException(string invalidLicensePlate)
    {
        //Act
        var licensePlate = () => LicensePlate.Create(invalidLicensePlate);
        
        //Assert
        Assert.Throws<ArgumentException>(licensePlate);
    }

    [Fact]
    public void Equals_WithSameValues_ShouldBeTrue()
    {
        var plate1 = LicensePlate.Create("SCI 6968M");
        var plate2 = LicensePlate.Create("sci6968m");
        
        plate1.ShouldBe(plate2);
        (plate1 == plate2).ShouldBeTrue();
    }
    
    [Fact]
    public void Equals_WithDifferentValues_ShouldBeFalse()
    {
        var plate1 = LicensePlate.Create("SCI 6968M");
        var plate2 = LicensePlate.Create("SB 6968m");
        
        plate1.ShouldNotBe(plate2);
        (plate1 == plate2).ShouldBeFalse();
    }
    
    public static IEnumerable<object[]> GetInvalidLicensePlates()
    {
        yield return new object[] { "" };
        yield return new object[] { "S" };
        yield return new object[] { "SC" };
        yield return new object[] { "1234567890" };
    }
}