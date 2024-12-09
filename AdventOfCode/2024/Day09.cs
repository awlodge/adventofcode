using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public static class Day09
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day09.txt");

    public static long RunPart1()
    {
        return File.ReadAllText(InputPath).GetMemoryMap().Collapse().Checksum();
    }

    public static long Checksum(this List<int> memory)
    {
        return memory.Each<int, long>((x, i) => x == -1 ? 0 : x * i).Sum();
    }

    public static List<int> Collapse(this List<int> memory)
    {
        var emptyIx = memory.GetNextEmpty(0);
        var endIx = memory.GetPrevFull(memory.Count - 1);
        while (emptyIx < endIx)
        {
            memory[emptyIx] = memory[endIx];
            memory[endIx] = -1;
            emptyIx = memory.GetNextEmpty(emptyIx);
            endIx = memory.GetPrevFull(endIx);
        }

        return memory;
    }

    private static int GetNextEmpty(this List<int> memory, int ix)
    {
        while (memory[ix] != -1)
        {
            ix++;
        }

        return ix;
    }

    private static int GetPrevFull(this List<int> memory, int ix)
    {
        while (memory[ix] == -1)
        {
            ix--;
        }

        return ix;
    }

    public static List<int> GetMemoryMap(this string input)
    {
        List<int> result = [];
        bool isFile = true;
        int i = 0;
        int fileIx = 0;
        foreach (char x in input)
        {
            var fileLen = (int)Char.GetNumericValue(x);
            var val = isFile ? fileIx : -1;
            for (int j = 0; j < fileLen; j++)
            {
                result.Add(val);
            }
            i += fileLen;

            if (isFile)
            {
                fileIx++;
                isFile = false;
            }
            else
            {
                isFile = true;
            }
        }

        return result;
    }

    public static string MemoryMapAsString(this List<int> memory)
    {
        return String.Join("", memory.Select(x => x == -1 ? "." : x.ToString()));
    }
}
