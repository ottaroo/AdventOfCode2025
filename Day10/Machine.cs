using System.Globalization;
using System.Numerics;
using System.Text;

namespace Day10;

public class Machine
{
    public char[] Lights { get; set; }

    public int LightPattern { get; private set; } = 0;

    public List<Button> Buttons { get; } = [];

    public static Machine Create(string line)
    {
        var machine = new Machine();
        var matches = MachineRegex.ParseLine().Matches(line);
        machine.Lights = matches[0].Groups["lights"].Value.ToCharArray();

        var pattern = 0;
        for(var n = 0; n <  machine.Lights.Length; n++)
            if (machine.Lights[n] == '#')
                pattern |= (1 << n);

        machine.LightPattern = pattern;

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
        Jolts = nums;
    }

    public int[] Jolts = [];

    private void AddButton(int[] nums)
    {
        Buttons.Add(Button.Create(Buttons.Count, nums));
        
    }

    private string BitsToString(int bits)
    {
        var sb = new StringBuilder();
        sb.Append("(");
        var current = bits;
        for(var n=0; n<64; n++)
        {
            if (current <= 0)
                break; 
            
            if ((current & 0x1) == 0x1)
                sb.Append($"{n},");
            current >>= 1;
        }
        var str = sb.ToString().TrimEnd(',');
        return str + ")";
    }
    private string ToButtonsDescription(int buttons)
    {
        var sb = new StringBuilder();
        for (var n = 0; n < Buttons.Count; n++)
        {
            if ((buttons & (1 << n)) == (1 << n))
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append($"Button {Buttons[n].Id} {BitsToString(Buttons[n].Bits)}");
            }
        }
        return sb.ToString();
    }
    
    public int PushButtons()
    {
        var buttonsPressedWithSolution = new HashSet<int>();

        var options = (int)Math.Pow(2, Buttons.Count) - 1;
        for (var o = 1; o < options; o++)
        {
            var evaluator = o;
            long test = 0;
            var pushButton = 0;
            for (var n = 0; n < Buttons.Count; n++)
            {
                if (evaluator == 0)
                    break;
                
                if ((evaluator & 0x1) == 0x1)
                {
                    test ^= Buttons[n].Bits;
                    pushButton |= (1 << n);
                    if ((test ^ LightPattern) == 0)
                    {
                        _ = buttonsPressedWithSolution.Add(pushButton);
                        break;
                    }
                }
                evaluator >>= 1;
            }
        }        

        var first = buttonsPressedWithSolution.OrderBy(x=>BitOperations.PopCount((uint)x)).First();
        Console.WriteLine($"Solution to {new string(Lights)}: {ToButtonsDescription(first)}");
        
        return buttonsPressedWithSolution.Min(x=> BitOperations.PopCount((uint)x));
    }

    private static bool IsValid(ref int[] toMatch, ref int[] currentJolts)
    {
        for (var n = 0; n < currentJolts.Length; n++)
        {
            if (currentJolts[n] > toMatch[n])
                return false;
        }

        return true;
    }
    private static bool IsMatch(ref int[] toMatch, ref int[] currentJolts)
    {
        for (var n = 0; n < currentJolts.Length; n++)
        {
            if (currentJolts[n] != toMatch[n])
                return false;
        }

        return true;
    }
    
    //TODO: make something smarter - works on example, not on actual puzzledata (to many combinations)
    public int FindBestJoltageCombinations(out string buttonsPushed)
    {
        buttonsPushed = string.Empty;
        var currentJolts = new int[Jolts.Length];
    
        var maxNumberOfPressesForOneButton = Jolts.Max() + 1;
        var numberOfButtons = Buttons.Count;
        var numberOfPossibleCombinations = (long)Math.Pow(maxNumberOfPressesForOneButton, numberOfButtons);
    
        var buttonCombo = new int[numberOfButtons];
        var numberOfButtonPushes = int.MaxValue;
        
        var div = new long[buttonCombo.Length];
        for (var n = 0; n < div.Length; n++)
        {
            div[n] = (long)Math.Pow(maxNumberOfPressesForOneButton, n);
        }
        for (var n = 0; n < numberOfPossibleCombinations; n++)
        {
            Array.Clear(currentJolts, 0, currentJolts.Length);
            Array.Clear(buttonCombo, 0, buttonCombo.Length);
            for (var i = 0; i < numberOfButtons; i++)
            {
                buttonCombo[i] = (int)((n / div[i]) % maxNumberOfPressesForOneButton);
                if (buttonCombo[i] > 0)
                    Buttons[i].IncrementJolts(ref currentJolts, buttonCombo[i]);
    
                if (!IsValid(ref Jolts, ref currentJolts))
                    break;
            }
    
            if (!IsMatch(ref Jolts, ref currentJolts))
                continue;
            var pushes = 0;
            foreach (var p in buttonCombo)
                pushes += p;
            if (pushes >= numberOfButtonPushes)
                continue;
    
            var str = new StringBuilder();
            for (var b = 0; b < buttonCombo.Length; ++b)
            {
                if (str.Length > 0)
                    str.Append(" | ");
                
                if (buttonCombo[b] > 0)
                    str.Append($"{buttonCombo[b]}x({(string.Join(",", Buttons[b].BitsSet))})");
            }
            
            buttonsPushed = str.ToString();
            numberOfButtonPushes = pushes;
        }
    
        return numberOfButtonPushes;
    
    }
}