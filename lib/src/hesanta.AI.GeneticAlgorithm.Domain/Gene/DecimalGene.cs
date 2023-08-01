namespace hesanta.AI.GA.Domain.Gene
{
    public class DecimalGene : Gene<decimal>
    {
        private static Random random = new Random((int)DateTime.Today.Ticks);
        private double maxRandomValue = 1;

        public DecimalGene(decimal initialValue = 0.1M)
        {
            Value = initialValue;
        }

        public override void Randomize()
        {
            Value = RandomNumberBetween(0, maxRandomValue);
        }

        public override void Mutate()
        {
            decimal randomValue = 0.1M;
            while (randomValue == Value)
            {
                randomValue = RandomNumberBetween(0, maxRandomValue);
            }
            Value = randomValue;
        }

        public override object Clone()
        {
            return new DecimalGene(Value);
        }

        private static decimal RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return (decimal)(minValue + next * (maxValue - minValue));
        }
    }
}
