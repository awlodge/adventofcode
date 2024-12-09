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
}
