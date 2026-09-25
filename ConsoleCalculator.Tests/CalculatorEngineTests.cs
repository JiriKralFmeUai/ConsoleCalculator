using System;
using Xunit;
using ConsoleCalculator;

namespace ConsoleCalculator.Tests;

public class CalculatorEngineTests
{
    private readonly CalculatorEngine _calculator = new();

    #region Basic Arithmetic Tests

    [Theory]
    [InlineData("2 + 3", 5.0)]
    [InlineData("10 - 4", 6.0)]
    [InlineData("6 * 7", 42.0)]
    [InlineData("20 / 4", 5.0)]
    public void Evaluate_BasicArithmetic_ReturnsExpectedResult(string expression, double expected)
    {
        double actual = _calculator.Evaluate(expression);
        Assert.Equal(expected, actual, precision: 9);
    }

    #endregion

    #region Operator Precedence & Parentheses Tests

    [Theory]
    [InlineData("2 + 3 * 4", 14.0)]
    [InlineData("(2 + 3) * 4", 20.0)]
    [InlineData("((8 + 2) * 3) / (2 + 3)", 6.0)]
    public void Evaluate_OperatorPrecedenceAndParentheses_ReturnsExpectedResult(string expression, double expected)
    {
        double actual = _calculator.Evaluate(expression);
        Assert.Equal(expected, actual, precision: 9);
    }

    #endregion

    #region Decimal Numbers & Separator Normalization Tests

    [Theory]
    [InlineData("4.5 * 2", 9.0)]
    [InlineData("4,5 * 2", 9.0)]
    [InlineData("1,5 + 2.5", 4.0)]
    [InlineData("(8+4.3)*9.07", 111.561)]
    public void Evaluate_DecimalSeparators_ReturnsExpectedResult(string expression, double expected)
    {
        double actual = _calculator.Evaluate(expression);
        Assert.Equal(expected, actual, precision: 9);
    }

    #endregion

    #region Negative Numbers (Unary Minus) Tests

    [Theory]
    [InlineData("-5 + 3", -2.0)]
    [InlineData("10 + -4", 6.0)]
    [InlineData("(-5)", -5.0)]
    public void Evaluate_LeadingNegativeNumbers_ReturnsExpectedResult(string expression, double expected)
    {
        double actual = _calculator.Evaluate(expression);
        Assert.Equal(expected, actual, precision: 9);
    }

    #endregion

    #region Built-in Functions Tests

    [Theory]
    [InlineData("abs(-5)", 5.0)]
    [InlineData("sqrt(16)", 4.0)]
    [InlineData("abs(-12.5) + sqrt(16)", 16.5)]
    public void Evaluate_BuiltInFunctions_ReturnsExpectedResult(string expression, double expected)
    {
        double actual = _calculator.Evaluate(expression);
        Assert.Equal(expected, actual, precision: 9);
    }

    #endregion

    #region Custom Function Registration Tests

    [Fact]
    public void RegisterFunction_CustomCubeFunction_EvaluatesCorrectly()
    {
        var calculator = new CalculatorEngine();
        calculator.RegisterFunction("cube", args => args[0] * args[0] * args[0]);

        double actual = calculator.Evaluate("cube(3)");

        Assert.Equal(27.0, actual, precision: 9);
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public void Evaluate_DivisionByZero_ThrowsDivideByZeroException()
    {
        Assert.Throws<DivideByZeroException>(() => _calculator.Evaluate("10 / 0"));
    }

    [Theory]
    [InlineData("(2 + 3")]
    [InlineData("2 + 3)")]
    public void Evaluate_MismatchedParentheses_ThrowsArgumentException(string expression)
    {
        Assert.Throws<ArgumentException>(() => _calculator.Evaluate(expression));
    }

    [Fact]
    public void Evaluate_InvalidCharacter_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _calculator.Evaluate("2 + $3"));
    }

    [Fact]
    public void Evaluate_MissingOperand_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => _calculator.Evaluate("2 + "));
    }

    #endregion
}
