using System.Runtime.Versioning;
using System.Text;

namespace Day2;

public record ProductRange(string Left, string Right);

public class ProductCatalog
{
    public ProductRange[] Ranges { get; private set; } = [];

    private (string Left, string Right) GetLeftAndRight(string line) => (line.Substring(0, line.Length / 2), line.Substring(line.Length / 2));


    public long GetSumOfInvalidProductRanges2A()
    {
        var sum = 0L;

        // lets just brute force it
        foreach (var range in Ranges)
        {
            var maxValue = range.Right.Length % 2 == 0 ? long.Parse(range.Right) : long.Parse("9".PadRight(range.Right.Length - 1, '9'));
            var minValue = range.Left.Length % 2 == 0 ? long.Parse(range.Left) : long.Parse("1".PadRight(range.Left.Length + 1, '0'));
            var patternMinString = GetLeftAndRight($"{minValue}");
            var patternMaxString = GetLeftAndRight($"{maxValue}");
            var patternMin = long.Parse(patternMinString.Left);
            var patternMax = long.Parse(patternMaxString.Left);

            for (var n = patternMin; n <= patternMax; n++)
            {
                var pattern = long.Parse($"{n}{n}");
                if (pattern >= minValue && pattern <= maxValue)
                    sum += pattern;
            }
        }

        return sum;
    }

    public long GetSumOfInvalidProductRanges2B()
    {
        var sum = 0L;

        // lets just brute force it
        foreach (var range in Ranges)
        {
            var maxValue = range.Right.Length % 2 == 0 ? long.Parse(range.Right) : long.Parse("9".PadRight(range.Right.Length - 1, '9'));
            var minValue = range.Left.Length % 2 == 0 ? long.Parse(range.Left) : long.Parse("1".PadRight(range.Left.Length + 1, '0'));
            var patternMinString = GetLeftAndRight($"{minValue}");
            var patternMaxString = GetLeftAndRight($"{maxValue}");
            var patternMin = long.Parse(patternMinString.Left);
            var patternMax = long.Parse(patternMaxString.Left);

            var added = new HashSet<long>();

            for (var n = patternMin; n <= patternMax; n++)
            {
                var pattern = long.Parse($"{n}{n}");
                if (pattern >= minValue && pattern <= maxValue)
                {
                    sum += pattern;
                    added.Add(pattern);
                }
            }


            // upping the game... all values not divisible by 2 could be identical numbers and be invalid
            // still brut forcing a bit
            maxValue = long.Parse(range.Right);
            minValue = long.Parse(range.Left);
            if (minValue < 10)
                minValue = 10; // the premiss is repeating number - single digits are not reaping
            for (var l = range.Left.Length; l <= range.Right.Length; l++)
            {
                if (l % 2 != 0)
                {
                    for (var i = 1; i <= 9; i++)
                    {
                        var pattern = "".PadRight(l, (char) ('0' + i));
                        var value = long.Parse(pattern);
                        if (value >= minValue && value <= maxValue && !added.Contains(value))
                        {
                            sum += value;
                            added.Add(value);
                        }
                    }
                }

                var groupings = (int) (l / 2);
                if (groupings >= 2)
                {
                    for (var g = 2; g <= groupings; g++)
                    {
                        var groupMax = "9".PadRight(g, '9');
                        for (var x = 10; x < int.Parse(groupMax); x++)
                        {
                            var sb = new StringBuilder();
                            while (sb.Length < l)
                            {
                                sb.Append($"{x}");
                            }

                            var value = long.Parse(sb.ToString());
                            if (value >= minValue && value <= maxValue && !added.Contains(value))
                            {
                                sum += value;
                                added.Add(value);
                            }
                        }
                    }
                }
            }
        }

        return sum;
    }

    public static ProductCatalog Parse(string line)
    {
        var lines = line.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x =>
        {
            var parts = x.Split("-", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new ProductRange(parts[0], parts[1]);
        }).ToArray();
        return new ProductCatalog()
        {
            Ranges = lines
        };
    }
}