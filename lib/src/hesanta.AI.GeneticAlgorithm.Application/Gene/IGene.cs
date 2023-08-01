namespace hesanta.AI.GeneticAlgorithm.Gene
{

    public interface IGene<T> : IGene
    {
        T Value { get; }
    }

    public interface IGene : ICloneable
    {
        object RawValue { get; }
        double MutationRate { get; set; }
        double MutationAmount { get; set; }

        bool Equals(object obj);
        void Randomize();
        void Mutate();
        int GetHashCode();
    }
}