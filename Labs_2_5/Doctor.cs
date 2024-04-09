using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Doctor
{
    public string name;
    public int experienceYears;
    public double salary;

    public Doctor(string name, int experienceYears, double salary)
    {
        this.name = name;
        this.experienceYears = experienceYears;
        this.salary = salary;
    }

    public abstract void DisplayExperience();
}

