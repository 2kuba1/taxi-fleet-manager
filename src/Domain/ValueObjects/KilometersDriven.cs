using Domain.Common;

namespace Domain.ValueObjects;

public record KilometersDriven : ValueObject
{
    public int Value { get; }

    private KilometersDriven(int value)
    {
        Value = value;
    }
    
    protected KilometersDriven(){}

    public static KilometersDriven Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("Value cannot be negative");

        return new KilometersDriven(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}