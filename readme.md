# Educational C# Console Calculator

A clean, modular, and extensible C# command-line calculator built on .NET. This project is specifically designed as an educational tool for teaching programming concepts such as tokenization, data structures (`List<T>`, `Stack<T>`, `Dictionary<K, V>`), expression parsing using the **Shunting-yard algorithm**, and software extensibility.

---

## 🚀 Features

- **In-Line Expression Parsing:** Evaluates full mathematical expressions entered in a single line (e.g., `(8 + 4.3) * 9.07`).
- **Standard Math Operations:** Supports addition (`+`), subtraction (`-`), multiplication (`*`), and division (`/`).
- **Parentheses Support:** Correctly handles nested parentheses and operator precedence.
- **Decimal Separator Normalization:** Accepts both dot (`.`) and comma (`,`) as decimal points automatically.
- **Extensible Architecture:** Easily register custom single-argument or multi-argument functions without modifying the core parsing algorithm.

---

## 🛠️ How It Works

The engine processes input expressions in three primary steps:

1. **Tokenization (`Tokenize`):** Converts the input text string into a list of individual tokens (numbers, operator symbols, function names, and parentheses).
2. **RPN Conversion (`ConvertToRpn`):** Converts the linear sequence of tokens into **Reverse Polish Notation** (Postfix notation) using Edsger Dijkstra's **Shunting-yard algorithm**.
3. **RPN Evaluation (`EvaluateRpn`):** Uses a stack-based algorithm to compute the final numeric result from the RPN token list.

---

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or higher installed on your machine.

---

## ⚙️ Getting Started

1. **Create a new Console project:**
   ```bash
   dotnet new console -n ConsoleCalculator
   cd ConsoleCalculator
   ```

2. **Replace the code:**
   Replace the contents of `Program.cs` with the provided C# calculator source code.

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Example Usage:**
   ```text
   === Educational Calculator (Console C#) ===
   Enter an expression (e.g., (8+4.3)*9.07 or abs(-5)):
   Press Enter on an empty line to exit.

   > (8+4.3)*9.07
   = 111.561

   > abs(-12.5) + sqrt(16)
   = 16.5

   > 
   ```

---

## 🎓 Extensibility & Teaching Assignments

This calculator is designed to serve as a base project for incremental learning. Here are suggested exercises for students:

### 1. Adding Trigonometric Functions
Students can add trigonometric functions by registering them inside the `RegisterBuiltInFunctions` method or via `RegisterFunction`:

```csharp
// Converting degrees to radians for trigonometric functions
calculator.RegisterFunction("sin", args => Math.Sin(args[0] * Math.PI / 180.0));
calculator.RegisterFunction("cos", args => Math.Cos(args[0] * Math.PI / 180.0));
calculator.RegisterFunction("tan", args => Math.Tan(args[0] * Math.PI / 180.0));
```

### 2. Adding Advanced Mathematical Functions
```csharp
calculator.RegisterFunction("log", args => Math.Log10(args[0]));
calculator.RegisterFunction("ln", args => Math.Log(args[0]));
calculator.RegisterFunction("exp", args => Math.Exp(args[0]));
```

### 3. Adding Constants (e.g., $\pi$, $e$)
Extend the tokenizer and RPN evaluator to support mathematical constants like `pi` and `e`.

### 4. Adding the Exponentiation Operator (`^`)
Modify `GetPrecedence` and `ExecuteOperator` to support exponentiation with right-associativity.

---

## 📄 License

This project is open-source and intended for educational purposes. Feel free to modify and adapt it for classroom or self-study use.