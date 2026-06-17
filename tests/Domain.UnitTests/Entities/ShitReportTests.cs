using Domain.Entities;
using Domain.Enums;
using Shouldly;

namespace Domain.UnitTests.Entities;

public class ShitReportTests
{
    [Fact]
    public void Create_WithValidData_CreatesReport()
    {
        //Arrange
        Guid expectedUserId = Guid.NewGuid();
        Guid expectedCarId = Guid.NewGuid();
        string expectedPhotoUrl = "https://some-link/photo.jpg";
        int expectedKilometers = 120;
        var expectedTransactionSum = 360.5f;
        
        //Act
        var report = ShiftReport.Create(
            expectedUserId,
            expectedPhotoUrl,
            expectedKilometers,
            expectedTransactionSum,
            DateTime.UtcNow,
            expectedCarId);
        
        //Assert
        report.ShouldNotBeNull();
        report.KilometersDriven.ShouldNotBeNull();
        report.CardTransactionsSum.ShouldNotBeNull();
        
        report.UserId.ShouldBe(expectedUserId);
        report.OdometerPhotoUrl.ShouldBe(expectedPhotoUrl);
        report.KilometersDriven.Value.ShouldBe(expectedKilometers);
        report.CardTransactionsSum.Value.ShouldBe(expectedTransactionSum);
        report.CarId.ShouldBe(expectedCarId);
        
        report.ReportStatus.ShouldBe(ReportStatus.Unsettled);
    }
    
    [Fact]
    public void Create_WithEmptyUserId_ShouldThrowArgumentException()
    {
        //Arrange
        Guid emptyUserId = Guid.Empty;
        Guid carId = Guid.NewGuid();
        string photoUrl = "https://some-link/photo.jpg";
        int kilometers = 50;
        float transactionsSum = 100.00f;

        //Act
        Action report = () => ShiftReport.Create(emptyUserId, photoUrl, kilometers, transactionsSum, DateTime.UtcNow, carId);

        //Assert
        Assert.Throws<ArgumentException>(report);
    }

    [Fact]
    public void Create_WithEmptyCarId_ShouldThrowArgumentException()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        Guid emptyCarId = Guid.Empty;
        string photoUrl = "https://some-link/photo.jpg";
        int kilometers = 50;
        float transactionsSum = 100.00f;

        //Act
        var report = () => ShiftReport.Create(userId, photoUrl, kilometers, transactionsSum, DateTime.UtcNow, emptyCarId);

        //Assert
        Assert.Throws<ArgumentException>(report);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_WithNullOrEmptyOdometerPhotoUrl_ShouldThrowArgumentException(string invalidPhotoUrl)
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        Guid carId = Guid.NewGuid();
        int kilometers = 50;
        float transactionsSum = 100.00f;

        //Act
        var report = () => ShiftReport.Create(userId, invalidPhotoUrl!, kilometers, transactionsSum, DateTime.UtcNow, carId);

        //Assert
        Assert.Throws<ArgumentException>(report);
    }

    [Theory]
    [InlineData(ReportStatus.Settled)]
    [InlineData(ReportStatus.Unsettled)]
    public void UpdateReportStatus_WithValidStatus_ShouldModifyReportStatusProperty(ReportStatus newStatus)
    {
        //Arrange
        var report = ShiftReport.Create(
            Guid.NewGuid(), 
            "https://storage.provider.com/photo.jpg", 
            100, 
            250.00f, 
            DateTime.UtcNow,
            Guid.NewGuid());

        //Act
        report.UpdateReportStatus(newStatus);

        //Assert
        report.ReportStatus.ShouldBe(newStatus);
    }
}