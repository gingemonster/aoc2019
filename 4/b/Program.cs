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
                    Console.WriteLine(i);
                    matches++;
                }
            }

            Console.WriteLine(matches);
        }

        private static bool Matches(string number)
        {
            var repeatedregex = @"(\w)\1+";
            if (number.Length != 6)
            {
                return false;
            }

            var matches = Regex.Matches(number, repeatedregex);
            if (!matches.Any())
            {
                return false;
            }

            var hasmatchlength2 = false;

            foreach (Match match in matches)
            {
                if (match.Value.Length == 2)
                {
                    hasmatchlength2 = true;
                }
            }

            if (!hasmatchlength2)
            {
                return false;
            }

            var chars = number.ToCharArray();
            for (var i = 1; i < chars.Length; i++)
            {
                if (int.Parse(chars[i].ToString()) < int.Parse(chars[i - 1].ToString()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
