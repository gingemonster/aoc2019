namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
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

            var axis = new List<string> { "x", "y", "z" };
            var repeatsat = new List<long>();

            axis.ForEach(a =>
            {
                long ticks = 0;
                var states = new HashSet<string>();

                while (true)
                {
                    moons.ForEach(m =>
                    {
                        moons.Where(o => o.Id != m.Id).ToList().ForEach(o =>
                        {
                            m.ApplyGravity(o, a);
                        });
                    });

                    moons.ForEach(m => m.ApplyVelocity(a));
                    var state = GetState(moons, a);
                    if (states.Contains(state))
                    {
                        Console.WriteLine($"{a} repeated at {ticks}");
                        repeatsat.Add(ticks);
                        break;
                    }

                    states.Add(state);

                    ticks++;
                }
            });

            Console.WriteLine($"LCM {LCM(repeatsat.ToArray())}");
        }

        private static string GetState(List<Moon> moons, string axis)
        {
            var result = new StringBuilder();
            moons.ForEach(m =>
            {
                switch (axis)
                {
                    case "x":
                        result.Append($"{m.X},{m.VX},");
                        break;
                    case "y":
                        result.Append($"{m.Y},{m.VY},");
                        break;
                    case "z":
                        result.Append($"{m.Z},{m.VZ},");
                        break;
                }
            });
            return result.ToString();
        }

        private static long LCM(long[] numbers)
        {
            return numbers.Aggregate(Lcm);
        }

        private static long Lcm(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }

        private static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}
