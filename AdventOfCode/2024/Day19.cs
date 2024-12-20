using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public class Day19
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day19.txt");

    private readonly Dictionary<string, bool> _viablePatternCache = [];
    private readonly Dictionary<string, HashSet<string>> _patternOptionCache = [];
    private readonly HashSet<string> _patterns = [];
    private readonly int _minLength;
    private readonly int _maxLength;

    [AdventOfCode2024(19, 1)]
    public static int RunPart1()
    {
        var (startPatterns, targetPatterns) = ParseFile(InputPath);
        return CountViablePatterns(startPatterns, targetPatterns);
    }
    
    [AdventOfCode2024(19, 2)]
    public static int RunPart2()
    {
        var (startPatterns, targetPatterns) = ParseFile(InputPath);
        return CountTotalPatternOptions(startPatterns, targetPatterns);
    }

    public Day19(IEnumerable<string> patterns)
    {
        var possiblePatterns = patterns.ToHashSet();
        var lengths = possiblePatterns.Select(p => p.Length).ToList();
        _minLength = lengths.Min();
        _maxLength = lengths.Max();
        foreach (var pattern in possiblePatterns)
        {
            _patterns.Add(pattern);
            _viablePatternCache[pattern] = true;
            _patternOptionCache[pattern] = FindPatternOptions(pattern).Union([pattern]).ToHashSet();
        }
    }

    public int CountViablePatterns(IEnumerable<string> patterns)
    {
        return patterns.Count(IsViablePattern);
    }

    public int TotalPatternOptions(IEnumerable<string> patterns)
    {
        return patterns.Sum(s =>
        {
            var c = FindPatternOptions(s).Count;
            Console.Write(".");
            return c;
        });
    }

    public bool IsViablePattern(string pattern)
    {
        if (pattern.Length == 0)
        {
            return true;
        }

        if (_viablePatternCache.TryGetValue(pattern, out var viablePattern))
        {
            return viablePattern;
        }

        if (PossibleSubPatternLengths(pattern)
            .Select(l => (pattern[..l], pattern[l..]))
            .Any(x => IsViablePattern(x.Item1) && IsViablePattern(x.Item2)))
        {
            _viablePatternCache[pattern] = true;
            return true;
        }
        
        _viablePatternCache[pattern] = false;
        return false;
    }

    public int CountPatternOptions(string pattern) => FindPatternOptions(pattern).Count;
    
    public HashSet<string> FindPatternOptions(string pattern)
    {
        if (pattern.Length == 0 || !IsViablePattern(pattern))
        {
            _patternOptionCache[pattern] = [];
            return _patternOptionCache[pattern];
        }

        if (_patternOptionCache.TryGetValue(pattern, out var patternCount))
        {
            return patternCount;
        }

        var options = PossibleSubPatternLengths(pattern)
            .Select(l => (pattern[..l], pattern[l..]))
            .Where(x => _patterns.Contains(x.Item1))
            .SelectMany(x => FindPatternOptions(x.Item2)
                .Select(y => string.Join(".", x.Item1, y)))
            .ToHashSet();

        _patternOptionCache[pattern] = options;
        return options;
    }

    private IEnumerable<int> PossibleSubPatternLengths(string pattern)
    {
        for (int i = 1; i < pattern.Length; i++)
        {
            if (i > _maxLength)
            {
                break;
            }

            yield return i;
        }
    }

    public static (List<string>, List<string>) Parse(string input)
    {
        return ParseInner(input.SplitLines());
    }

    public static (List<string>, List<string>) ParseFile(string path)
    {
        return ParseInner(File.ReadLines(path));
    }

    private static (List<string>, List<string>) ParseInner(IEnumerable<string> lines)
    {
        var blocks = lines.SplitBlocks().ToList();
        var a = blocks[0].First().Split(',', StringSplitOptions.TrimEntries).ToList();
        var b = blocks[1];
        return (a, b);
    }

    public static int CountViablePatterns(string input)
    {
        var (startPatterns, targetPatterns) = Parse(input);
        return CountViablePatterns(startPatterns, targetPatterns);
    }

    public static int CountTotalPatternOptions(string input)
    {
        var (startPatterns, targetPatterns) = Parse(input);
        return CountTotalPatternOptions(startPatterns, targetPatterns);
    }

    private static int CountViablePatterns(IEnumerable<string> startPatterns, IEnumerable<string> targetPatterns)
    {
        var d = new Day19(startPatterns);
        return d.CountViablePatterns(targetPatterns);
    }

    private static int CountTotalPatternOptions(IEnumerable<string> startPatterns, IEnumerable<string> targetPatterns)
    {
        var d = new Day19(startPatterns);
        return d.TotalPatternOptions(targetPatterns);
    }
}
