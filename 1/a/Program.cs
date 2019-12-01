namespace Aoc
{
    using System;
    using System.IO;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var totalfuel = 0d;
            foreach (var line in File.ReadLines(args[0]))
            {
                totalfuel += Math.Floor((double)(int.Parse(line) / 3)) - 2;
            }

            Console.WriteLine(totalfuel);
        }
    }
}
