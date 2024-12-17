using AdventOfCode.Helpers;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024;

public static partial class Day17
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day17.txt");

    [GeneratedRegex("Register [ABC]: (\\d+)")]
    private static partial Regex RegisterRegex();

    [GeneratedRegex("Program: ((\\d,?)+)")]
    private static partial Regex ProgramRegex();

    [AdventOfCode2024(17, 1)]
    public static string RunPart1()
    {
        var computer = ParseFile(InputPath);
        computer.Execute();
        return computer.PrintOutput();
    }

    public static string TestComputer(string input)
    {
        var computer = Parse(input);
        computer.Execute();
        return computer.PrintOutput();
    }

    public static List<int> TestComputer(List<int> program,
        int a, int b, int c,
        out int outA, out int outB, out int outC)
    {
        var computer = new Computer(program, a, b, c);
        computer.Execute();
        outA = computer.A;
        outB = computer.B;
        outC = computer.C;
        return computer.Outputs;
    }

    private static Computer Parse(string input) =>
        input.SplitLines().ParseInner();

    private static Computer ParseFile(string path) =>
        File.ReadLines(path).ParseInner();

    private static Computer ParseInner(this IEnumerable<string> lines)
    {
        var blocks = lines.SplitBlocks().ToList();
        var regLines = blocks[0];
        var a = ParseRegister(regLines[0]);
        var b = ParseRegister(regLines[1]);
        var c = ParseRegister(regLines[2]);

        var program = ParseProgram(blocks[1][0]);

        return new Computer(program, a, b, c);

        int ParseRegister(string input) => Int32.Parse(RegisterRegex().Match(input).Groups[1].Value);

        List<int> ParseProgram(string input) => ProgramRegex()
            .Match(input).Groups[1].Value
            .Split(',')
            .Select(c => Int32.Parse(c))
            .ToList();
    }
}

internal class Computer
{
    private readonly List<int> _operations;
    private int _instructionPtr;
    private bool _moveNext;

    public Computer(List<int> operations, int a = 0, int b = 0, int c = 0)
    {
        _operations = operations;
        A = a;
        B = b;
        C = c;
        Outputs = [];
    }

    public int A { get; private set; }

    public int B { get; private set; }

    public int C { get; private set; }

    public List<int> Outputs { get; private set; }

    public void Execute()
    {
        while (_instructionPtr < _operations.Count - 1)
        {
            _moveNext = true;
            var opCode = _operations[_instructionPtr];
            var operand = _operations[_instructionPtr + 1];
            ExecuteOp(opCode, operand);

            if (_moveNext)
            {
                _instructionPtr += 2;
            }
        }
    }

    public string PrintOutput() => String.Join(',', Outputs);

    private void ExecuteOp(int opCode, int operand)
    {
        switch (opCode)
        {
            case 0:
                Adv(Combo(operand));
                break;
            case 1:
                Bxl(Lit(operand));
                break;
            case 2:
                Bst(Combo(operand));
                break;
            case 3:
                Jnz(Lit(operand));
                break;
            case 4:
                Bxc();
                break;
            case 5:
                Out(Combo(operand));
                break;
            case 6:
                Bdv(Combo(operand));
                break;
            case 7:
                Cdv(Combo(operand));
                break;
            default:
                throw new ArgumentException($"Invalid opCode: {opCode}", nameof(opCode));
        }
    }

    private static int Lit(int operand) => operand;

    private int Combo(int operand) => operand switch
    {
        0 => 0,
        1 => 1,
        2 => 2,
        3 => 3,
        4 => A,
        5 => B,
        6 => C,
        _ => throw new ArgumentException($"Invalid operand: {operand}", nameof(operand))
    };

    private void Adv(int input)
    {
        A = Div(input);
    }

    private void Bxl(int input)
    {
        B ^= input;
    }

    private void Bst(int input)
    {
        B = input % 8;
    }

    private void Jnz(int input)
    {
        if (A != 0)
        {
            _instructionPtr = input;
            _moveNext = false;
        }
    }

    private void Bxc()
    {
        B ^= C;
    }

    private void Out(int input)
    {
        Outputs.Add(input % 8);
    }

    private void Bdv(int input)
    {
        B = Div(input);
    }

    private void Cdv(int input)
    {
        C = Div(input);
    }

    private int Div(int input) => (int)(A / Math.Pow(2, input));
}
