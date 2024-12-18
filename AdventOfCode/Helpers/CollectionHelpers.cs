namespace AdventOfCode.Helpers;

public static class CollectionHelpers
{
    public static IEnumerable<List<T>> Pairs<T>(this IEnumerable<T> input)
    {
        var inputList = input.ToList();
        for (int i = 0; i < inputList.Count; i++)
        {
            for (int j = i + 1; j < inputList.Count; j++)
            {
                yield return [inputList[i], inputList[j]];
            }
        }
    }

    public static IEnumerable<TResult> Each<T, TResult>(this IEnumerable<T> ie, Func<T, int, TResult> action)
    {
        var i = 0;
        foreach (var e in ie)
        {
            yield return action(e, i++);
        }
    }

    public static T BinarySearch<T>(this IList<T> input, Func<T, bool> predicate)
    {
        var start = 0;
        var end = input.Count;
        int midPoint = 0;
        while (start < end)
        {
            midPoint = (start + end) / 2;
            if (predicate(input[midPoint]))
            {
                end = midPoint - 1;
            }
            else
            {
                start = midPoint + 1;
            }
        }

        return input[midPoint];
    }
}
