namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Ingredient
    {
        public Ingredient(string name, int quantity)
        {
            this.Name = name;
            this.Quantity = quantity;
        }

        public string Name
        {
            get; set;
        }

        public int Quantity
        {
            get; set;
        }
    }
}
