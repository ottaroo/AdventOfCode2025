namespace Day3;

public class Batteries
{
    public async Task<long> GetJoltage(string path)
    {
        long joltage = 0;
        await using var reader = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var sr = new StreamReader(reader);
        var lineIndex = 1;
        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync();
            var j = GetJoltageFromLine(line);
            joltage += j;
            lineIndex++;
        }

        return joltage;
    }

    private protected virtual long GetJoltageFromLine(ReadOnlySpan<char> lineChars)
    {
        var length = lineChars.Length;
        var high = 0;
        var highIndex = 0;
        for (var n = length - 2; n >= 0; n--)
            if (lineChars[n] - '0' >= high)
            {
                high = lineChars[n] - '0';
                highIndex = n;
            }

        var low = 0;
        for (var n = highIndex + 1; n < lineChars.Length; n++)
            if (lineChars[n] - '0' > low)
            {
                low = lineChars[n] - '0';
            }

        return long.Parse($"{high}{low}");
    }
}