namespace Day1;

public class Vault
{
    public int Code
    {
        get;
        private protected set;
    }

    public int CurrentPosition
    {
        get;
        private protected set
        {
            field = value;
            if (value > 99)
                field = value - 100;
            if (value < 0)
                field = 100 + value;

            if (field == 0)
                Code++;
        }
    } = 50;

    public void Reset() =>  CurrentPosition = 50;

    public virtual void MoveRight(int right)
    {
        CurrentPosition += (right % 100);   
    }

    public virtual void MoveLeft(int left)
    {
        CurrentPosition -= (left % 100);
    }

    public void ParseFile(string file)
    {
        var path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(file));
        Console.WriteLine($"Parsing {path}");
        using var stream = File.OpenRead(path);
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
            Parse(reader.ReadLine());
    }
    
    public void Parse(string? line)
    {
        if (line == null)
            return;
        
        var entries = line.Split(',', StringSplitOptions.RemoveEmptyEntries |  StringSplitOptions.TrimEntries);
        foreach (var entry in entries)
        {
            if (entry[0] == 'L')
                MoveLeft(int.Parse(entry.Substring(1)));
            else if (entry[0] == 'R')
                MoveRight(int.Parse(entry.Substring(1)));
        }
    }
    
}

public class SecureVault : Vault
{
    public override void MoveRight(int right)
    {
        if (right < 100 && CurrentPosition + right > 100)
            Code++;
        if (right > 100)
        {
            var rotations = right / 100;
            Code += rotations;
            MoveRight(right % 100);
            return;
        }
        base.MoveRight(right);
    }

    public override void MoveLeft(int left)
    {
        if (CurrentPosition != 0 && left < 100 && CurrentPosition - left < 0)
            Code++;
        if (left > 100)
        {
            var rotations = left / 100;
            Code += rotations;
            MoveLeft(left % 100);
            return;
        }
        base.MoveLeft(left);
    }
}

