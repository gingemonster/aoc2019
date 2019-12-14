namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Ingredient
    {
        public Ingredient(string name, long quantity)
        {
            this.Name = name;
            this.Quantity = quantity;
        }

        public string Name
        {
            get; set;
        }

        public long Quantity
        {
            get; set;
        }
    }
}
