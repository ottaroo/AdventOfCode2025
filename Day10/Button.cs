namespace Day10;

public class Button : IEquatable<Button>
{
    public int Id { get; init; }
    public required int Bits { get; init; }
    public required int[] BitsSet { get; init; }
    public int MaxNumberOfPushes { get; set; }
    public int Weight { get; set; }

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
    public bool Equals(Button? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Bits == other.Bits;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Button) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Bits);
    }


    public void IncrementJolts(int[] currentJolts, int incrementValue = 1)
    {
        foreach (var t in BitsSet)
            currentJolts[t] += incrementValue;
    }
}