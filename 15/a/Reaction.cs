namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Reaction
    {
        private List<Ingredient> inputs = new List<Ingredient>();

        public Reaction(string recipe)
        {
            var matches = Regex.Matches(recipe, @"((\d+)\s(\w+))");
            for (var i = 0; i < matches.Count - 1; i++)
            {
                var match = matches[i];
                this.inputs.Add(new Ingredient(match.Groups[3].Value, int.Parse(match.Groups[2].Value)));
            }

            var lastmatch = matches[matches.Count - 1];
            this.Output = new Tuple<string, int>(lastmatch.Groups[3].Value, int.Parse(lastmatch.Groups[2].Value));
        }

        public List<Ingredient> Inputs
        {
            get => this.inputs; set => this.inputs = value;
        }

        public Tuple<string, int> Output
        {
            get; set;
        }
    }
}
