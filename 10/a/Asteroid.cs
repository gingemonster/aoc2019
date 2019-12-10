namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Asteroid
    {
        private List<Tuple<int, double>> otherAsteroids = new List<Tuple<int, double>>();

        public int Id
        {
            get; set;
        }

        public int X
        {
            get; set;
        }

        public int Y
        {
            get; set;
        }

        public List<Tuple<int, double>> OtherAsteroids
        {
            get => this.otherAsteroids; set => this.otherAsteroids = value;
        }
    }
}
