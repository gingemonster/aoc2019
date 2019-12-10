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
            var asteroids = new List<Asteroid>();
            var lines = File.ReadLines(args[0]).ToArray();
            var numcolumns = lines.First().Length;
            var i = 0;

            for (var y = 0; y < lines.Count(); y++)
            {
                for (var x = 0; x < numcolumns; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        asteroids.Add(new Asteroid() { X = x, Y = y, Id = i });
                        i++;
                    }
                }
            }

            asteroids.ForEach(a =>
            {
                asteroids.Where(o => o.Id != a.Id).ToList().ForEach(o =>
                {
                    var xdiff = o.X - a.X;
                    var ydiff = o.Y - a.Y;
                    var anglebetween = Math.Atan2(xdiff, ydiff);

                    a.OtherAsteroids.Add(new Tuple<int, double>(o.Id, anglebetween));
                });
            });

            var best = asteroids.OrderByDescending(a => a.OtherAsteroids.GroupBy(o => o.Item2).Count()).First();
        }
    }
}
