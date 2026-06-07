using Domain.Common;

namespace Domain.ValueObjects;

public record Vin : ValueObject
{
    public string Value { get; }

    private Vin(string value)
    {
        Value = value;
    }

    public static Vin Create(string value)
    {
        if(string.IsNullOrEmpty(value) || value.Length != 17)
            throw new ArgumentException("Invalid Vin value");
        
        return new Vin(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}