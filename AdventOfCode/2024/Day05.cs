using AdventOfCode.Helpers;

namespace AdventOfCode._2024;
using Rules = Dictionary<int, HashSet<int>>;

public static class Day05
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day05.txt");

    public static int RunPart1()
    {
        var parsedInput = ParseFile(InputPath);
        return parsedInput.SumMiddleValidUpdate();
    }

    public static int RunPart2()
    {
        var parsedInput = ParseFile(InputPath);
        return parsedInput.SumMiddleReorderedUpdates();
    }

    public static int SumMiddleValidUpdate(this ParsedInput parsedInput)
    {
        return parsedInput.Updates
            .Where(u => u.CheckUpdate(parsedInput.Rules))
            .Sum(u => u.GetMiddle());
    }

    public static int SumMiddleReorderedUpdates(this ParsedInput parsedInput)
    {
        return parsedInput.Updates
            .Where(u => !u.CheckUpdate(parsedInput.Rules))
            .Select(u => u.ReorderUpdate(parsedInput.Rules))
            .Sum(u => u.GetMiddle());
    }

    public static bool CheckUpdate(this List<int> update, Rules rules)
    {
        HashSet<int> previous = [];
        foreach (var page in update)
        {
            if (rules.TryGetValue(page, out var next) && next.Any(n => previous.Contains(n)))
            {
                return false;
            }
            previous.Add(page);
        }

        return true;
    }

    public static int GetMiddle(this List<int> update) => update[update.Count / 2];

    public static ParsedInput Parse(string input)
    {
        var lines = input.SplitLines(removeEmpty: false).ToList();
        return ParseInner(lines);
    }

    public static List<int> ReorderUpdate(this List<int> update, Rules rules)
    {
        update.Sort((int a, int b) =>
        {
            if (a == b)
            {
                return 0;
            }

            if (rules.TryGetValue(a, out var rulesa) && rulesa.Contains(b))
            {
                return -1;
            }

            if (rules.TryGetValue(b, out var rulesb) && rulesb.Contains(a))
            {
                return 1;
            }

            // Pathological case
            if (rules.TryGetValue(a, out var rulesa2))
            {
                foreach (var x in rulesa2)
                {
                    if (rules.TryGetValue(x, out var rulesx) && rulesx.Contains(b))
                    {
                        return -1;
                    }
                }
            }

            if (rules.TryGetValue(b, out var rulesb2))
            {
                foreach (var x in rulesb2)
                {
                    if (rules.TryGetValue(x, out var rulesx) && rulesx.Contains(a))
                    {
                        return 1;
                    }
                }
            }

            throw new InvalidOperationException($"Could not order {a} and {b}");
        });

        return update;
    }

    public static ParsedInput ParseFile(string path)
    {
        var lines = File.ReadLines(path).ToList();
        return ParseInner(lines);
    }

    private static ParsedInput ParseInner(List<string> lines)
    {
        Dictionary<int, HashSet<int>> rules = new();
        int i = 0;
        while (i < lines.Count && lines[i] != String.Empty)
        {
            var line = lines[i];
            var row = line.ParseAsInts(splitChars: ['|']).ToList();
            var rule = rules.GetValueOrDefault(row[0], []);
            rule.Add(row[1]);
            rules[row[0]] = rule;
            i++;
        }

        i++;
        List<List<int>> updates = new();
        while (i < lines.Count)
        {
            updates.Add(lines[i].ParseAsInts(splitChars: [',']).ToList());
            i++;
        }

        return new ParsedInput(rules, updates);
    }
}

public record ParsedInput(Rules Rules, List<List<int>> Updates);
