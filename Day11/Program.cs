namespace Day11;

class Program
{
    static void Main(string[] args)
    {
        // var path = Path.GetFullPath(".\\TestData.txt");
        var path = Path.GetFullPath(".\\PuzzleData.txt");
        var lines = File.ReadAllLines(path);

        var reactor = Reactor.Create(lines);

        long results = reactor.CountAllPathsFrom("you", "out");

        long svrFft = reactor.CountAllPathsFrom("svr", "fft");
        long svrDac = reactor.CountAllPathsFrom("svr", "dac");
        long dacOut = reactor.CountAllPathsFrom("dac", "out");
        long fftOut = reactor.CountAllPathsFrom("fft", "out");
        long fftDac = reactor.CountAllPathsFrom("fft", "dac");
        long dacFft = reactor.CountAllPathsFrom("dac", "fft");

        Console.WriteLine($"11a) Number of paths from you to out: {results}");

        Console.WriteLine($"11b) Number of paths from svr to out [which passes through both fft and dac]: {(svrFft * fftDac * dacOut) + (svrDac * dacFft * fftOut)}");
    }
}