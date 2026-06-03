using System;
using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Car : BaseEntity
{
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public LicensePlate LicensePlate { get; private set; }
    
    public Guid CarFleetId { get; set; }
    public CarFleet CarFleet { get; set; }
    
    protected Car(){}
    
    private Car(string brand, string model, string licensePlate)
    {
        Brand = brand;
        Model = model;
        LicensePlate = LicensePlate.Create(licensePlate);
    }

    public static Car Create(string brand, string model, string licensePlate)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Car brand cannot be empty");

        if (string.IsNullOrEmpty(model))
            throw new ArgumentException("Car model cannot be empty");

        if (string.IsNullOrEmpty(licensePlate))
            throw new ArgumentException("License plate cannot be empty");
        
        return new Car(brand, model, licensePlate);
    }
}