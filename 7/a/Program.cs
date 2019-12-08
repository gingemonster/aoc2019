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
            var phasecombinations = Permutations<int>(new List<int>() { 0, 1, 2, 3, 4 });
            var maxpowersofar = 0;
            ICollection<int> bestcombination = new List<int>();

            phasecombinations.ToList().ForEach(c =>
            {
                // loop 5 amps
                // i tried moving this so that its only 0 at the begining otherwise amp a input is last amp e output but ALL tests fail
                var nextampinput = 0;
                for (var i = 0; i < 5; i++)
                {
                    var phaseinput = c.ElementAt(i);
                    var inputs = new Stack<int>();
                    inputs.Push(nextampinput);
                    inputs.Push(phaseinput);
                    nextampinput = RunProgram(inputs, args[0]);
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

        private static int RunProgram(Stack<int> input, string programfile)
        {
            var programtext = File.ReadAllText(programfile).Split(',');
            var program = Array.ConvertAll(programtext, s => int.Parse(s));

            var finished = false;
            var output = -1;

            for (int i = 0; i < program.Length; i++)
            {
                var skipcharacters = 3;
                var opcodearray = program[i].ToString().ToCharArray().Select(o => Convert.ToInt32(o.ToString())).ToArray();

                // get last two digits
                switch (program[i] % 100)
                {
                    case 99:
                        finished = true;
                        break;
                    case 1:
                        // adds together numbers read from two positions and stores the result in a third position.
                        // The three integers immediately after the opcode tell you these three positions
                        // -the first two indicate the positions from which you should read the input values,
                        // and the third indicates the position at which the output should be stored.
                        // WARNING assumes param 3 CANNOT be anything other than mode 0
                        program[program[i + 3]] = GetParameterValue(i, opcodearray, 1, program) + GetParameterValue(i, opcodearray, 2, program);

                        break;
                    case 2:
                        program[program[i + 3]] = GetParameterValue(i, opcodearray, 1, program) * GetParameterValue(i, opcodearray, 2, program);
                        break;
                    case 3:
                        /*
                        Opcode 3 takes a single integer as input and saves it to the position given by its only parameter.For example,
                        the instruction 3,50 would take an input value and store it at address 50.
                        */
                        program[program[i + 1]] = input.Pop();
                        skipcharacters = 1;
                        break;
                    case 4:
                        /*
                        Opcode 4 outputs the value of its only parameter.For example, the instruction 4,50 would output the value at address 50.
                        */
                        output = GetParameterValue(i, opcodearray, 1, program);
                        skipcharacters = 1;
                        break;
                    case 5:
                        if (GetParameterValue(i, opcodearray, 1, program) != 0)
                        {
                            i = GetParameterValue(i, opcodearray, 2, program);
                            skipcharacters = -1;
                        }
                        else
                        {
                            skipcharacters = 2;
                        }

                        break;
                    case 6:
                        if (GetParameterValue(i, opcodearray, 1, program) == 0)
                        {
                            i = GetParameterValue(i, opcodearray, 2, program);
                            skipcharacters = -1;
                        }
                        else
                        {
                            skipcharacters = 2;
                        }

                        break;
                    case 7:
                        program[program[i + 3]] = GetParameterValue(i, opcodearray, 1, program) < GetParameterValue(i, opcodearray, 2, program) ? 1 : 0;

                        break;
                    case 8:
                        program[program[i + 3]] = GetParameterValue(i, opcodearray, 1, program) == GetParameterValue(i, opcodearray, 2, program) ? 1 : 0;

                        break;
                }

                if (finished)
                {
                    break;
                }

                i += skipcharacters;
            }

            return output;
        }

        private static int GetParameterValue(int currentoffset, int[] opcodearray, int parameterindex, int[] program)
        {
            // get this params mode from op code, starting two characters from right as first two are op code
            int mode;

            // some params will not have a mode in the op code so assume zero
            if (opcodearray.Length - 2 - parameterindex >= 0)
            {
                mode = opcodearray[opcodearray.Length - 2 - parameterindex];
            }
            else
            {
                mode = 0;
            }

            // mode 0 position
            if (mode == 0)
            {
                return program[program[parameterindex + currentoffset]];
            }

            // mode 1 value
            else if (mode == 1)
            {
                return program[parameterindex + currentoffset];
            }
            else
            {
                throw new Exception("unknown paramater mode");
            }
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
