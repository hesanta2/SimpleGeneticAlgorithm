namespace hesanta.AI.GA.Domain
{
    public interface IFitnessFunction<T>
        where T : IGene
    {
        decimal GetFitness(IChromosome<T> chromosome);
    }
}