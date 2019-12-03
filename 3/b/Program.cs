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
            var matchingcrossover = wires[0].VisitedLocations.Intersect(wires[1].VisitedLocations).OrderBy(l => wires[0].VisitedLocations.IndexOf(l) + wires[1].VisitedLocations.IndexOf(l)).Select(l => new { location = l, moves = wires[0].VisitedLocations.IndexOf(l) + wires[1].VisitedLocations.IndexOf(l) + 2 }).FirstOrDefault();

            Console.WriteLine($"cross over at {matchingcrossover.location} has min moves {matchingcrossover.moves}");
        }
    }
}
