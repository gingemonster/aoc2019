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
            var width = 25;
            var height = 6;

            var input = File.ReadAllText(args[0]).ToCharArray();

            var numlayers = input.Length / (width * height);
            var image = new List<List<int>>();

            for (var l = 0; l < numlayers; l++)
            {
                var layer = new List<int>();
                for (var h = 0; h < height; h++)
                {
                    for (var w = 0; w < width; w++)
                    {
                        layer.Add(int.Parse(input[(l * height * width) + (h * width) + w].ToString()));
                    }
                }

                image.Add(layer);
            }

            var fewestzerolayer = image.OrderBy(l => l.Count(p => p == 0)).FirstOrDefault();

            var answer = fewestzerolayer.Count(p => p == 1) * fewestzerolayer.Count(p => p == 2);

            Console.WriteLine(answer);
        }
    }
}
