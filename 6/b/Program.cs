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
            var relationships = new List<Tuple<string, string>>();

            foreach (var line in File.ReadLines(args[0]))
            {
                var splits = line.Split(')');

                // store orbiting object as key, object being orbited as
                relationships.Add(new Tuple<string, string>(splits[0], splits[1]));
            }

            var youpath = BuildOrbitPath("YOU", relationships);
            var santapath = BuildOrbitPath("SAN", relationships);

            // first first shared object
            var firstintersectobject = youpath.Intersect(santapath).FirstOrDefault();

            Console.WriteLine(youpath.IndexOf(firstintersectobject) + santapath.IndexOf(firstintersectobject));
        }

        private static List<string> BuildOrbitPath(string currentobject, List<Tuple<string, string>> relationships)
        {
            var path = new List<string>();

            while (currentobject != "COM")
            {
                // find next direct orbit
                var relationship = relationships.Where(r => r.Item2 == currentobject).FirstOrDefault();
                path.Add(relationship.Item1);
                currentobject = relationship.Item1;
            }

            return path;
        }
    }
}
