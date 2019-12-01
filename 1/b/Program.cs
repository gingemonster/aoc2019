namespace Aoc
{
    using System;
    using System.IO;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var totalfuel = 0d;
            foreach (var line in File.ReadLines(args[0]))
            {
                var modulefuel = Math.Floor((double)(int.Parse(line) / 3)) - 2;

                // calculate fuel for fuel
                var lastfuel = modulefuel;
                while (true)
                {
                    var fuelfuel = Math.Floor((double)(lastfuel / 3)) - 2;
                    if (fuelfuel > 0)
                    {
                        modulefuel += fuelfuel;
                        lastfuel = fuelfuel;
                    }
                    else
                    {
                        break;
                    }
                }

                totalfuel += modulefuel;
            }

            Console.WriteLine(totalfuel);
        }
    }
}
