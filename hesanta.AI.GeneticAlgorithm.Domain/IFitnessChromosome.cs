namespace hesanta.AI.GA.Domain
{
    public interface IFitnessChromosome<TGene>
        where TGene : IGene
    {
        IChromosome<TGene> Chromosome { get; }
        decimal Fitness { get; }
    }
}