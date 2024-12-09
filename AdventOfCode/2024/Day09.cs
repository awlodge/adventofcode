using AdventOfCode.Helpers;

namespace AdventOfCode._2024;

public static class Day09
{
    private static readonly string InputPath = Path.Combine(Environment.CurrentDirectory, "2024/inputs/day09.txt");

    [AdventOfCode2024(9, 1)]
    public static long RunPart1()
    {
        return File.ReadAllText(InputPath).GetMemoryMap().Collapse().Checksum();
    }

    [AdventOfCode2024(9, 2)]
    public static long RunPart2()
    {
        return File.ReadAllText(InputPath).GetMemoryMap().Defrag().Checksum();
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
            if (ix >= memory.Count)
            {
                return -1;
            }
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

    public static List<int> Defrag(this List<int> memory)
    {
        var emptyBlocks = memory.GetAllEmptyBlocks();
        var endIx = memory.GetPrevFile(memory.Count - 1, out var fileLen);
        while (endIx > 0)
        {
            for (int blockIx = 0; blockIx < emptyBlocks.Count; blockIx++)
            {
                var block = emptyBlocks[blockIx];
                var emptyIx = block[0];
                var emptyBlockLen = block[1];

                if (emptyIx < endIx && fileLen <= emptyBlockLen)
                {
                    for (int j = 0; j < fileLen; j++)
                    {
                        memory[emptyIx + j] = memory[endIx - j];
                        memory[endIx - j] = -1;
                    }

                    emptyBlocks[blockIx] = [emptyIx + fileLen, emptyBlockLen - fileLen];
                    break;
                }
            }
            endIx = memory.GetPrevFile(endIx - fileLen, out fileLen);
        }

        return memory;
    }

    private static int GetNextEmptyBlock(this List<int> memory, int ix, out int len)
    {
        ix = memory.GetNextEmpty(ix);
        len = 0;
        if (ix != -1)
        {
            while ((ix + len) < memory.Count && memory[ix + len] == -1)
            {
                len++;
            }
        }

        return ix;
    }

    private static List<int[]> GetAllEmptyBlocks(this List<int> memory)
    {
        List<int[]> result = [];
        int ix = 0;
        while (ix < memory.Count)
        {
            ix = memory.GetNextEmptyBlock(ix, out var len);
            if (ix == -1)
            {
                break;
            }
            result.Add([ix, len]);
            ix += len;
        }

        return result;
    }

    private static int GetPrevFile(this List<int> memory, int ix, out int len)
    {
        ix = memory.GetPrevFull(ix);
        len = 0;
        while ((ix - len) > 0 && memory[ix - len] == memory[ix])
        {
            len++;
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
