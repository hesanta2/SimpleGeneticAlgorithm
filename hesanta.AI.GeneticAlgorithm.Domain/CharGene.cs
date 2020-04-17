using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace hesanta.AI.GA.Domain
{
    public class CharGene : IGene
    {
        private static Random random = new Random((int)DateTime.Today.Ticks);

        public string Value { get; protected set; } = " ";

        public CharGene() { }

        public void Randomize()
        {
            this.Value = ((char)random.Next(32, 255)).ToString();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj.ToString().Equals(Value);
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<string>.Default.GetHashCode(Value);
        }

        public void Mutate()
        {
            var value = this.Value;
            while (value == this.Value)
                this.Randomize();
        }

        public object Clone()
        {
            var clone = new CharGene();
            clone.Value = this.Value;

            return clone;
        }
    }
}
