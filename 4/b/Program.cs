namespace Aoc
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var matches = 0;
            for (var i = 240920; i <= 789857; i++)
            {
                if (Matches(i.ToString()))
                {
                    matches++;
                }
            }

            Console.WriteLine(matches);
        }

        private static bool Matches(string number)
        {
            if (number.Length != 6)
            {
                return false;
            }

            if (new string(number.ToCharArray().OrderBy(s => s).ToArray()) != number)
            {
                return false;
            }

            var repeatedregex = @"(\w)\1+";
            var matches = Regex.Matches(number, repeatedregex);

            return matches.Any(m => m.Value.Length == 2);
        }
    }
}
