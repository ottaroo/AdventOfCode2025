using System.Text;

namespace Day6;

public class MathHomework
{
    public string[] Operators { get; private set; } = [];
    public double[] Calculations { get; private set; } = Array.Empty<double>();

    public void Parse(string path)
    {
        using var reader = new FileStream(path, FileMode.Open, FileAccess.Read);
        reader.Seek(-1, SeekOrigin.End);

        Operators = reader.ReadLineInReverse().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        reader.Seek(0, SeekOrigin.Begin);
        using var sr = new StreamReader(reader);
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (!long.TryParse(data[0], out var result))
                break;
            if (!Calculations.Any())
            {
                Calculations = data.Select(x => (double) long.Parse(x)).ToArray(); // initial values
                continue;
            }

            for (var i = 0; i < data.Length; i++)
            {
                switch (Operators[i])
                {
                    case "+":
                        Calculations[i] += long.Parse(data[i]);
                        break;
                    case "-":
                        Calculations[i] -= long.Parse(data[i]);
                        break;
                    case "*":
                        Calculations[i] *= long.Parse(data[i]);
                        break;
                    case "/":
                        Calculations[i] /= long.Parse(data[i]);
                        break;
                }
            }
        }
    }

    public void ParseVertical(string path)
    {
        using var reader = new FileStream(path, FileMode.Open, FileAccess.Read);
        var lines = File.ReadAllLines(path);
        Operators = lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        lines = lines[..^1];
        var map = lines.Select(x => x.ToCharArray()).ToArray();

        Calculations = Operators.Select(x => double.NaN).ToArray();

        var n = 0;
        for (var x = 0; x < lines.Max(l => l.Length); x++)
        {
            var sb = new StringBuilder();
            for (var y = 0; y < map.Length; y++)
            {
                if (map[y][x] != ' ')
                    sb.Append(map[y][x]);
            }

            if (sb.Length == 0)
            {
                n++;
                continue;
            }

            if (Double.IsNaN(Calculations[n]))
                Calculations[n] = long.Parse(sb.ToString());
            else
            {
                var result = long.Parse(sb.ToString());
                switch (Operators[n])
                {
                    case "+":
                        Calculations[n] += result;
                        break;
                    case "-":
                        Calculations[n] -= result;
                        break;
                    case "*":
                        Calculations[n] *= result;
                        break;
                    case "/":
                        Calculations[n] /= result;
                        break;
                }
            }
        }
    }
}

public static class MathExtensions
{
    extension(FileStream fileStream)
    {
        // messy - mostly to test new extensions
        public string ReadLineInReverse()
        {
            if (fileStream.Position == 0)
                return string.Empty;

            var buffer = new byte[4096];
            var lastKnownEndPosition = fileStream.Position;
            while (fileStream.Position > 0)
            {
                var readFrom = fileStream.Position - buffer.Length;
                if (readFrom < 0)
                    readFrom = 0;

                fileStream.Seek(readFrom, SeekOrigin.Begin);
                var bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    break;
                }

                var index = lastKnownEndPosition < buffer.Length ? buffer[..(int) lastKnownEndPosition].LastIndexOf((byte) '\n') : buffer.LastIndexOf((byte) '\n');
                if (index < bytesRead)
                {
                    var tmpBeginPosition = fileStream.Position - bytesRead + index;

                    byte[] tmp = Array.Empty<byte>();
                    if (lastKnownEndPosition - tmpBeginPosition > buffer.Length)
                    {
                        tmp = new byte[lastKnownEndPosition - tmpBeginPosition];
                    }
                    else
                    {
                        tmp = buffer;
                    }

                    fileStream.Seek(tmpBeginPosition, SeekOrigin.Begin);
                    var tmpRead = fileStream.Read(tmp, 0, (int) Math.Min(tmp.Length, lastKnownEndPosition - tmpBeginPosition));
                    var str = Encoding.UTF8.GetString(tmp, 0, tmpRead);
                    fileStream.Seek(tmpBeginPosition, SeekOrigin.Begin);
                    return str.Trim();
                }
            }

            return string.Empty;
        }
    }
}