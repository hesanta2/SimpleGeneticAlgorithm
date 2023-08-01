using hesanta.AI.GeneticAlgorithm.Gene;

namespace hesanta.AI.GeneticAlgorithm.Chromosome
{
    public interface IFitnessChromosome<T>
        where T : IGene
    {
        IChromosome<T> Chromosome { get; }
        decimal Fitness { get; }
    }
}