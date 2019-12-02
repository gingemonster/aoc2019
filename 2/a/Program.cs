namespace Aoc
{
    using System;
    using System.IO;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var program = File.ReadAllText(args[0]).Split(',');
            var finished = false;

            var opcodes = Array.ConvertAll(program, s => int.Parse(s));

            opcodes[1] = 12;
            opcodes[2] = 2;

            for (int i = 0; i < opcodes.Length; i++)
            {
                switch (opcodes[i])
                {
                    case 99:
                        finished = true;
                        break;
                    case 1:
                        // adds together numbers read from two positions and stores the result in a third position.
                        // The three integers immediately after the opcode tell you these three positions
                        // -the first two indicate the positions from which you should read the input values,
                        // and the third indicates the position at which the output should be stored.
                        opcodes[opcodes[i + 3]] = opcodes[opcodes[i + 1]] + opcodes[opcodes[i + 2]];
                        break;
                    case 2:
                        opcodes[opcodes[i + 3]] = opcodes[opcodes[i + 1]] * opcodes[opcodes[i + 2]];
                        break;
                }

                if (finished)
                {
                    break;
                }

                i += 3;
            }

            Console.WriteLine(string.Join(',', opcodes));

            Console.WriteLine(opcodes[0]);
        }
    }
}
