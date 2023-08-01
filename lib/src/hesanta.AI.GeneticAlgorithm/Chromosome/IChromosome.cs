using hesanta.AI.GeneticAlgorithm.Gene;

namespace hesanta.AI.GeneticAlgorithm.Chromosome
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