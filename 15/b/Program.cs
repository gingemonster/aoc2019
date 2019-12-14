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
            var knownorderofreactions = GetKnowOrderOfReactions(ParseReactions(args[0]));
            long fuelneeded = 1;

            var orerequired = CalculateOreNeeedtoGenerateFuel(knownorderofreactions, fuelneeded);

            // we can generate at least this much fuel from one trillion ore
            var minfuelfortrillionore = 1000000000000 / orerequired;

            var guess = minfuelfortrillionore;
            long lastoreneeded = 0;
            var guessteps = 1000000;
            var guessesgoingup = true;

            while (lastoreneeded != 1000000000000)
            {
                lastoreneeded = CalculateOreNeeedtoGenerateFuel(knownorderofreactions, guess);

                if (guessteps == 1)
                {
                    if (CalculateOreNeeedtoGenerateFuel(knownorderofreactions, guess) < 1000000000000 && CalculateOreNeeedtoGenerateFuel(knownorderofreactions, guess + 1) > 1000000000000)
                    {
                        break;
                    }

                    if (CalculateOreNeeedtoGenerateFuel(knownorderofreactions, guess) > 1000000000000 && CalculateOreNeeedtoGenerateFuel(knownorderofreactions, guess - 1) < 1000000000000)
                    {
                        guess--;
                        break;
                    }
                }

                if (lastoreneeded < 1000000000000)
                {
                    if (!guessesgoingup)
                    {
                        guessesgoingup = true;
                        guessteps = guessteps / 10;
                    }

                    guess += guessteps;
                }
                else
                {
                    if (guessesgoingup)
                    {
                        guessesgoingup = false;
                        guessteps = guessteps / 10;
                    }

                    guess -= guessteps;
                }
            }

            Console.WriteLine($"{lastoreneeded} ore required for {guess} fuel");
        }

        private static long CalculateOreNeeedtoGenerateFuel(List<Reaction> knownorderofreactions, long fuelneeded)
        {
            var ingredienceneeded = new List<Ingredient>();
            var reactions = new Queue<Reaction>(knownorderofreactions);

            var spareingredience = new List<Ingredient>();

            // work back to find all ingrediant and quanity needed for 1 fuel
            while (true)
            {
                var reaction = reactions.Dequeue();

                if (reaction.Output.Item1 == "FUEL")
                {
                    ingredienceneeded.Add(new Ingredient("FUEL", fuelneeded));
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
            return orerequired;
        }

        private static List<Reaction> GetKnowOrderOfReactions(List<Reaction> reactions)
        {
            var known = new List<Reaction>();
            while (reactions.Any())
            {
                var reaction = FindFirstReactionWithNoDependants(reactions);
                reactions.Remove(reaction);
                known.Add(reaction);
            }

            return known;
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

        private static void GetIngredienceFromReaction(Reaction reaction, List<Ingredient> partsneeded, List<Ingredient> spareingredients, long quantityneeded)
        {
            // work out how many times the partsneeded
            var multiples = (long)Math.Ceiling(quantityneeded / (double)reaction.Output.Item2);

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
