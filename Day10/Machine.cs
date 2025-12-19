using System.Collections;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Day10;

public class Machine
{
    public char[] Lights { get; set; }

    public BitArray LightPattern { get; private set; } = new([false]);

    public void ConvertLightsToBitPattern()
    {
        LightPattern = new BitArray(Lights.Select(x => x.Equals('#')).ToArray());
    }

    public void Clear()
    {
        Lights = Lights.Select(x=>'.').ToArray();
    }
    
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
        machine.ConvertLightsToBitPattern();
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

    private Dictionary<int, BitArray> _buttons = [];
    private int[] _jolts;

    private void AddButton(int[] nums)
    {
        var max = nums.Max();
        var bits = new BitArray(max);
        foreach (var num in nums)
            bits[num] = true;
        _buttons.Add(_buttons.Count + 1, bits);
    }

    public int[] PushButtons()
    
    {
        var start = new BitArray(LightPattern);
        start.SetAll(false);
        
        
        
        
        
        return [];
    }
    
}

public partial class MachineRegex
{
    [GeneratedRegex(@"\[(?<lights>[^\]]+)\]\s*(?<button>.*)(?<jolt>\{[^\}]+\})")]
    public static partial Regex ParseLine();
}