namespace Day5;

class Program
{
    static void Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");
        var database = Database.Create(path);
        
        Console.WriteLine($"5a) - Number of fresh ingredients: {database.TotalNumberOfIngredients - database.SpoiledIngredients.Count}");

        database.MergeRanges();

        var numberOfIngredientsCoveredByRanges = 0L;
        foreach (var range in database.Data)
        {
            numberOfIngredientsCoveredByRanges += (range.End - range.Start + 1);
        }
        
        Console.WriteLine($"5b) - Number of ingredients covered by ranges: {numberOfIngredientsCoveredByRanges}");
    }
}