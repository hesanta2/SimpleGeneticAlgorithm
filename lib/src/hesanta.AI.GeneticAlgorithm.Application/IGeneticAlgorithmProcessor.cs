using hesanta.AI.GA.Domain;
using System;

namespace hesanta.AI.GA.Application
{
    public interface IGeneticAlgorithmProcessor<T>
        where T : IGene
    {
        event EventHandler OnAlgorithmStart;
        event EventHandler<IFitnessChromosome<T>> OnAlgorithmComplete;
        event EventHandler<int> OnIterationProcess;
        event EventHandler OnIterationStart;

        IGeneticAlgorithm<T> AlgorithmInstance { get; }

        int MaximumIterations { get; }

        Task<IFitnessChromosome<T>> ComputeIterativeSolutionAsync(int maxIterations = 100);
    }
}