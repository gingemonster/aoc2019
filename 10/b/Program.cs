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
                    if (lines[y][x] == '#' || lines[y][x] == 'X')
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
                    var anglebetween = GetAngleFromYAxis(a.X, a.Y, o.X, o.Y);
                    var distance = Math.Sqrt(Math.Pow(xdiff, 2) + Math.Pow(ydiff, 2));

                    a.OtherAsteroids.Add(new Tuple<int, Vector>(o.Id, new Vector() { Angle = anglebetween, Distance = distance }));
                });
            });

            var best = asteroids.OrderByDescending(a => a.CountVisibleOtherAsteroids()).First();

            // order otherasteroids from top clockwise
            var orderedotherasteroids = best.OtherAsteroids.OrderBy(o => o.Item2.Angle).ThenBy(o => o.Item2.Distance).ToList();
            int? currentasteroidid = orderedotherasteroids.FirstOrDefault().Item1;
            var asteroidsdestroyed = 0;

            while (orderedotherasteroids.Count() > 1)
            {
                var currentasteroid = orderedotherasteroids.Where(o => o.Item1 == currentasteroidid).FirstOrDefault();
                var asteroid = asteroids.Where(a => a.Id == currentasteroidid).First();

                // blast this one
                orderedotherasteroids.Remove(currentasteroid);
                asteroidsdestroyed++;

                if (asteroidsdestroyed == 200)
                {
                    Console.WriteLine($"{(asteroid.X * 100) + asteroid.Y}");
                }

                // skip to first astroid at new angle
                if (orderedotherasteroids.Count() > 1)
                {
                    // WRONG needs to get the next increasing one and handle 359->0 which it doesnt do yet
                    currentasteroidid = orderedotherasteroids.Where(o => o.Item2.Angle > currentasteroid.Item2.Angle).FirstOrDefault()?.Item1;

                    // if none start at zero again
                    if (currentasteroidid == null)
                    {
                        currentasteroidid = orderedotherasteroids.First().Item1;
                    }
                }
            }

            var remainingasteroid = asteroids.Where(a => a.Id == orderedotherasteroids.FirstOrDefault().Item1).FirstOrDefault();
        }

        private static double GetAngleFromYAxis(double x1, double y1, double x2, double y2)
        {
            var w = x2 - x1;
            var h = y2 - y1;
            var radians = Math.Atan2(h, w);

            // get rid of negative
            var bearingDegrees = radians * (180.0 / Math.PI);

            // rotate 90 to be relative to Y axis
            bearingDegrees += 90;

            bearingDegrees = bearingDegrees > 0 ? bearingDegrees : (360 + bearingDegrees); // correct discontinuity
            bearingDegrees = bearingDegrees == 360 ? 0 : bearingDegrees; // correct 360 to zero

            return bearingDegrees;
        }
    }
}
