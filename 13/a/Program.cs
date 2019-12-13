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
            var vM = new VM(args[0]);
            var inputs = new Queue<long>();

            var outputs = vM.RunProgram(inputs);
            var gameblocks = GenerateGameBlocks(outputs);

            Console.WriteLine(gameblocks.Count(x => x.BlockType == BlockType.Block));
        }

        private static List<GameBlock> GenerateGameBlocks(Queue<long> outputs)
        {
            var result = new List<GameBlock>();

            while (outputs.Any())
            {
                result.Add(new GameBlock() { X = outputs.Dequeue(), Y = outputs.Dequeue(), BlockType = Enum.Parse<BlockType>(outputs.Dequeue().ToString()) });
            }

            return result;
        }
    }
}
