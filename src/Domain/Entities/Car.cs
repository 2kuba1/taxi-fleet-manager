using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Car : BaseEntity
{
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public LicensePlate LicensePlate { get; private set; }
    public Vin VinNumber { get; private set; }
    public DateTime InspectionDate { get; private set; }
    public DateTime InsuranceRenewalDate { get; private set; }
    
    public Guid CarFleetId { get; set; }
    public CarFleet CarFleet { get; set; }
    
    protected Car(){}
    
    private Car(string brand, string model, string licensePlate, string vin, DateTime inspectionDate, DateTime insuranceRenewalDate)
    {
        Brand = brand;
        Model = model;
        LicensePlate = LicensePlate.Create(licensePlate);
        VinNumber = Vin.Create(vin);
        InspectionDate = inspectionDate;
        InsuranceRenewalDate = insuranceRenewalDate;
    }

    #region CarFactorySummary
    /// <summary>
    /// Factory method that creates a new instance of a <see cref="Car"/>, validating its brand, model, and formatting its license plate.
    /// </summary>
    /// <param name="brand">The brand or manufacturer of the car (e.g., "Skoda", "Toyota"). Cannot be null, empty, or whitespace.</param>
    /// <param name="model">The specific model of the car (e.g., "Octavia", "Corolla"). Cannot be null or empty.</param>
    /// <param name="licensePlate">The registration license plate string. Automatically parsed, cleaned, and validated inside <see cref="LicensePlate"/>.</param>
    /// <param name="vin">The vin number string. Cannot be null or empty and has to be exactly 17 characters long<see cref="Vin"/>.</param>
    /// <param name="inspectionDate">The date of passing the inspection.</param>
    /// <param name="insuranceRenewalDate">The date of renewal the insurance.</param>
    /// <returns>A fully initialized and business-valid <see cref="Car"/> entity.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description>The <paramref name="brand"/> is null, empty, or consists only of white-space characters.</description></item>
    /// <item><description>The <paramref name="model"/> is null or empty.</description></item>
    /// <item><description>The <paramref name="licensePlate"/> is null, empty, or fails the underlying alphanumeric length checks.</description></item>
    /// <item><description>The <paramref name="vin"/> is null, empty, or fails the 17 characters check</description></item>
    /// </list>
    /// </exception>
    #endregion
    public static Car Create(string brand, string model, string licensePlate, string vin, DateTime inspectionDate, DateTime insuranceRenewalDate)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Car brand cannot be empty");

        if (string.IsNullOrEmpty(model))
            throw new ArgumentException("Car model cannot be empty");

        if (string.IsNullOrEmpty(licensePlate))
            throw new ArgumentException("License plate cannot be empty");
        
        return new Car(brand, model, licensePlate, vin,  inspectionDate, insuranceRenewalDate);
    }
}