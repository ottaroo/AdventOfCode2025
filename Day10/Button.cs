namespace Day10;

public class Button
{
    public int Id { get; init; } 
    public required int Bits { get; init; }
    public required int[] BitsSet { get; init; }

    public static Button Create(int id, int[] nums)
    {
        var pattern = 0;
        

        foreach (var n in nums)
        {
            pattern = pattern | (1 << n);
        }
        return new Button
        {
            Id = id,
            Bits = pattern,
            BitsSet = nums
        };
    }


    public void IncrementJolts(ref int[] currentJolts, int incrementValue = 1)
    {
        foreach (var t in BitsSet)
            currentJolts[t] += incrementValue;
    }
}