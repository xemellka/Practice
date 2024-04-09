using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Surgeon : Doctor
{
    public Surgeon(string name, int experienceYears, double salary)
        : base(name, experienceYears, salary) { }
    public override void DisplayExperience()
    {
        Console.WriteLine($"Surgeon {name} experience: {experienceYears} years");
    }
    public void PerformSurgery()
    {
        if (experienceYears > 1)
        {
            Console.WriteLine($"{name} is performing surgery.");
        }
        else
        {
            Console.WriteLine($"{name} is resting.");
        }
    }
}

public class Neurosurgeon : Surgeon
{
    public string Tools { get; set; }
    public Neurosurgeon(string name, int experienceYears, double salary)
        : base(name, experienceYears, salary) { }
}

public class PlasticSurgeon : Surgeon
{
    public string Robe { get; set; }
    public PlasticSurgeon(string name, int experienceYears, double salary)
        : base(name, experienceYears, salary) { }
}
