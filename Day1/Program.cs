namespace Day1;

class Program
{
    static void Main(string[] args)
    {
        var path = Path.GetFullPath(".\\PuzzleData.txt");
        var vault = new Vault();
        vault.ParseFile(path);
        Console.WriteLine("Code for 1a is: " + vault.Code);
        
        vault = new SecureVault();
        vault.ParseFile(path);
        Console.WriteLine("Code for 1b is: " + vault.Code);
    }
}