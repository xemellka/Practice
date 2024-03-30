using Newtonsoft.Json;
using System.ComponentModel;
class Program
{
    static void Main(string[] args)
    {
        string jsonPath = @"E:\DevLabs\Labs_2_4_22\gift.json";
        List<Sweets> sweets = LoadFromJson(jsonPath); 
        Gift gift = new Gift();
        foreach (Sweets sweet in sweets)
        {
            gift.AddSweet(sweet);
        }
        Console.WriteLine("Total weight: " +gift.TotalWeight()+ "gram");
        gift.SortSugar();
        Console.WriteLine("Gift sorted by sugar:");
        foreach (Sweets sweet in gift.GetSweets())
        {
            Console.WriteLine(sweet.Name + ": " + sweet.Sugar + "% of sugar");
        }
        int minS = 30;
        int maxS = 40;
        List<Sweets> findSweet = gift.FindSugar(minS, maxS);
        Console.WriteLine($"Sweets with sugar between {minS} and {maxS} %");
        foreach (Sweets sweet in findSweet)
        {
            Console.WriteLine(sweet.Name + ": " + sweet.Sugar + " % of sugar");
        }
    }
    static List<Sweets> LoadFromJson(string jsonPath)
    {
        string json = File.ReadAllText(jsonPath);
        return JsonConvert.DeserializeObject<List<Sweets>>(json);
    }
}