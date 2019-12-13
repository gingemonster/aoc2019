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
            long score = 0;
            List<GameBlock> gameblocks = new List<GameBlock>();

            while (true)
            {
                var outputs = vM.RunProgram(inputs);
                UpdateFrame(gameblocks, GenerateGameBlocks(outputs, ref score));

                // get padel position
                var padel = gameblocks.FirstOrDefault(x => x.BlockType == BlockType.HorizontalPaddle);

                // get ball position
                var ball = gameblocks.FirstOrDefault(x => x.BlockType == BlockType.Ball);

                // move padel towards ball x position, hopefully we always have time to get there?
                var joystickinput = padel.X < ball.X ? 1 : padel.X == ball.X ? 0 : -1;
                inputs.Enqueue(joystickinput);

                Console.WriteLine($"blocks remaining {gameblocks.Count(g => g.BlockType == BlockType.Block)}");
                if (!gameblocks.Any(g => g.BlockType == BlockType.Block))
                {
                    break;
                }
            }

            Console.WriteLine(score);
        }

        private static List<GameBlock> GenerateGameBlocks(Queue<long> outputs, ref long score)
        {
            var result = new List<GameBlock>();

            while (outputs.Any())
            {
                var x = outputs.Dequeue();
                var y = outputs.Dequeue();
                if (x == -1 && y == 0)
                {
                    score = outputs.Dequeue();
                }
                else
                {
                    result.Add(new GameBlock() { X = x, Y = y, BlockType = Enum.Parse<BlockType>(outputs.Dequeue().ToString()) });
                }
            }

            return result;
        }

        private static void UpdateFrame(List<GameBlock> lastframe, List<GameBlock> changes)
        {
            changes.ForEach(c =>
            {
                var toupdated = lastframe.Where(cl => cl.X == c.X && cl.Y == c.Y).FirstOrDefault();
                if (toupdated == null)
                {
                    toupdated = c;
                    lastframe.Add(toupdated);
                }

                toupdated.BlockType = c.BlockType;
            });
        }
    }
}
