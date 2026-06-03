using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.ValueObjects;
 
public record KilometerRate : ValueObject
{
    public float Value { get; }

    private KilometerRate(float value)
    {
        Value = value;
    }

    public static KilometerRate Create(float value)
    {
        if(value < 0)
            throw new ArgumentException("Kilometer rate must be greater than or equal to zero");

        return new KilometerRate(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new System.NotImplementedException();
    }
}