using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class ShiftReport : BaseEntity
{
    public Guid UserId { get; private set; }
    public string OdometerPhotoUrl { get; private set; }
    public KilometersDriven KilometersDriven { get; private set; }
    public CardTransactionsSum CardTransactionsSum { get; private set; }
    public ReportStatus ReportStatus { get; private set; }
    public Guid CarId { get; private set; }
    
    public Car Car { get; set; }
    public User User { get; set; }
    
    protected ShiftReport(){}

    private ShiftReport(Guid userId, string odometerPhotoUrl, int kilometersDriven, float cardTransactionsSum, Guid carId)
    {
        UserId = userId;
        OdometerPhotoUrl = odometerPhotoUrl;
        KilometersDriven = KilometersDriven.Create(kilometersDriven);
        CardTransactionsSum = CardTransactionsSum.Create(cardTransactionsSum);
        ReportStatus = ReportStatus.Unsettled;
        CarId = carId;
    }

    #region ShiftReportFactorySummary
    /// <summary>
    /// Factory method that creates a new instance of a <see cref="ShiftReport"/> for a completed driver shift, validating core business parameters.
    /// </summary>
    /// <param name="userId">The unique identifier of the <see cref="User"/> (driver) submitting the report. Cannot be an empty GUID.</param>
    /// <param name="odometerPhotoUrl">The secure URL or path to the uploaded photo of the vehicle's odometer dashboard. Cannot be null or empty.</param>
    /// <param name="kilometersDriven">The total distance covered during the shift. Encapsulated and validated inside <see cref="KilometersDriven"/>.</param>
    /// <param name="cardTransactionsSum">The total sum of payments processed via card terminal during the shift. Encapsulated and validated inside <see cref="CardTransactionsSum"/>.</param>
    /// <param name="carId">The unique identifier of the <see cref="Car"/> used during this shift. Cannot be an empty GUID.</param>
    /// <returns>A fully initialized, business-valid <see cref="ShiftReport"/> entity with an initial unsettled status.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description>The <paramref name="userId"/> is equal to <see cref="Guid.Empty"/>.</description></item>
    /// <item><description>The <paramref name="carId"/> is equal to <see cref="Guid.Empty"/>.</description></item>
    /// <item><description>The <paramref name="odometerPhotoUrl"/> is null or empty.</description></item>
    /// <item><description>The validation for distance or card transactions fails within their respective Value Objects (e.g., negative values).</description></item>
    /// </list>
    /// </exception>
    #endregion
    public static ShiftReport Create(Guid userId, string odometerPhotoUrl, int kilometersDriven,
        float cardTransactionsSum, Guid carId)
    {
        if(userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty");

        if(carId == Guid.Empty)
            throw new ArgumentException("Car ID cannot be empty");

        if(string.IsNullOrEmpty(odometerPhotoUrl))
            throw new ArgumentException("Odometer photoUrl cannot be empty");
        
        return new ShiftReport(userId, odometerPhotoUrl, kilometersDriven, cardTransactionsSum,  carId);
    }

    public void UpdateReportStatus(ReportStatus status)
    {
        ReportStatus = status;
    }
}