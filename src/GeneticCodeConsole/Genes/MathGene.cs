using hesanta.AI.GA.Domain;
using System;

namespace GeneticCodeConsole
{
    public class MathGene : Gene<object>
    {
        private static readonly Random random = new Random((int)DateTime.Today.Ticks);

        public enum MathType { Add = 0, Sub = 1, Mult = 2, Div = 3 }
        public MathType Type { get; private set; }

        public decimal MathValue { get; internal set; }

        public override object Clone()
        {
            return new MathGene { Type = Type, MathValue = MathValue };
        }

        public override void Mutate()
        {
            Type = (MathType)random.Next(4);
            MathValue = random.Next(500);
        }

        public override void Randomize()
        {
            Type = (MathType)random.Next(4);
            MathValue = random.Next(500);
        }

        public override bool Equals(object obj)
        {
            return Type == ((MathGene)obj).Type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode() + Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Enum.GetName(Type.GetType(), Type)}({MathValue})";
        }

        public decimal Operate(decimal result)
        {
            switch (Type)
            {
                case MathType.Add:
                    return result += MathValue;
                case MathType.Sub:
                    return result -= MathValue;
                case MathType.Mult:
                    return result *= MathValue;
                case MathType.Div:
                    return (long)(MathValue == 0 ? result /= 0.1M : result /= MathValue);
                default: return result;
            }
        }
    }
}
