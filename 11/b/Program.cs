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
            var vM = new VM(args[0]);
            var inputs = new Queue<long>();
            var x = 0;
            var y = 0;
            var panels = new List<Panel>();
            var directionfacing = 0;

            while (vM.IsPaused)
            {
                // get input
                var panel = panels.Where(p => p.X == x && p.Y == y).FirstOrDefault();
                long input;

                if (panel == null)
                {
                    input = 0;
                    panel = new Panel() { X = x, Y = y };
                    panels.Add(panel);
                }
                else
                {
                    input = panel.Colour;
                }

                if (panels.Count == 1)
                {
                    input = 1; // first is always white
                }

                inputs.Enqueue(input);

                var outputs = vM.RunProgram(inputs);

                // output 0 is black, 1 if white
                panel.Colour = outputs.Dequeue();

                var direction = outputs.Dequeue();
                directionfacing = GetNewDirection(directionfacing, direction);

                // move
                switch (directionfacing)
                {
                    case 0:
                        y += 1;
                        break;
                    case 90:
                        x += 1;
                        break;
                    case 180:
                        y -= 1;
                        break;
                    case 270:
                        x -= 1;
                        break;
                }
            }

            // get drawing boundaries
            var minx = panels.Min(p => p.X);
            var miny = panels.Min(p => p.Y);
            var maxx = panels.Max(p => p.X);
            var maxy = panels.Max(p => p.Y);

            for (y = maxy; y >= miny; y--)
            {
                for (x = minx; x <= maxx; x++)
                {
                    var panel = panels.Where(p => p.X == x && p.Y == y).FirstOrDefault();

                    Console.Write(panel != null && panel.Colour == 1 ? "■" : " ");
                }

                Console.Write(System.Environment.NewLine);
            }
        }

        private static int GetNewDirection(int directionfacing, long direction)
        {
            directionfacing += direction == 0 ? -90 : 90;

            if (directionfacing < 0)
            {
                directionfacing = 270;
            }

            if (directionfacing >= 360)
            {
                directionfacing = 0;
            }

            return directionfacing;
        }
    }
}
