namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Amp
    {
        private int[] program;
        private string programfile;

        public Amp(string programfile)
        {
            this.programfile = programfile;
            this.Reset();
        }

        public void Reset()
        {
            var programtext = File.ReadAllText(this.programfile).Split(',');
            this.program = Array.ConvertAll(programtext, s => int.Parse(s));
        }

        public int Run(Stack<int> input)
        {
            var finished = false;
            var output = -1;

            for (int i = 0; i < this.program.Length; i++)
            {
                var skipcharacters = 3;
                var opcodearray = this.program[i].ToString().ToCharArray().Select(o => Convert.ToInt32(o.ToString())).ToArray();

                // get last two digits
                switch (this.program[i] % 100)
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
                        this.program[this.program[i + 3]] = this.GetParameterValue(i, opcodearray, 1, this.program) + this.GetParameterValue(i, opcodearray, 2, this.program);

                        break;
                    case 2:
                        this.program[this.program[i + 3]] = this.GetParameterValue(i, opcodearray, 1, this.program) * this.GetParameterValue(i, opcodearray, 2, this.program);
                        break;
                    case 3:
                        /*
                        Opcode 3 takes a single integer as input and saves it to the position given by its only parameter.For example,
                        the instruction 3,50 would take an input value and store it at address 50.
                        */
                        this.program[this.program[i + 1]] = input.Pop();
                        skipcharacters = 1;
                        break;
                    case 4:
                        /*
                        Opcode 4 outputs the value of its only parameter.For example, the instruction 4,50 would output the value at address 50.
                        */
                        output = this.GetParameterValue(i, opcodearray, 1, this.program);
                        skipcharacters = 1;
                        break;
                    case 5:
                        if (this.GetParameterValue(i, opcodearray, 1, this.program) != 0)
                        {
                            i = this.GetParameterValue(i, opcodearray, 2, this.program);
                            skipcharacters = -1;
                        }
                        else
                        {
                            skipcharacters = 2;
                        }

                        break;
                    case 6:
                        if (this.GetParameterValue(i, opcodearray, 1, this.program) == 0)
                        {
                            i = this.GetParameterValue(i, opcodearray, 2, this.program);
                            skipcharacters = -1;
                        }
                        else
                        {
                            skipcharacters = 2;
                        }

                        break;
                    case 7:
                        this.program[this.program[i + 3]] = this.GetParameterValue(i, opcodearray, 1, this.program) < this.GetParameterValue(i, opcodearray, 2, this.program) ? 1 : 0;

                        break;
                    case 8:
                        this.program[this.program[i + 3]] = this.GetParameterValue(i, opcodearray, 1, this.program) == this.GetParameterValue(i, opcodearray, 2, this.program) ? 1 : 0;

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

        private int GetParameterValue(int currentoffset, int[] opcodearray, int parameterindex, int[] program)
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
                return this.program[program[parameterindex + currentoffset]];
            }

            // mode 1 value
            else if (mode == 1)
            {
                return this.program[parameterindex + currentoffset];
            }
            else
            {
                throw new Exception("unknown paramater mode");
            }
        }
    }
}
