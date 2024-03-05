class Program
{
    static void Main(string[] args )
    {
        List<int> numbers = new List<int>();
        for (int i = 10; i <= 100; i++)
        {
            numbers.Add(i);
        }
        PrintSpiral(numbers, 10);
    }

    static void PrintSpiral(List<int> numbers, int einrow)
    {
        int count = 0;
        for (int i = 0; i < numbers.Count; i += einrow)
        {
            List<int> row = numbers.GetRange(i, Math.Min(einrow, numbers.Count - i));
            if (count % 2 != 0)
            {
                row.Reverse();
            }
            Console.WriteLine(string.Join(" ", row));
            count++;
        }
    }
}




//using Newtonsoft.Json;

//class Program
//{
//    static void Main()
//    {
//        Dictionary<string, char[]> dict = new Dictionary<string, char[]> {
//            {"1", new[] {'A', 'd'}},
//            {"2", new[] {'C', 'B'}}
//        };
//        List<string> comb = GenerateComb(dict);
//        string result = string.Join(" ", comb);
//        Console.WriteLine(result);

//        string jsonResult = JsonConvert.SerializeObject(comb);
//        string filePath = @"E:\DevLabs\Labs_2_1\result.json";
//        File.WriteAllText(filePath, jsonResult);
//    }

//    static List<string> GenerateComb(Dictionary<string, char[]> dict)
//    {
//        List<string> comb = new List<string>();
//        foreach (var ch1 in dict["1"])
//        {
//            foreach (var ch2 in dict["2"])
//            {
//                comb.Add(ch1.ToString() + ch2.ToString());
//            }
//        }
//        return comb;
//    }
//}




//class Program
//{
//    static void Main()
//    {
//        string[] A = { "kizzapeperoni", "papa", "mama" };
//        var result = A.Reverse().SelectMany(s => s.Take(1));
//        Console.WriteLine(string.Join("", result));
//    }
//}
