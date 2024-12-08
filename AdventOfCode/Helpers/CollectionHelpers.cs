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
}
