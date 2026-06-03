using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;

namespace Domain.ValueObjects;

public record PhoneNumber : ValueObject
{
    public string Number { get; }
    public string AreaCode { get; }

    private PhoneNumber(string number, string areaCode)
    {
        Number = number;
        AreaCode = areaCode;
    }

    public static PhoneNumber Create(string number, string areaCode)
    {
        if (string.IsNullOrWhiteSpace(number) || string.IsNullOrWhiteSpace(areaCode))
            throw new ArgumentException("Phone number and area code can not be null or whitespace.");

        var cleanedNumber = new string(number.Where(char.IsDigit).ToArray());
        
        var cleanedAreaCode = new string(areaCode.Where(char.IsDigit).ToArray());

        if (cleanedNumber.Length != 9)
            throw new ArgumentException("Phone number must be 9 digits long");

        if (cleanedAreaCode.Length is < 1 or > 4)
            throw new ArgumentException("Area code must be 1 to 4 digits long");
        
        return new PhoneNumber(number, areaCode);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
        yield return AreaCode;
    }
}