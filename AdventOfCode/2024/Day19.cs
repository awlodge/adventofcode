using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public class Day19
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day19.txt");

    private readonly HashSet<string> _possiblePatterns;
    private readonly int _minLength;
    private readonly int _maxLength;

    [AdventOfCode2024(19, 1)]
    public static int RunPart1()
    {
        (var startPatterns, var targetPatterns) = ParseFile(InputPath);
        return CountViablePatterns(startPatterns, targetPatterns);
    }

    public Day19(IEnumerable<string> patterns)
    {
        _minLength = patterns.Select(s => s.Length).Min();
        _maxLength = patterns.Select(s => s.Length).Max();
        _possiblePatterns = new HashSet<string>(patterns);
    }

    public int CountViablePatterns(IEnumerable<string> patterns)
    {
        return patterns.Count(p => IsViablePattern(p));
    }

    public bool IsViablePattern(string pattern)
    {
        if (pattern.Length == 0)
        {
            return true;
        }

        if (_possiblePatterns.Contains(pattern))
        {
            return true;
        }

        foreach (var l in PossibleSubPatternLengths(pattern))
        {
            var a = pattern[..l];
            var b = pattern[l..];
            if (_possiblePatterns.Contains(a) && IsViablePattern(b))
            {
                _possiblePatterns.Add(pattern);
                return true;
            }
        }

        return false;
    }

    private IEnumerable<int> PossibleSubPatternLengths(string pattern)
    {
        for (int i = _minLength; i < pattern.Length; i++)
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
        (var startPatterns, var targetPatterns) = Parse(input);
        return CountViablePatterns(startPatterns, targetPatterns);
    }

    private static int CountViablePatterns(IEnumerable<string> startPatterns, IEnumerable<string> targetPatterns)
    {
        var d = new Day19(startPatterns);
        return d.CountViablePatterns(targetPatterns);
    }
}

internal class InfiniteTrie
{
    private readonly InfiniteTrieNode _root;
    public InfiniteTrie(IEnumerable<string> patterns)
    {
        _root = new();
        foreach (var pattern in patterns)
        {
            Add(pattern);
        }
    }

    public void Add(string pattern)
    {
        var node = _root;
        foreach (var c in pattern)
        {
            node = node.Add(c);
        }
        node.SetFallback(_root);
    }
}

internal class InfiniteTrieNode
{
    private readonly Dictionary<char, InfiniteTrieNode> _children = [];
    private InfiniteTrieNode? _fallback;

    public InfiniteTrieNode Add(char s)
    {
        if (!_children.TryGetValue(s, out var child))
        {
            child = new();
            _children[s] = child;
        }
        return child;
    }

    public void SetFallback(InfiniteTrieNode fallback)
    {
        _fallback = fallback;
    }
}