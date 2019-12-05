namespace Aoc
{
    using System;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var programtext = File.ReadAllText(args[0]).Split(',');
            var unchangedprogram = Array.ConvertAll(programtext, s => int.Parse(s));

            var finished = false;
            var program = (int[])unchangedprogram.Clone();
            var input = 1;
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
                        program[program[i + 1]] = input;
                        skipcharacters = 1;
                        break;
                    case 4:
                        /*
                        Opcode 4 outputs the value of its only parameter.For example, the instruction 4,50 would output the value at address 50.
                        */
                        output = program[program[i + 1]];
                        skipcharacters = 1;
                        break;
                }

                if (finished)
                {
                    break;
                }

                i += skipcharacters;
            }

            Console.WriteLine(output);
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
    }
}
