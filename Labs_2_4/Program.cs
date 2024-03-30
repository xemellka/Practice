using System;
using System.Collections.Generic;

public class Planet
{
    public string Name { get; set; }
    public List<Continent> Continents { get; set; }
    public List<Ocean> Oceans { get; set; }
    public List<Island> Islands { get; set; }

    public Planet(string name)
    {
        Name = name;
        Continents = new List<Continent>();
        Oceans = new List<Ocean>();
        Islands = new List<Island>();
    }

    public void AddContinent(Continent continent)
    {
        Continents.Add(continent);
    }

    public void AddOcean(Ocean ocean)
    {
        Oceans.Add(ocean);
    }

    public void AddIsland(Island island)
    {
        Islands.Add(island);
    }

    public void PrintPlanetInfo()
    {
        Console.WriteLine("Planet Info:");
        Console.WriteLine($"Planet: {Name}");
        Console.WriteLine($"Number of Continents: {Continents.Count}");
        Console.WriteLine("Continents:");
        foreach (var continent in Continents)
        {
            Console.WriteLine($"- {continent.Name}");
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Planet planet) return Name == planet.Name;
        return false;
    }
    public override int GetHashCode() => Name.GetHashCode();
    public override string ToString() => $"Planet: {Name}";
}

public class Continent
{
    public string Name { get; set; }

    public Continent(string name)
    {
        Name = name;
    }

    public override bool Equals(object obj)
    {
        if (obj is Continent continent) return Name == continent.Name;
        return false;
    }
    public override int GetHashCode() => Name.GetHashCode();
    public override string ToString() => $"Continent: {Name}";
}

public class Ocean
{
    public string Name { get; set; }

    public Ocean(string name)
    {
        Name = name;
    }

    public override bool Equals(object obj)
    {
        if (obj is Ocean ocean) return Name == ocean.Name;
        return false;
    }
    public override int GetHashCode() => Name.GetHashCode();
    public override string ToString() => $"Ocean: {Name}";
}

public class Island
{
    public string Name { get; set; }

    public Island(string name)
    {
        Name = name;
    }

    public override bool Equals(object obj)
    {
        if (obj is Island island) return Name == island.Name;
        return false;
    }
    public override int GetHashCode() => Name.GetHashCode();
    public override string ToString() => $"Island: {Name}";
}

class Program
{
    static void Main(string[] args)
    {
        Planet earth = new Planet("Earth");
        Planet dota2 = new Planet("Dota2");

        earth.AddContinent(new Continent("Europe"));
        earth.AddContinent(new Continent("Asia"));
        earth.AddContinent(new Continent("North America"));
        earth.AddOcean(new Ocean("Atlantic Ocean"));
        earth.AddOcean(new Ocean("Pacific Ocean"));
        earth.AddIsland(new Island("Hawai"));
        earth.AddIsland(new Island("Madagascar"));
        Continent con1 = new Continent("Europe");
        Ocean oc1 = new Ocean("Atlantic Ocean");
        Ocean oc2 = new Ocean("Pacific Ocean");
        Island is1 = new Island("Hawai");
        earth.PrintPlanetInfo();

        dota2.AddContinent(new Continent("Midle"));
        dota2.AddContinent(new Continent("Right"));
        dota2.AddContinent(new Continent("Left"));
        dota2.AddContinent(new Continent("Forest"));
        dota2.AddOcean(new Ocean("Mid Ocean"));
        dota2.AddIsland(new Island("Store"));
        dota2.AddIsland(new Island("Pudge"));
        dota2.PrintPlanetInfo();

        Console.WriteLine(earth.Equals(new Planet("Earth")));
        Console.WriteLine(earth.Equals(new Planet("Mars")));
        Console.WriteLine($"Equels is1 to is1: {is1.Equals(is1)}");
        Console.WriteLine($"Equels con1 to con1: {con1.Equals(con1)}");
        Console.WriteLine($"Equels oc1 to oc2: {oc1.Equals(oc2)}");
        Console.WriteLine(oc1.GetHashCode());
        Console.WriteLine(is1.GetHashCode());
        Console.WriteLine(con1.GetHashCode());
        Console.WriteLine(earth.GetHashCode());
        Console.WriteLine(earth.ToString());
        Console.WriteLine(con1.ToString());
        Console.WriteLine(oc1.ToString());
        Console.WriteLine(is1.ToString());
    }
}
