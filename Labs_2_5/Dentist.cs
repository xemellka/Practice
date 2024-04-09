using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Dentist : Doctor
{
    public Dentist(string name, int experienceYears, double salary)
        : base(name, experienceYears, salary) { }
    public override void DisplayExperience()
    {
        Console.WriteLine($"Dentist {name} work experience: {experienceYears} years");
    }
    public virtual void CalculatePayment(double clientPayment)
    {
        Console.WriteLine($"{name} receives payment of {clientPayment * 0.9} from the client.");
    }
    public void RichDentist()
    {
        if (salary > 15000)
        {
            Console.WriteLine($"{name} has a large office");
        }
        else
        {
            Console.WriteLine($"{name} has a small office");
        }
    }
}

public class Orthodontist : Dentist
{
    public string braces { get; set; }
    public Orthodontist(string name, int experienceYears, double salary)
        : base(name, experienceYears, salary) { }
    public override void CalculatePayment(double clientPayment)
    {
        Console.WriteLine($"{name} receives payment of {clientPayment * 0.7} from the client.");
    }
}

public class DentalTechnician : Dentist
{
    public string implants { get; set; }
    public DentalTechnician(string name, int experienceYears, double salary)
        : base(name, experienceYears, salary) { }
    public override void CalculatePayment(double clientPayment)
    {
        Console.WriteLine($"{name} receives payment of {clientPayment * 1.2} from the client.");
    }
}
