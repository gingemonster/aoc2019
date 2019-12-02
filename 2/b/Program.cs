namespace Aoc
{
    using System;
    using System.IO;

    internal class Program
        {
            private static void Main(string[] args)
            {
                var program = File.ReadAllText(args[0]).Split(',');
                var unchangedopcodes = Array.ConvertAll(program, s => int.Parse(s));

                for (var n = 0; n < 100; n++)
                {
                    for (var v = 0; v < 100; v++)
                    {
                        var finished = false;
                        var opcodes = (int[])unchangedopcodes.Clone();
                        opcodes[1] = n;
                        opcodes[2] = v;

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

                        Console.WriteLine(opcodes[0]);
                        if (opcodes[0] == 19690720)
                        {
                            Console.WriteLine("noun=" + n);
                            Console.WriteLine("verb=" + v);
                            Console.WriteLine((100 * n) + v);
                            Console.ReadLine();
                        }
                    }
                }
            }
        }
    }
