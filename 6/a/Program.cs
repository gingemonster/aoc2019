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
            var totalorbits = 0d;

            // store each object and a count of the things it orbits
            var relationships = new List<Tuple<string, string>>();
            var orbits = new Dictionary<string, int>();

            orbits.Add("COM", 0);

            foreach (var line in File.ReadLines(args[0]))
            {
                var splits = line.Split(')');

                // store orbiting object as key, object being orbited as
                relationships.Add(new Tuple<string, string>(splits[0], splits[1]));
                orbits.Add(splits[1], 1);
            }

            CalculateOrbits("COM", relationships, orbits);

            totalorbits = orbits.Sum(o => o.Value);

            Console.WriteLine(totalorbits);
        }

        private static void CalculateOrbits(string currentobject, List<Tuple<string, string>> relationships, Dictionary<string, int> orbits)
        {
            // find anything that has currentobject as things its orbiting and add
            // current objects orbit count to its orbit count
            relationships.Where(r => r.Item1 == currentobject).ToList().ForEach(r =>
            {
                orbits[r.Item2] += orbits[currentobject];
                CalculateOrbits(r.Item2, relationships, orbits);
            });
        }
    }
}
