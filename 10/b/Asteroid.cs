namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Asteroid
    {
        private List<Tuple<int, Vector>> otherAsteroids = new List<Tuple<int, Vector>>();

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

        public List<Tuple<int, Vector>> OtherAsteroids
        {
            get => this.otherAsteroids; set => this.otherAsteroids = value;
        }

        public int CountVisibleOtherAsteroids()
        {
            return this.OtherAsteroids.GroupBy(o => o.Item2.Angle).Count();
        }
    }
}
