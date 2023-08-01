using hesanta.AI.GeneticAlgorithm.Chromosome;
using hesanta.AI.GeneticAlgorithm.Gene;

namespace hesanta.AI.GeneticAlgorithm
{
    public interface IGeneticAlgorithmSolver<T>
        where T : IGene
    {
        event EventHandler OnAlgorithmStartEvent;
        event EventHandler<IFitnessChromosome<T>> OnAlgorithmCompleteEvent;
        event EventHandler<int> OnIterationProcessEvent;
        event EventHandler OnIterationStartEvent;

        IGeneticAlgorithm<T> GeneticAlgorithm { get; }

        int MaxIterations { get; }

        Task<IFitnessChromosome<T>> FindSolutionAsync(int maxIterations = 100);
    }
}