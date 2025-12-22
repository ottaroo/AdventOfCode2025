using System.Reflection.Metadata;

namespace Day11;

public class Reactor
{
    private readonly Dictionary<string, long> _cache = [];

    private Reactor()
    {
    }

    public Dictionary<string, string[]> Devices { get; } = [];


    public long CountAllPathsFrom(string from, string to)
    {
        _cache.Clear();
        return CountAllPathsFromHelper(from, to);
    }

    private long CountAllPathsFromHelper(string from, string to)
    {
        if (_cache.TryGetValue($"{from}:{to}", out var cache))
            return cache;

        var numberOfPaths = 0L;
        var queue = new Queue<string>(Devices[from]);
        var visited = new HashSet<string>();
        var visitedCount = new Dictionary<string, long>();
        while (queue.TryDequeue(out var device))
        {
            if (!to.Equals("out", StringComparison.InvariantCultureIgnoreCase) && device.Equals("out", StringComparison.InvariantCultureIgnoreCase))
                continue;

            var devices = Devices[device];
            // if (devices.Length == 1 && devices[0].Equals(to, StringComparison.InvariantCultureIgnoreCase))
            if (devices.Contains(to))
            {
                numberOfPaths++;
                continue;
            }

            if (!visited.Add(device))
            {
                if (visitedCount.TryGetValue(device, out _))
                    visitedCount[device]++;
                else
                    visitedCount.Add(device, 1);
                continue;
            }

            foreach (var name in devices)
                queue.Enqueue(name);
        }

        foreach (var dev in visited)
        {
            if (visitedCount.TryGetValue(dev, out var count))
            {
                if (count > 0)
                {
                    numberOfPaths += (CountAllPathsFromHelper(dev, to) * count);
                }
            }
        }

        _cache.Add($"{from}:{to}", numberOfPaths);

        return numberOfPaths;
    }

    public static Reactor Create(string[] data)
    {
        var reactor = new Reactor();
        foreach (var line in data)
        {
            var deviceName = line.Substring(0, line.IndexOf(':'));
            var outputs = line.Substring(line.IndexOf(':') + 1).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            reactor.Devices.Add(deviceName, outputs);
        }

        return reactor;
    }
}