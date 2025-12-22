namespace Day5;

public class Database
{
    private Database()
    {
    }

    public List<Range> Data { get; private set; } = [];
    public HashSet<long> SpoiledIngredients { get; } = [];
    public HashSet<long> FreshIngredients { get; } = [];
    public long TotalNumberOfIngredients => SpoiledIngredients.Count + FreshIngredients.Count;

    public static Database Create(string path)
    {
        var data = new Database();

        using var stream = new StreamReader(path);
        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            if (line.IndexOf('-') > 0)
            {
                var range = line.Split('-');
                var rangeStart = long.Parse(range[0]);
                var rangeEnd = long.Parse(range[1]);
                data.Data.Add(new Range(rangeStart, rangeEnd));
                continue;
            }

            if (!long.TryParse(line, out var number))
                continue;
            foreach (var range in data.Data)
                if (range.IsInRange(number))
                {
                    data.FreshIngredients.Add(number);
                    break;
                }

            if (!data.FreshIngredients.Contains(number))
                data.SpoiledIngredients.Add(number);
        }

        return data;
    }

    private Range[] FindOverlappingRanges(Range rangeToCheck, params Range[] ranges)
    {
        var overlapping = new List<Range>();
        foreach (var range in ranges)
        {
            if (range.Start == rangeToCheck.Start && range.End == rangeToCheck.End)
                continue;

            if (range.IsInRange(rangeToCheck.Start) || range.IsInRange(rangeToCheck.End) || rangeToCheck.IsInRange(range.Start) || rangeToCheck.IsInRange(range.End))
            {
                overlapping.Add(range);
                var more = FindOverlappingRanges(range, ranges.Where(x => x != rangeToCheck && x != range).ToArray());
                if (more.Any())
                    overlapping.AddRange(more);
            }
        }

        return overlapping.ToArray();
    }

    public bool IsFresh(long number) => Data.Any(x => x.IsInRange(number));

    public void MergeRanges()
    {
        var merged = new List<Range>();
        var fixedList = Data.ToArray();

        foreach (var range in fixedList)
        {
            if (merged.Any(x => x.IsInRange(range.Start) || x.IsInRange(range.End)))
                continue;

            var overlapping = FindOverlappingRanges(range, fixedList);
            if (!overlapping.Any())
            {
                merged.Add(range);
                continue;
            }

            var overlappingStart = overlapping.Min(x => x.Start);
            var overlappingEnd = overlapping.Max(x => x.End);
            merged.Add(new Range(Math.Min(overlappingStart, range.Start), Math.Max(overlappingEnd, range.End)));
        }

        Data = merged;
    }
}