namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Amp
    {
        private string programfile;
        private int[] program;
        private int currentmemorylocation = 0;
        private int lastoutput = 0;

        public Amp(string programfile)
        {
            this.programfile = programfile;
            this.Reset();
        }

        public bool IsPaused
        {
            get; set;
        }

        public void Reset()
        {
            var programtext = File.ReadAllText(this.programfile).Split(',');
            this.program = Array.ConvertAll(programtext, s => int.Parse(s));
            this.currentmemorylocation = 0;
            this.IsPaused = false;
        }

        public int RunProgram(Stack<int> input)
        {
            var finished = false;

            for (var i = this.currentmemorylocation; i < this.program.Length; i++)
            {
                // when we pause we need to continue from the current memory location
                var skipcharacters = 3;
                var opcodearray = this.program[i].ToString().ToCharArray().Select(o => Convert.ToInt32(o.ToString())).ToArray();

                // get last two digits
                switch (this.program[i] % 100)
                {
                    case 99:
                        finished = true;
                        this.IsPaused = false;
                        break;
                    case 1:
                        // adds together numbers read from two positions and stores the result in a third position.
                        // The three integers immediately after the opcode tell you these three positions
                        // -the first two indicate the positions from which you should read the input values,
                        // and the third indicates the position at which the output should be stored.
                        // WARNING assumes param 3 CANNOT be anything other than mode 0
                        this.program[this.program[i + 3]] = GetParameterValue(i, opcodearray, 1, this.program) + GetParameterValue(i, opcodearray, 2, this.program);

                        break;
                    case 2:
                        this.program[this.program[i + 3]] = GetParameterValue(i, opcodearray, 1, this.program) * GetParameterValue(i, opcodearray, 2, this.program);
                        break;
                    case 3:
                        /*
                        Opcode 3 takes a single integer as input and saves it to the position given by its only parameter.For example,
                        the instruction 3,50 would take an input value and store it at address 50.
                        */
                        if (!input.Any())
                        {
                            finished = true;
                            this.IsPaused = true;
                            skipcharacters = 0;
                            break;
                        }

                        this.program[this.program[i + 1]] = input.Pop();
                        skipcharacters = 1;
                        break;
                    case 4:
                        /*
                        Opcode 4 outputs the value of its only parameter.For example, the instruction 4,50 would output the value at address 50.
                        */
                        this.lastoutput = GetParameterValue(i, opcodearray, 1, this.program);
                        skipcharacters = 1;
                        break;
                    case 5:
                        if (GetParameterValue(i, opcodearray, 1, this.program) != 0)
                        {
                            i = GetParameterValue(i, opcodearray, 2, this.program);
                            skipcharacters = -1;
                        }
                        else
                        {
                            skipcharacters = 2;
                        }

                        break;
                    case 6:
                        if (GetParameterValue(i, opcodearray, 1, this.program) == 0)
                        {
                            i = GetParameterValue(i, opcodearray, 2, this.program);
                            skipcharacters = -1;
                        }
                        else
                        {
                            skipcharacters = 2;
                        }

                        break;
                    case 7:
                        this.program[this.program[i + 3]] = GetParameterValue(i, opcodearray, 1, this.program) < GetParameterValue(i, opcodearray, 2, this.program) ? 1 : 0;

                        break;
                    case 8:
                        this.program[this.program[i + 3]] = GetParameterValue(i, opcodearray, 1, this.program) == GetParameterValue(i, opcodearray, 2, this.program) ? 1 : 0;

                        break;
                    default:
                        throw new Exception("invalid ops code");
                }

                i += skipcharacters;

                if (finished)
                {
                    this.currentmemorylocation = i;
                    break;
                }
            }

            return this.lastoutput;
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
