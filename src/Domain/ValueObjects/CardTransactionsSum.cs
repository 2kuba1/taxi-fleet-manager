using Domain.Common;

namespace Domain.ValueObjects;

public record CardTransactionsSum : ValueObject
{
    public float Value { get; }

    private CardTransactionsSum(float value)
    {
        Value = value;
    }

    public static CardTransactionsSum Create(float value)
    {
        if(value < 0)
            throw new ArgumentException("Value cannot be negative");

        return new CardTransactionsSum(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}