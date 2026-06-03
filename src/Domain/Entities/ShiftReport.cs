using System;
using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class ShiftReport : BaseEntity
{
    public Guid UserId { get; private set; }
    public string OdometerPhotoUrl { get; private set; }
    public KilometersDriven KilometersDriven { get; private set; }
    public CardTransactionsSum CardTransactionsSum { get; private set; }

    public User User { get; set; }
    
    protected ShiftReport(){}

    private ShiftReport(Guid userId, string odometerPhotoUrl, int kilometersDriven, float cardTransactionsSum)
    {
        UserId = userId;
        OdometerPhotoUrl = odometerPhotoUrl;
        KilometersDriven = KilometersDriven.Create(kilometersDriven);
        CardTransactionsSum = CardTransactionsSum.Create(cardTransactionsSum);
    }

    public static ShiftReport Create(Guid userId, string odometerPhotoUrl, int kilometersDriven,
        float cardTransactionsSum)
    {
        if(string.IsNullOrEmpty(odometerPhotoUrl))
            throw new ArgumentException("Odometer photoUrl cannot be empty");
        
        return new ShiftReport(userId, odometerPhotoUrl, kilometersDriven, cardTransactionsSum);
    }
}