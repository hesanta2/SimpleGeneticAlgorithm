using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace hesanta.AI.GA.Domain
{
    public class IntGene : IGene
    {
        private static Random random = new Random((int)DateTime.Today.Ticks);
        private int maxRandomValue = 100;

        public string Value { get; protected set; } = "2";

        public IntGene() { }

        public void Randomize()
        {
            this.Value = random.Next(2, maxRandomValue).ToString();
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
            int randomValue = 2;
            while (randomValue.ToString() == this.Value)
                randomValue = random.Next(2, maxRandomValue);
            this.Value = randomValue.ToString();
        }

        public object Clone()
        {
            var clone = new IntGene();
            clone.Value = this.Value;

            return clone;
        }
    }
}
