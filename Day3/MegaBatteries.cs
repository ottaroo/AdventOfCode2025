using System.Text;

namespace Day3;

public class MegaBatteries : Batteries
{
    private int GetHighNumber(ReadOnlySpan<char> lineChars, out int index)
    {
        var high = 0;
        var highIndex = 0;
        var n = lineChars.Length - 1;
        while (n >= 0)
        {
            if (lineChars[n] - '0' >= high)
            {
                high = lineChars[n] - '0';
                highIndex = n;
            }

            n--;
        }

        index = highIndex + 1;
        return high;
    }

    private protected override long GetJoltageFromLine(ReadOnlySpan<char> lineChars)
    {
        var index = 0;
        var batteries = new StringBuilder();
        for (var n = 0; n < 11; n++)
        {
            var num = GetHighNumber(lineChars[index..^(11 - n)], out var partIndex);
            batteries.Append(num.ToString());
            index += partIndex;
        }

        var lastDigit = GetHighNumber(lineChars[index..], out _);
        batteries.Append(lastDigit.ToString());

        return long.Parse(batteries.ToString());
    }
}