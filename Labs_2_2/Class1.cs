using System;
using Newtonsoft.Json;

public class sequence
{
    public int[] seqnum { get; set; }

    public sequence(int[] numbers)
    {
        seqnum = numbers;
    }

    public bool Increase()
    {
        for (int i = 0; i < seqnum.Length - 1; i++)
        {
            if (seqnum[i] >= seqnum[i + 1])
                return false;
        }
        return true;
    }

    public bool Decrease()
    {
        for (int i = 0; i < seqnum.Length - 1; i++)
        {
            if (seqnum[i] <= seqnum[i + 1])
                return false;
        }
        return true;
    }

    public bool NonDecrease()
    {
        for (int i = 0; i < seqnum.Length - 1; i++)
        {
            if (seqnum[i] > seqnum[i + 1])
                return false;
        }
        return true;
    }

    public bool NonIncrease()
    {
        for (int i = 0; i < seqnum.Length - 1; i++)
        {
            if (seqnum[i] < seqnum[i + 1])
                return false;
        }
        return true;
    }

    public bool ArithmeticProgression()
    {
        int diff = seqnum[1] - seqnum[0];
        for (int i = 1; i < seqnum.Length - 1; i++)
        {
            if (seqnum[i + 1] - seqnum[i] != diff)
                return false;
        }
        return true;
    }

    public bool GeometricProgression()
    {
        int ratio = seqnum[1] / seqnum[0];
        for (int i = 1; i < seqnum.Length - 1; i++)
        {
            if (seqnum[i + 1] / seqnum[i] != ratio)
                return false;
        }
        return true;
    }

    public bool Contain(int num)
    {
        return seqnum.Contains(num);
    }

    public bool Equal(sequence numbers2)
    {
        return seqnum.SequenceEqual(numbers2.seqnum);
    }

    public int Maximum()
    {
        return seqnum.Max();
    }

    public int Minimum()
    {
        return seqnum.Min();
    }

    public int[] SimpleSub()
    {
        List<int> simple = new List<int>();

        foreach (int sim in seqnum)
        {
            if (FindSim(sim))
            {
                simple.Add(sim);
            }
        }
        return simple.ToArray();
    }

    private bool FindSim(int sim)
    {
        if (sim <= 1)
        {
            return false;
        }

        for (int i = 2; i <= Math.Sqrt(sim); i++)
        {
            if (sim % i == 0)
            {
                return false;
            }
        }
        return true;
    }
    public int MinSim()
    {
        int minPrime = int.MaxValue;
        int[] simpleArray = SimpleSub();

        foreach (int num in simpleArray)
        {
            if (num < minPrime)
            {
                minPrime = num;
            }
        }
        return minPrime;
    }

    public int MaxSim()
    {
        int maxPrime = int.MinValue;
        int[] simpleArray = SimpleSub();

        foreach (int num in simpleArray)
        {
            if (num > maxPrime)
            {
                maxPrime = num;
            }
        }
        return maxPrime;
    }

    public int[] EvenSub()
    {
        List<int> evenNumbers = new List<int>();

        foreach (int num in seqnum)
        {
            if (num % 2 == 0)
            {
                evenNumbers.Add(num);
            }
        }
        return evenNumbers.ToArray();
    }

    public int MinEve()
    {
        int[] evenNumbers = EvenSub();

        int minEven = evenNumbers.Min();
        return minEven;
    }

    public int MaxEve()
    {
        int[] evenNumbers = EvenSub();

        int maxEven = evenNumbers.Max();
        return maxEven;
    }

    public string CompareSubsequences()
    {
        int[] EvenSubibubi = EvenSub();
        int[] SimpleSubibubi = SimpleSub();

        int evenCount = EvenSubibubi.Length;
        int simpleCount = SimpleSubibubi.Length;

        if (evenCount > simpleCount)
            return "more num in even";
        else if (evenCount < simpleCount)
            return "more num in simple";
        else
            return "equal even and simple";
    }

    public void SaveToJsonFile(string directoryPath, string fileName)
    {
        string filePath = Path.Combine(directoryPath, fileName);
        string jsonData = JsonConvert.SerializeObject(this);
        File.WriteAllText(filePath, jsonData);
        Console.WriteLine("object saved in JSON " + filePath);
    }

    public static sequence LoadFromJsonFile(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        sequence loadedSequence = JsonConvert.DeserializeObject<sequence>(jsonData);
        Console.WriteLine("object loaded from JSON " + filePath);
        return loadedSequence;
    }
}