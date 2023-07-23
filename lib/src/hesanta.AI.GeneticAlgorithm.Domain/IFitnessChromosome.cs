namespace hesanta.AI.GA.Domain
{
    public interface IFitnessChromosome<T>
        where T : IGene
    {
        IChromosome<T> Chromosome { get; }
        decimal Fitness { get; }
    }
}