namespace hesanta.AI.GA.Domain
{
    public interface IChromosome<T> : ICloneable
        where T : IGene
    {
        List<T> Genes { get; }
        double MutationRate { get; set; }
        double MutationAmount { get; set; }

        void Crossover(IChromosome<T> chromosomeToRecombine);

        void Randomize();
        string ToString();
        IChromosome<T> Mutate();
        bool Equals(object obj);
        int GetHashCode();
    }
}