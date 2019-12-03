namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Wire> wires = new List<Wire>();

            foreach (var line in File.ReadLines(args[0]))
            {
                var wire = new Wire(line);
                wires.Add(wire);
            }

            // now find first join visited location
            var firstcrossover = wires[0].VisitedLocations.Where(l => wires[1].VisitedLocations.Contains(l)).OrderBy(l => Wire.CalculateDistance(l)).FirstOrDefault();

            Console.WriteLine($"cross over at {firstcrossover} has min distance {Wire.CalculateDistance(firstcrossover)}");
        }
    }
}
