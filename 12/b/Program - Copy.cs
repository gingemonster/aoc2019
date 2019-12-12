namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var moons = new List<Moon>();
            var i = 0;

            foreach (var line in File.ReadLines(args[0]))
            {
                var match = Regex.Match(line, "<x=(.*), y=(.*), z=(.*)>");
                moons.Add(new Moon(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), i));
                i++;
            }

            var ticks = 0;
            var states = new HashSet<string>();

            while (true)
            {
                moons.ForEach(m =>
                {
                    moons.Where(o => o.Id != m.Id).ToList().ForEach(o =>
                    {
                        m.ApplyGravity(o);
                    });
                });

                moons.ForEach(m => m.ApplyVelocity());
                var state = GetState(moons);
                if (!states.Contains(state))
                {
                    states.Add(state);
                }
                else
                {
                    break;
                }

                ticks++;
            }

            Console.WriteLine(ticks);
        }

        private static string GetState(List<Moon> moons)
        {
            var result = string.Empty;
            moons.ForEach(m => result += $"{m.X},{m.Y},{m.Z},{m.VX},{m.VY},{m.VZ}");
            return result;
        }
    }
}
