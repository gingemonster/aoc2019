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
            var reactions = new List<Reaction>();
            foreach (var line in File.ReadLines(args[0]))
            {
                reactions.Add(new Reaction(line));
            }

            var ingredienceneeded = new List<Ingredient>();
            var spareingredience = new List<Ingredient>();

            while (true)
            {
                var reaction = FindFirstReactionWithNoDependants(reactions);
                reactions.Remove(reaction);
            }
        }

        private static Reaction FindFirstReactionWithNoDependants(List<Reaction> reactions)
        {
            var matches = reactions.Where(r =>
            {
                var result = reactions.Any(r2 =>
                {
                    var hasany = r2.Inputs.Any(i =>
                    {
                        return i.Name == r.Output.Item1;
                    });
                    return hasany;
                });

                return !result;
            });
            return matches.First();
        }
    }
}
