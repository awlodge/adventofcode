namespace AdventOfCode.Helpers;

[AttributeUsage(AttributeTargets.Method)]
public class AdventOfCodeAttribute(int year, int day, int part) : Attribute, IComparable
{
    public int Year { get; } = year;
    public int Day { get; } = day;
    public int Part { get; } = part;

    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is not AdventOfCodeAttribute attrObj)
        {
            return 1;
        }

        if (Year != attrObj.Year)
        {
            return Year > attrObj.Year ? 1 : -1;
        }

        if (Day != attrObj.Day)
        {
            return Day > attrObj.Day ? 1 : -1;
        }

        if (Part != attrObj.Part)
        {
            return Part > attrObj.Part ? 1 : -1;
        }

        return 0;
    }
}

internal class AdventOfCode2024Attribute(int day, int part) : AdventOfCodeAttribute(2024, day, part);