using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public class Day19
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day19.txt");

    private readonly InfiniteTrie _trie;

    [AdventOfCode2024(19, 1)]
    public static int RunPart1()
    {
        (var startPatterns, var targetPatterns) = ParseFile(InputPath);
        return CountViablePatterns(startPatterns, targetPatterns);
    }

    public Day19(IEnumerable<string> patterns)
    {
        _trie = new InfiniteTrie(patterns);
    }

    public int CountViablePatterns(IEnumerable<string> patterns)
    {
        return patterns.Count(p => IsViablePattern(p));
    }

    public bool IsViablePattern(string pattern) => _trie.Search(pattern);

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

    private static int CountViablePatterns(IEnumerable<string> startPatterns, IEnumerable<string> targetPatterns)
    {
        var d = new Day19(startPatterns);
        return d.CountViablePatterns(targetPatterns);
    }
}

internal class InfiniteTrie
{
    private readonly InfiniteTrieNode _root;

    public InfiniteTrie()
    {
        _root = new();
    }
    
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

    public bool Search(string pattern)
    {
        var node = _root;
        foreach (var c in pattern)
        {
            if (!node!.Search(c, out node))
            {
                return false;
            }
        }

        return true;
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

    public virtual bool Search(char c, out InfiniteTrieNode? next)
    {
        next = null;
        return _fallback != null ? Combine(_fallback).Search(c, out next) : _children.TryGetValue(c, out next);
    }

    private CombinedInfiniteTrieNode Combine(InfiniteTrieNode other) => new(this, other);
}

internal class CombinedInfiniteTrieNode(InfiniteTrieNode a, InfiniteTrieNode b) : InfiniteTrieNode
{
    public override bool Search(char c, out InfiniteTrieNode? next)
    {
        var searchA = a.Search(c, out var nextA);
        var searchB = b.Search(c, out var nextB);

        if (searchA && searchB)
        {
            return (new CombinedInfiniteTrieNode(nextA!, nextB!)).Search(c, out next);
        }
        
        if (searchA)
        {
            next = nextA;
            return true;
        }

        if (searchB)
        {
            next = nextB;
            return true;
        }

        next = null;
        return false;
    }
}