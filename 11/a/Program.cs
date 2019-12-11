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
            var panels = new Dictionary<string, long>();
            var directionfacing = 0;

            while (vM.IsPaused)
            {
                // get input
                var input = panels.ContainsKey($"{x},{y}") ? panels[$"{x},{y}"] : 0;
                inputs.Enqueue(input);

                var outputs = vM.RunProgram(inputs);

                // output 0 is black, 1 if white
                panels[$"{x},{y}"] = outputs.Dequeue();

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

            Console.WriteLine(panels.Count());
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
