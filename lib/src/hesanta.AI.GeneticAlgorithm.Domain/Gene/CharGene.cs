namespace hesanta.AI.GA.Domain.Gene
{
    public class CharGene : Gene<char>
    {
        private static Random random = new Random((int)DateTime.Today.Ticks);


        public CharGene()
        {
            Value = ' ';
        }
        public CharGene(char value)
        {
            Value = value;
        }

        public override void Randomize()
        {
            Value = (char)random.Next(32, 255);
        }

        public override void Mutate()
        {
            var value = Value;
            while (value == Value)
            {
                Randomize();
            }
        }

        public override object Clone()
        {
            return new CharGene(Value);
        }
    }
}
