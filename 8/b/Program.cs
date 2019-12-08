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
            // 0 is black, 1 is white, and 2 is transparent.
            // The layers are rendered with the first layer in front
            // and the last layer in back
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

            var finalimage = new List<int>();

            for (var h = 0; h < height; h++)
            {
                for (var w = 0; w < width; w++)
                {
                    var i = 0;
                    while (i < image.Count())
                    {
                        if (image[i][(h * width) + w] == 0 || (image[i][(h * width) + w] == 1))
                        {
                            if (image[i][(h * width) + w] == 0)
                            {
                                Console.Write(" ");
                            }
                            else
                            {
                                Console.Write(image[i][(h * width) + w]);
                            }

                            finalimage.Add(image[i][(h * width) + w]);
                            break;
                        }

                        i++;
                    }
                }

                Console.Write(Environment.NewLine);
            }
        }
    }
}
