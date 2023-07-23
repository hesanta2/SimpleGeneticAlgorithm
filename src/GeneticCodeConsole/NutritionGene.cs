using hesanta.AI.GA.Domain;
using System;

namespace GeneticCodeConsole
{
    public class NutritionGene : Gene<double>
    {
        private static readonly Random random = new Random((int)DateTime.Today.Ticks);
        private double factor;

        public double Factor
        {
            get => factor;
            private set
            {
                factor = value;
                RawValue = factor;
            }
        }

        public int NutritionValue { get; internal set; }

        public override object Clone()
        {
            return new NutritionGene { Factor = Factor, NutritionValue = NutritionValue };
        }

        public override void Mutate()
        {
            Factor = RandomNumberBetween(0, 1);
            //NutritionValue = random.Next(500);
        }

        public override void Randomize()
        {
            Factor = RandomNumberBetween(0, 1);
            NutritionValue = random.Next(100);
        }

        public override bool Equals(object obj)
        {
            return Factor == ((NutritionGene)obj).Factor && Value == ((NutritionGene)obj).Value;
        }

        public override int GetHashCode()
        {
            return Factor.GetHashCode() + Value.GetHashCode();
        }

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }

        public override string ToString()
        {
            return $"{NutritionValue}, {Factor}";
        }
    }
}
