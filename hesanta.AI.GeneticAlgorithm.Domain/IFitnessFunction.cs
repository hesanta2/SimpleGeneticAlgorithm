namespace hesanta.AI.GA.Domain
{
    public interface IFitnessFunction<TGene>
        where TGene : IGene
    {
        decimal GetFitness(IChromosome<TGene> chromosome);
    }
}