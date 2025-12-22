using System.Numerics;
using System.Text;
using Google.OrTools.LinearSolver;

namespace Day10;

public class Machine
{
    public int[] Jolts = [];
    public static int MachinesCreated { get; private set; }
    public string Id { get; private set; }

    public char[] Lights { get; set; }

    public int LightPattern { get; private set; } = 0;

    public List<Button> Buttons { get; } = [];

    private void AddButton(int[] nums)
    {
        var button = Button.Create(Buttons.Count, nums);
        Buttons.Add(button);
    }

    private void AddJolts(int[] nums)
    {
        Jolts = nums;
    }

    private string BitsToString(int bits)
    {
        var sb = new StringBuilder();
        sb.Append("(");
        var current = bits;
        for (var n = 0; n < 64; n++)
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

    public static Machine Create(string line)
    {
        var machine = new Machine();
        var matches = MachineRegex.ParseLine().Matches(line);
        machine.Lights = matches[0].Groups["lights"].Value.ToCharArray();
        machine.Id = $"{MachinesCreated++}: " + matches[0].Groups["lights"].Value;

        var pattern = 0;
        for (var n = 0; n < machine.Lights.Length; n++)
            if (machine.Lights[n] == '#')
                pattern |= (1 << n);

        machine.LightPattern = pattern;


        var jolts = matches[0].Groups["jolt"].Value;
        var joltNums = jolts.Trim('{', '}').Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
        machine.AddJolts(joltNums);

        var buttons = matches[0].Groups["button"].Value;
        var array = buttons.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var str in array)
        {
            var nums = str.Trim('(', ')').Split(',').Select(int.Parse).ToArray();
            machine.AddButton(nums);
        }

        return machine;
    }

    
    public int FindTheLeastAmountOfButtonsToPush()
    {
        var solver = Solver.CreateSolver("SCIP");
        if (solver == null)
            throw new Exception("Solver could not be created");
        var x = new Variable[Buttons.Count];
        for (var b = 0; b < Buttons.Count; b++)
        {
            var upperBound = Buttons[b].BitsSet.Select(pos => Jolts[pos]).Prepend(0).Max();
            x[b] = solver.MakeIntVar(0, upperBound, $"x_{b}");
        }

        for (var c = 0; c < Jolts.Length; c++)
        {
            var constraint = solver.MakeConstraint(Jolts[c], Jolts[c], $"target_{c}");
            for (var b = 0; b < Buttons.Count; b++)
            {
                // 1 if current button increase joltage C otherwise 0
                constraint.SetCoefficient(x[b], Buttons[b].BitsSet.Contains(c) ? 1 : 0);
            }
        }
        var minimumButtonPushes = solver.Objective();
        for (var b = 0; b < Buttons.Count; b++)
        {
            minimumButtonPushes.SetCoefficient(x[b], 1.0); // all buttons increase only 1 at a time
        } 
        minimumButtonPushes.SetMinimization(); 
        var resultStatus = solver.Solve(); 
        if (resultStatus != Solver.ResultStatus.OPTIMAL && resultStatus != Solver.ResultStatus.FEASIBLE) 
        {
            return -1; // failed
        } 
        int totalPresses = 0;
        var verify = new int[Jolts.Length];
        for (var b = 0; b < Buttons.Count; b++)
        {

            for (var n = 0; n < (int) x[b].SolutionValue(); n++)
            {
                Buttons[b].IncrementJolts(verify);
            }
            
            totalPresses += (int)x[b].SolutionValue();
        } 
        // verify we actully found the solution
        if (!IsMatch(Jolts, verify))
            Console.WriteLine($"Failed on machine: {Id}");
        return totalPresses;
    }

    private static bool IsMatch(int[] toMatch, int[] currentJolts)
    {
        for (var n = 0; n < currentJolts.Length; n++)
        {
            if (currentJolts[n] != toMatch[n])
                return false;
        }

        return true;
    }

    public int PushButtons()
    {
        var buttonsPressedWithSolution = new HashSet<int>();

        var options = (int) Math.Pow(2, Buttons.Count) - 1;
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

        // var first = buttonsPressedWithSolution.OrderBy(x => BitOperations.PopCount((uint) x)).First();
        // Console.WriteLine($"Solution to {new string(Lights)}: {ToButtonsDescription(first)}");

        return buttonsPressedWithSolution.Min(x => BitOperations.PopCount((uint) x));
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
}