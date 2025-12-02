namespace Day2;

class Program
{
    static void Main(string[] args)
    {
        var path = Path.GetFullPath(".\\PuzzleData.txt");
        using var reader = File.OpenText(path);
        var line = reader.ReadLine();
        if (line == null)
        {
            Console.WriteLine("Nothing found");
            return;
        }
        
        var catalog = ProductCatalog.Parse(line);
        var sum = catalog.GetSumOfInvalidProductRanges2A();
        Console.WriteLine($"2a) Sum of valid products: {sum}");
        var sum2 = catalog.GetSumOfInvalidProductRanges2B();
        Console.WriteLine($"2b) Sum of valid products: {sum2}");
    }
}