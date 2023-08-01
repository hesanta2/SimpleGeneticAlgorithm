using hesanta.AI.GA.Domain.Gene;

namespace hesanta.AI.GA.Domain.Chromosome
{
    public interface IFitnessChromosome<T>
        where T : IGene
    {
        IChromosome<T> Chromosome { get; }
        decimal Fitness { get; }
    }
}