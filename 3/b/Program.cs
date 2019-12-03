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
            var crossovers = wires[0].VisitedLocationsLookup.Where(l => wires[1].VisitedLocationsLookup.Contains(l));
            var firstcrossover = wires[0].VisitedLocationsLookup.Where(l => wires[1].VisitedLocationsLookup.Contains(l)).OrderBy(l => wires[0].VisitedLocations.IndexOf(l) + wires[1].VisitedLocations.IndexOf(l) + 2).Select(l => new { location = l, moves = wires[0].VisitedLocations.IndexOf(l) + wires[1].VisitedLocations.IndexOf(l) + 2 }).FirstOrDefault();

            Console.WriteLine($"cross over at {firstcrossover.location} has min moves {firstcrossover.moves}");
        }
    }
}
