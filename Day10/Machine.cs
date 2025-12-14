using System.Globalization;
using System.Text.RegularExpressions;

namespace Day10;

public class Machine
{
    public char[] Lights { get; set; }

    public void ToggleLights(params int[] lights)
    {
        foreach (var toggle in lights)
        {
            Lights[toggle] = Lights[toggle] == '.' ? '#' : '.';
        }
    }

    public static Machine Create(string line)
    {
        var machine = new Machine();
        var matches = MachineRegex.ParseLine().Matches(line);
        machine.Lights = matches[0].Groups["lights"].Value.ToCharArray();
        var buttons = matches[0].Groups["button"].Value;
        var array = buttons.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var str in array)
        {
            var nums = str.Trim('(', ')').Split(',').Select(int.Parse).ToArray();
            machine.AddButton(nums);
        }

        var jolts =  matches[0].Groups["jolt"].Value;
        var joltNums = jolts.Trim('{', '}').Split(',', StringSplitOptions.RemoveEmptyEntries|StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
        machine.AddJolts(joltNums);
        return machine;
    }

    private void AddJolts(int[] nums)
    {
        _jolts = nums;
    }

    private List<int[]> _buttons = [];
    private int[] _jolts;

    private void AddButton(int[] nums)
    {
        _buttons.Add(nums);
    }
}

public partial class MachineRegex
{
    [GeneratedRegex(@"\[(?<lights>[^\]]+)\]\s*(?<button>.*)(?<jolt>\{[^\}]+\})")]
    public static partial Regex ParseLine();
}