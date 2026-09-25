using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ConsoleCalculator;

class Program
{
    static int Main(string[] args)
    {
        Console.WriteLine("=== Educational Calculator (Console C#) ===");
        Console.WriteLine("Enter an expression (e.g., (8+4.3)*9.07 or abs(-5)):");
        Console.WriteLine("Supports +, -, *, /, parentheses, and built-in functions abs() and sqrt().");
        Console.WriteLine("Press Enter on an empty line to exit.\n");

        var calculator = new CalculatorEngine();

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                break;

            try
            {
                double result = calculator.Evaluate(input);
                Console.WriteLine($"= {result.ToString(CultureInfo.InvariantCulture)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine();
        }

        return 0;
    }
}

/// <summary>
/// The core calculator engine performing expression evaluation.
/// </summary>
public class CalculatorEngine
{
    // Registry for basic and advanced functions (ready for future lessons)
    private readonly Dictionary<string, Func<double[], double>> _functions;

    public CalculatorEngine()
    {
        _functions = new Dictionary<string, Func<double[], double>>(StringComparer.OrdinalIgnoreCase);
        RegisterBuiltInFunctions();
    }

    /// <summary>
    /// Registers basic built-in functions.
    /// </summary>
    private void RegisterBuiltInFunctions()
    {
        // Basic single-argument functions
        RegisterFunction("abs", args => Math.Abs(args[0]));
        RegisterFunction("sqrt", args => Math.Sqrt(args[0]));

        // --- ADD TRIGONOMETRY HERE IN THE NEXT LESSON ---
        // RegisterFunction("sin", args => Math.Sin(args[0]));
        // RegisterFunction("cos", args => Math.Cos(args[0]));
    }

    /// <summary>
    /// Registers a new custom function to easily extend the calculator.
    /// </summary>
    public void RegisterFunction(string name, Func<double[], double> function)
    {
        _functions[name] = function;
    }

    /// <summary>
    /// Evaluates a mathematical expression string using RPN (Shunting-yard algorithm).
    /// </summary>
    public double Evaluate(string expression)
    {
        var tokens = Tokenize(expression);
        var rpnTokens = ConvertToRpn(tokens);
        return EvaluateRpn(rpnTokens);
    }

    #region 1. Tokenization

    private List<string> Tokenize(string expression)
    {
        var tokens = new List<string>();
        int index = 0;

        while (index < expression.Length)
        {
            char current = expression[index];

            if (char.IsWhiteSpace(current))
            {
                index++;
                continue;
            }

            // Number parsing (handles both dot and comma as decimal separator, as well as leading minus sign)
            bool isLeadingMinus = current == '-' && 
                (tokens.Count == 0 || tokens[^1] == "(" || IsOperator(tokens[^1])) &&
                index + 1 < expression.Length && (char.IsDigit(expression[index + 1]) || expression[index + 1] == '.' || expression[index + 1] == ',');

            if (char.IsDigit(current) || current == '.' || current == ',' || isLeadingMinus)
            {
                var sb = new StringBuilder();
                if (isLeadingMinus)
                {
                    sb.Append('-');
                    index++;
                }

                while (index < expression.Length && (char.IsDigit(expression[index]) || expression[index] == '.' || expression[index] == ','))
                {
                    // Standardize decimal separator to dot for invariant parsing
                    char digitChar = expression[index] == ',' ? '.' : expression[index];
                    sb.Append(digitChar);
                    index++;
                }
                tokens.Add(sb.ToString());
                continue;
            }

            // Function names or identifiers (e.g., sin, cos, abs)
            if (char.IsLetter(current))
            {
                var sb = new StringBuilder();
                while (index < expression.Length && char.IsLetterOrDigit(expression[index]))
                {
                    sb.Append(expression[index]);
                    index++;
                }
                tokens.Add(sb.ToString());
                continue;
            }

            // Operators and parentheses
            if ("+-*/()".Contains(current))
            {
                tokens.Add(current.ToString());
                index++;
                continue;
            }

            throw new ArgumentException($"Invalid character in expression: '{current}'");
        }

        return tokens;
    }

    #endregion

    #region 2. Conversion to RPN (Shunting-yard Algorithm)

    private List<string> ConvertToRpn(List<string> tokens)
    {
        var output = new List<string>();
        var operatorStack = new Stack<string>();

        foreach (var token in tokens)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                output.Add(token);
            }
            else if (_functions.ContainsKey(token))
            {
                operatorStack.Push(token);
            }
            else if (IsOperator(token))
            {
                while (operatorStack.Count > 0 && IsOperator(operatorStack.Peek()) &&
                       GetPrecedence(operatorStack.Peek()) >= GetPrecedence(token))
                {
                    output.Add(operatorStack.Pop());
                }
                operatorStack.Push(token);
            }
            else if (token == "(")
            {
                operatorStack.Push(token);
            }
            else if (token == ")")
            {
                while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                {
                    output.Add(operatorStack.Pop());
                }

                if (operatorStack.Count == 0)
                    throw new ArgumentException("Mismatched parentheses in expression.");

                operatorStack.Pop(); // Remove '('

                if (operatorStack.Count > 0 && _functions.ContainsKey(operatorStack.Peek()))
                {
                    output.Add(operatorStack.Pop());
                }
            }
        }

        while (operatorStack.Count > 0)
        {
            string op = operatorStack.Pop();
            if (op == "(" || op == ")")
                throw new ArgumentException("Mismatched parentheses in expression.");
            output.Add(op);
        }

        return output;
    }

    #endregion

    #region 3. RPN Evaluation

    private double EvaluateRpn(List<string> rpnTokens)
    {
        var stack = new Stack<double>();

        foreach (var token in rpnTokens)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                stack.Push(value);
            }
            else if (IsOperator(token))
            {
                if (stack.Count < 2)
                    throw new InvalidOperationException("Invalid expression (not enough operands).");

                double operandB = stack.Pop();
                double operandA = stack.Pop();

                stack.Push(ExecuteOperator(token, operandA, operandB));
            }
            else if (_functions.TryGetValue(token, out var func))
            {
                if (stack.Count < 1)
                    throw new InvalidOperationException($"Missing argument for function '{token}'.");

                double argument = stack.Pop();
                stack.Push(func(new[] { argument }));
            }
        }

        if (stack.Count != 1)
            throw new InvalidOperationException("Invalid expression.");

        return stack.Pop();
    }

    #endregion

    #region Helper Methods for Operators

    private bool IsOperator(string token) => token is "+" or "-" or "*" or "/";

    private int GetPrecedence(string op) => op switch
    {
        "+" or "-" => 1,
        "*" or "/" => 2,
        _ => 0
    };

    private double ExecuteOperator(string op, double leftOperand, double rightOperand) => op switch
    {
        "+" => leftOperand + rightOperand,
        "-" => leftOperand - rightOperand,
        "*" => leftOperand * rightOperand,
        "/" => rightOperand != 0 ? leftOperand / rightOperand : throw new DivideByZeroException("Division by zero."),
        _ => throw new InvalidOperationException($"Unknown operator: '{op}'")
    };

    #endregion
}