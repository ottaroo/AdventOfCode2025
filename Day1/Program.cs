namespace Day1;

class Program
{
    static void Main(string[] args)
    {
        var vault = new Vault();
        vault.ParseFile(args[0]);
        Console.WriteLine("Code for 1a is: " + vault.Code);
        
        vault = new SecureVault();
        vault.ParseFile(args[0]);
        Console.WriteLine("Code for 1b is: " + vault.Code);
    }
}