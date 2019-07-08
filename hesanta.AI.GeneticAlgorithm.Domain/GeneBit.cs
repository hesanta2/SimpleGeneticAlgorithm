using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace hesanta.AI.GA.Domain
{
    public class GeneBit : IGene
    {
        private static Random random = new Random((int)DateTime.Today.Ticks);

        public string Value { get; protected set; } = "0";

        public GeneBit() { }

        public void Randomize()
        {
            this.Value = random.Next(2).ToString();
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
            this.Value = this.Value == "0" ? "1" : "0";
        }

        public object Clone()
        {
            var clone = new GeneBit();
            clone.Value = this.Value;

            return clone;
        }
    }
}
