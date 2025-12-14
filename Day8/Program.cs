namespace Day8;

class Program
{
    static void Main(string[] args)
    {
        var path = Path.GetFullPath(".\\TestData.txt");
        // var path = Path.GetFullPath(".\\PuzzleData.txt");

        //TODO:  disorganizer - get back to this later
        var junctionBoxOrganizer = new JunctionBoxOrganizer();
        
        using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            var data = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            junctionBoxOrganizer.AddJunctionBox(data[0], data[1], data[2]);
        }
        
        junctionBoxOrganizer.Connect();
        
    }
}