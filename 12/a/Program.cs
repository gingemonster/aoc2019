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
            while (ticks < 1000)
            {
                moons.ForEach(m =>
                {
                    moons.Where(o => o.Id != m.Id).ToList().ForEach(o =>
                    {
                        m.ApplyGravity(o);
                    });
                });

                moons.ForEach(m => m.ApplyVelocity());

                ticks++;
            }

            Console.WriteLine(moons.Sum(m => m.CalculateTotalEnergy()));
        }
    }
}
