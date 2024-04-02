using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Sweets
{
    public string Name { get; set; }
    public int Weight { get; set; }
    public int Sugar { get; set; }
}
class Candy : Sweets
{
    public string Color { get; set; }
}
class Chocolate : Sweets
{
    public int Cacao { get; set; }
}
class Cookie : Sweets
{
    public int Flour { get; set; }
}
