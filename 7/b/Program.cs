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
            var phasecombinations = Permutations<int>(new List<int>() { 5, 6, 7, 8, 9 });
            var maxpowersofar = 0;
            ICollection<int> bestcombination = new List<int>();
            var amps = new List<Amp>();
            amps.Add(new Amp(args[0]));
            amps.Add(new Amp(args[0]));
            amps.Add(new Amp(args[0]));
            amps.Add(new Amp(args[0]));
            amps.Add(new Amp(args[0]));

            phasecombinations.ToList().ForEach(c =>
            {
                amps.ForEach(a => a.Reset());

                var nextampinput = 0;

                // loop 5 amps until output of E doesnt change
                while (true)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        var phaseinput = c.ElementAt(i);
                        var inputs = new Stack<int>();
                        inputs.Push(nextampinput);
                        if (!amps[i].IsPaused)
                        {
                            // is first run so add phaseinput
                            inputs.Push(phaseinput);
                        }

                        nextampinput = amps[i].RunProgram(inputs);
                    }

                    if (!amps[4].IsPaused)
                    {
                        break;
                    }
                }

                if (nextampinput > maxpowersofar)
                {
                    maxpowersofar = nextampinput;
                    bestcombination = c;
                }

                Console.WriteLine($"power {nextampinput} from combination {string.Join(",", c)}");
            });

            Console.WriteLine($"power {maxpowersofar} from combination {string.Join(",", bestcombination)}");
        }

        private static ICollection<ICollection<T>> Permutations<T>(ICollection<T> list)
        {
            var result = new List<ICollection<T>>();
            if (list.Count == 1)
            { // If only one possible permutation
                result.Add(list); // Add it and return it
                return result;
            }

            foreach (var element in list)
            { // For each element in that list
                var remainingList = new List<T>(list);
                remainingList.Remove(element); // Get a list containing everything except of chosen element
                foreach (var permutation in Permutations<T>(remainingList))
                { // Get all possible sub-permutations
                    permutation.Add(element); // Add that element
                    result.Add(permutation);
                }
            }

            return result;
        }
    }
}
