namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Wire
    {
        private string[] moves;
        private Tuple<int, int> currentLocation = new Tuple<int, int>(0, 0);
        private HashSet<string> visitedLocations = new HashSet<string>();

        public Wire(string moves)
        {
            // parse instructions
            this.moves = moves.Split(',');
            this.ProcessMoves();
        }

        public HashSet<string> VisitedLocations
        {
            get => this.visitedLocations;
            set => this.visitedLocations = value;
        }

        public static int CalculateDistance(string location)
        {
            var x = int.Parse(location.Split(',')[0]);
            var y = int.Parse(location.Split(',')[1]);
            return Math.Abs(x) + Math.Abs(y);
        }

        private void ProcessMoves()
        {
            this.moves.ToList<string>().ForEach(s => this.ProcessMove(s));
        }

        private void ProcessMove(string move)
        {
            var direction = move.Substring(0, 1);
            var distance = int.Parse(move.Substring(1));
            Tuple<int, int> movexy;

            // dont bother adding 0,0 as that location is ignored
            switch (direction)
            {
                case "L":
                    movexy = new Tuple<int, int>(-1, 0);
                    break;
                case "R":
                    movexy = new Tuple<int, int>(1, 0);
                    break;
                case "U":
                    movexy = new Tuple<int, int>(0, -1);
                    break;
                case "D":
                    movexy = new Tuple<int, int>(0, 1);
                    break;
                default:
                    movexy = new Tuple<int, int>(0, 0);
                    break;
            }

            while (distance > 0)
            {
                this.currentLocation = new Tuple<int, int>(this.currentLocation.Item1 + movexy.Item1, this.currentLocation.Item2 + movexy.Item2);
                this.VisitedLocations.Add($"{this.currentLocation.Item1},{this.currentLocation.Item2}");
                distance--;
            }
        }
    }
}
