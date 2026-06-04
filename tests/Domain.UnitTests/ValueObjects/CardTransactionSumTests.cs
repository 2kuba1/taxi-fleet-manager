using Domain.ValueObjects;
using Shouldly;

namespace Domain.UnitTests.ValueObjects;

public class CardTransactionSumTests
{
    [Fact]
    public void Create_WithPositiveValue_CreatesCardSum()
    {
        //Arange
        float value = 99.99f;
        
        //Act
        var cardTransactionSum = CardTransactionsSum.Create(value);
        
        //Assert
        cardTransactionSum.ShouldNotBeNull();
        cardTransactionSum.Value.ShouldBe(value);
    }

    [Fact]
    public void Create_WithNegativeValue_ThrowsArgumentException()
    {
        //Arrange
        float value = -99.99f;
        
        //Act
        var cardTransactionSumFunc = () => CardTransactionsSum.Create(value);
        
        //Assert
        Assert.Throws<ArgumentException>(cardTransactionSumFunc);
    }
}