class Program
{
    static void Main(string[] args)
    {
        string directoryPath = @"E:\DevLabs\Labs_2_2";
        string fileName = "sequence.json";

        if (File.Exists(Path.Combine(directoryPath, fileName)))
        {
            sequence seq = sequence.LoadFromJsonFile(Path.Combine(directoryPath, fileName));

            int[] numbers2 = { 1, 2, 4, 7, 11, 16, 22, 29, 37 };
            sequence seq2 = new sequence(numbers2);
            Console.WriteLine("increasing " + seq.Increase());
            Console.WriteLine("decreasing " + seq.Decrease());
            Console.WriteLine("nondecreasing " + seq.NonDecrease());
            Console.WriteLine("nonincreasing " + seq.NonIncrease());
            Console.WriteLine("arithmetic progression " + seq.ArithmeticProgression());
            Console.WriteLine("geometric progression " + seq.GeometricProgression());
            Console.WriteLine("equal: " + seq.Equal(seq2));
            Console.WriteLine("contain 4 " + seq.Contain(4));
            Console.WriteLine("max " + seq.Maximum());
            Console.WriteLine("min " + seq.Minimum());
            Console.WriteLine("subsequence simple " + string.Join(", ", seq.SimpleSub()));
            Console.WriteLine("min simple " + seq.MinSim());
            Console.WriteLine("max simple " + seq.MaxSim());
            Console.WriteLine("subsequence even " + string.Join(", ", seq.EvenSub()));
            Console.WriteLine("min even " + seq.MinEve());
            Console.WriteLine("max even " + seq.MaxEve());
            Console.WriteLine(seq.CompareSubsequences());
        }
        else
        {
            Console.WriteLine("Where is that json bro?");
        }
    }
}