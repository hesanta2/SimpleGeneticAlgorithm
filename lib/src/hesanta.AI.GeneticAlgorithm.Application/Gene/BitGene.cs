namespace hesanta.AI.GeneticAlgorithm.Gene
{
    public class BitGene : Gene<bool>
    {
        private static readonly Random random = new Random((int)DateTime.Today.Ticks);

        public BitGene(bool initialValue = false)
        {
            Value = initialValue;
        }

        public override void Randomize()
        {
            Value = random.Next(2) == 1;
        }

        public override void Mutate()
        {
            Value = !Value;
        }

        public override object Clone()
        {
            return new BitGene(Value);
        }
    }
}
