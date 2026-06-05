using Domain.Common;

namespace Domain.ValueObjects;

public record LicensePlate : ValueObject
{
    public string Value { get; }

    private LicensePlate(string value)
    {
        Value = value;
    }

    public static LicensePlate Create(string value)
    {
        if(string.IsNullOrEmpty(value) || value.Length < 3 || value.Length > 9)
            throw new ArgumentException("License plate number is invalid");
        
        var formattedValue = value.ToUpper().Replace(" ", "");
        
        return new LicensePlate(formattedValue);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}