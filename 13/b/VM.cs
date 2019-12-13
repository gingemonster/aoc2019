namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class VM
    {
        private string programfile;
        private long[] program;
        private long currentmemorylocation = 0;
        private long relativeoffset = 0;
        private Queue<long> outputs = new Queue<long>();

        public VM(string programfile)
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

            // make memory much bigger x 10?
            this.program = Array.ConvertAll(programtext, s => long.Parse(s));
            Array.Resize(ref this.program, this.program.Length * 10);
            this.currentmemorylocation = 0;
            this.IsPaused = true;
        }

        public Queue<long> RunProgram(Queue<long> input)
        {
            var finished = false;

            for (long i = this.currentmemorylocation; i < this.program.Length; i++)
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
                        this.program[this.program[i + 3] + (this.GetModeFromOpCode(opcodearray, 3) == 0 ? 0 : this.relativeoffset)] = this.GetParameterValue(i, opcodearray, 1, this.program) + this.GetParameterValue(i, opcodearray, 2, this.program);

                        break;
                    case 2:
                        // multiple two params
                        this.program[this.program[i + 3] + (this.GetModeFromOpCode(opcodearray, 3) == 0 ? 0 : this.relativeoffset)] = this.GetParameterValue(i, opcodearray, 1, this.program) * this.GetParameterValue(i, opcodearray, 2, this.program);
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

                        // has to support relative positioning!
                        var mode = this.GetModeFromOpCode(opcodearray, 1);
                        if (mode == 0)
                        {
                            this.program[this.program[i + 1] + (this.GetModeFromOpCode(opcodearray, 3) == 0 ? 0 : this.relativeoffset)] = input.Dequeue();
                        }
                        else
                        {
                            this.program[this.program[i + 1] + this.relativeoffset] = input.Dequeue();
                        }

                        skipcharacters = 1;
                        break;
                    case 4:
                        /*
                        Opcode 4 outputs the value of its only parameter.For example, the instruction 4,50 would output the value at address 50.
                        */
                        this.outputs.Enqueue(this.GetParameterValue(i, opcodearray, 1, this.program));
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
                        this.program[this.program[i + 3] + (this.GetModeFromOpCode(opcodearray, 3) == 0 ? 0 : this.relativeoffset)] = this.GetParameterValue(i, opcodearray, 1, this.program) < this.GetParameterValue(i, opcodearray, 2, this.program) ? 1 : 0;

                        break;
                    case 8:
                        this.program[this.program[i + 3] + (this.GetModeFromOpCode(opcodearray, 3) == 0 ? 0 : this.relativeoffset)] = this.GetParameterValue(i, opcodearray, 1, this.program) == this.GetParameterValue(i, opcodearray, 2, this.program) ? 1 : 0;

                        break;
                    case 9:
                        this.relativeoffset += this.GetParameterValue(i, opcodearray, 1, this.program);
                        skipcharacters = 1;
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

            return this.outputs;
        }

        private long GetParameterValue(long currentoffset, int[] opcodearray, int parameterindex, long[] program)
        {
            // get this params mode from op code, starting two characters from right as first two are op code
            int mode = this.GetModeFromOpCode(opcodearray, parameterindex);

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

            // mode 2 relative
            else if (mode == 2)
            {
                // WARNING NOT SURE IN WHICH BRAKETS relativeoffset belongs
                return program[program[parameterindex + currentoffset] + this.relativeoffset];
            }
            else
            {
                throw new Exception("unknown paramater mode");
            }
        }

        private int GetModeFromOpCode(int[] opcodearray, int parameterindex)
        {
            var mode = 0;
            if (opcodearray.Length - 2 - parameterindex >= 0)
            {
                mode = opcodearray[opcodearray.Length - 2 - parameterindex];
            }

            return mode;
        }
    }
}
