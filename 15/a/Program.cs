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

            var ingredienceneeded = new Queue<Ingredient>();
            var spareingredience = new List<Ingredient>();

            // find fuel reaction
            var fuelreaction = reactions.Where(r => r.Output.Item1 == "FUEL").FirstOrDefault();
            GetIngredienceFromReaction(fuelreaction, ingredienceneeded, spareingredience, 1);

            // work back to find all ingrediant and quanity needed for 1 fuel
            while (ingredienceneeded.Any(i => i.Name != "ORE"))
            {
                // take first needed
                var needed = ingredienceneeded.Dequeue();

                if (needed.Name == "ORE")
                {
                    // put to back of queue
                    ingredienceneeded.Enqueue(needed);
                    needed = ingredienceneeded.Dequeue();
                }

                // find reaction where it is output
                var reaction = reactions.Where(r => r.Output.Item1 == needed.Name).First();

                // get its parts
                GetIngredienceFromReaction(reaction, ingredienceneeded, spareingredience, needed.Quantity);
            }

            var remaining = ingredienceneeded.Dequeue();
            Console.WriteLine($"{remaining.Quantity} {remaining.Name}");
        }

        private static void GetIngredienceFromReaction(Reaction reaction,  Queue<Ingredient> partsneeded, List<Ingredient> spareingredients, int quantityneeded)
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
                    partsneeded.Enqueue(new Ingredient(i.Name, needed));
                }
            });
        }
    }
}
