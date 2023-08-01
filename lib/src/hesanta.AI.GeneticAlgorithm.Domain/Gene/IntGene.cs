namespace hesanta.AI.GA.Domain.Gene
{
    public class IntGene : Gene<int>
    {
        private static Random random = new Random((int)DateTime.Today.Ticks);
        private int maxRandomValue = 100;

        public IntGene(int initialValue = 2)
        {
            Value = initialValue;
        }

        public override void Randomize()
        {
            Value = random.Next(2, maxRandomValue);
        }

        public override void Mutate()
        {
            int randomValue = 2;
            while (randomValue == Value)
                randomValue = random.Next(2, maxRandomValue);
            Value = randomValue;
        }

        public override object Clone()
        {
            return new IntGene(Value);
        }
    }
}
