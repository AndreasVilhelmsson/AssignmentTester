using Assignment;
using Xunit;

namespace Assignment.Tests
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData(5.5f, 3.3f, 8.8f)]
        [InlineData(-1.1f, 1.1f, 0f)]
        [InlineData(0f, 0f, 0f)]
        public void Add_ReturnsCorrectSum(float a, float b, float expected)
        {
            float result = Calculator.Add(a, b);
            Assert.Equal(expected, result, 1);
        }

        [Theory]
        [InlineData(5.5f, 3.3f, 2.2f)]
        [InlineData(-1.1f, 1.1f, -2.2f)]
        [InlineData(0f, 0f, 0f)]
        public void Subtract_ReturnsCorrectDifference(float a, float b, float expected)
        {
            float result = Calculator.Subtract(a, b);
            Assert.Equal(expected, result, 1);
        }

        [Theory]
        [InlineData(5.5f, 3.3f, 18.15f)]
        [InlineData(-1.1f, 1.1f, -1.21f)]
        [InlineData(0f, 0f, 0f)]
        public void Multiply_ReturnsCorrectProduct(float a, float b, float expected)
        {
            float result = Calculator.Multiply(a, b);
            Assert.Equal(expected, result, 2);
        }

        [Theory]
        [InlineData(6.6f, 3.3f, 2.0f)]
        [InlineData(-1.1f, 1.1f, -1.0f)]
        [InlineData(0f, 1f, 0f)]
        public void Divide_ReturnsCorrectQuotient(float a, float b, float expected)
        {
            float result = Calculator.Divide(a, b);
            Assert.Equal(expected, result, 1);
        }

        [Theory]
        [InlineData(6.6f, 0f)]
        [InlineData(-1.1f, 0f)]
        public void Divide_ByZero_ThrowsDivideByZeroException(float a, float b)
        {
            Assert.Throws<DivideByZeroException>(() => Calculator.Divide(a, b));
        }
    }
}