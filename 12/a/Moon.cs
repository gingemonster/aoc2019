namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Moon
    {
        public Moon(int x, int y, int z, int id)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Id = id;
        }

        public int X
        {
            get; set;
        }

        public int Y
        {
            get; set;
        }

        public int Z
        {
            get; set;
        }

        public int Id { get; private set; }

        public int VX
        {
            get; set;
        }

        public int VY
        {
            get; set;
        }

        public int VZ
        {
            get; set;
        }

        public void ApplyGravity(Moon other)
        {
            this.VX += this.X < other.X ? 1 : this.X == other.X ? 0 : -1;
            this.VY += this.Y < other.Y ? 1 : this.Y == other.Y ? 0 : -1;
            this.VZ += this.Z < other.Z ? 1 : this.Z == other.Z ? 0 : -1;
        }

        public void ApplyVelocity()
        {
            this.X += this.VX;
            this.Y += this.VY;
            this.Z += this.VZ;
        }

        public int CalculatePotentialEnergy()
        {
            return Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z);
        }

        public int CalculateKineticEnergy()
        {
            return Math.Abs(this.VX) + Math.Abs(this.VY) + Math.Abs(this.VZ);
        }

        public int CalculateTotalEnergy()
        {
            return this.CalculateKineticEnergy() * this.CalculatePotentialEnergy();
        }
    }
}
