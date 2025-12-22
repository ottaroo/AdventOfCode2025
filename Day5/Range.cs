using System.Collections;

namespace Day5;

public class Range(long min, long max)
{
    public long Start { get; } = min;
    public long End { get; } = max;


    public bool IsInRange(long number) => Start <= number && number <= End;

    public static bool operator ==(Range range1, Range range2)
    {
        return range1.Start == range2.Start && range1.End == range2.End;
    }

    public static bool operator !=(Range range1, Range range2)
    {
        return !(range1 == range2);
    }
}