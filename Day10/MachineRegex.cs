using System.Text.RegularExpressions;

namespace Day10;

public abstract partial class MachineRegex
{
    [GeneratedRegex(@"\[(?<lights>[^\]]+)\]\s*(?<button>.*)(?<jolt>\{[^\}]+\})")]
    public static partial Regex ParseLine();
}