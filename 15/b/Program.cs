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
            var ingredienceneeded = new List<Ingredient>();
            var spareingredience = new List<Ingredient>();
            var remainingore = 1000000000000;
            var fuelmade = 0;

            while (remainingore > 0)
            {
                var reactions = ParseReactions(args[0]);

                // work back to find all ingrediant and quanity needed for 1 fuel
                while (true)
                {
                    var reaction = FindFirstReactionWithNoDependants(reactions);
                    reactions.Remove(reaction);

                    if (reaction.Output.Item1 == "FUEL")
                    {
                        ingredienceneeded.Add(new Ingredient("FUEL", 1));
                    }

                    // take first needed
                    var needed = ingredienceneeded.Where(i => i.Name == reaction.Output.Item1).First();
                    ingredienceneeded.Remove(needed);

                    // get its parts
                    GetIngredienceFromReaction(reaction, ingredienceneeded, spareingredience, needed.Quantity);

                    if (ingredienceneeded.Count == 1)
                    {
                        break;
                    }
                }

                var orerequired = ingredienceneeded[0].Quantity;
                ingredienceneeded.Clear();
                remainingore -= orerequired;
                fuelmade++;
                Console.WriteLine(remainingore);
            }

            Console.WriteLine($"fuel {fuelmade}");
        }

        private static List<Reaction> ParseReactions(string inputfile)
        {
            var reactions = new List<Reaction>();
            foreach (var line in File.ReadLines(inputfile))
            {
                reactions.Add(new Reaction(line));
            }

            return reactions;
        }

        private static void GetIngredienceFromReaction(Reaction reaction, List<Ingredient> partsneeded, List<Ingredient> spareingredients, int quantityneeded)
        {
            // work out how many times the partsneeded
            var multiples = (int)Math.Ceiling(quantityneeded / (double)reaction.Output.Item2);

            // work out how much spare we would generate
            var sparegenerated = reaction.Output.Item2 - quantityneeded;

            if (sparegenerated > 0 && spareingredients.Any(s => s.Name == reaction.Output.Item1))
            {
                spareingredients.Where(s => s.Name == reaction.Output.Item1).First().Quantity += sparegenerated;
            }
            else if (sparegenerated > 0)
            {
                spareingredients.Add(new Ingredient(reaction.Output.Item1, sparegenerated));
            }

            reaction.Inputs.ForEach(i =>
            {
                var needed = i.Quantity * multiples;

                // check spare ingredients to see if we need any more
                var numberspare = spareingredients.Where(s => s.Name == i.Name).Any() ? spareingredients.Where(i => i.Name == i.Name).FirstOrDefault().Quantity : 0;

                if (numberspare > 0)
                {
                    // there are more spare than we need
                    if (numberspare >= needed)
                    {
                        // decrease the number of spares
                        spareingredients.Where(s => s.Name == i.Name).First().Quantity -= quantityneeded;
                        needed = 0;
                    }
                    else
                    {
                        // there are less spare than we need
                        needed -= numberspare;

                        // remove spares as we have none left (if there were any spare to start with)
                        spareingredients.RemoveAll(s => s.Name == i.Name);
                    }
                }

                if (partsneeded.Any(x => x.Name == i.Name))
                {
                    partsneeded.Where(x => x.Name == i.Name).FirstOrDefault().Quantity += needed;
                }
                else
                {
                    partsneeded.Add(new Ingredient(i.Name, needed));
                }
            });
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
