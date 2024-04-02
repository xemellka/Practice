using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Gift
{
    public List<Sweets> sweets;
    public Gift()
    {
        sweets = new List<Sweets>();
    }
    public void AddSweet(Sweets sweet)
    {
        sweets.Add(sweet);
    }
    public int TotalWeight()
    {
        return sweets.Sum(s => s.Weight);
    }
    public void SortSugar()
    {
        sweets.Sort((s1, s2) => s1.Sugar.CompareTo(s2.Sugar));
    }
    public List<Sweets> FindSugar(int minS, int maxS)
    {
        return sweets.Where(s => s.Sugar >= minS && s.Sugar <= maxS).ToList();
    }
    public List<Sweets> GetSweets()
    {
        return sweets;
    }
}
