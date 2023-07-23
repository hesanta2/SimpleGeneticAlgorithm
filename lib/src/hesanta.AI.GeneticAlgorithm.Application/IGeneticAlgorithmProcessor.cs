using hesanta.AI.GA.Domain;
using System;

namespace hesanta.AI.GA.Application
{
    public interface IGeneticAlgorithmProcessor<T>
        where T : IGene
    {
        event EventHandler OnStart;
        event EventHandler<IFitnessChromosome<T>> OnFinish;
        event EventHandler<int> OnIterate;
        event EventHandler OnStartIterations;

        IGeneticAlgorithm<T> GeneticAlgorithm { get; }

        int MaxIterations { get; }

        IFitnessChromosome<T> GetIterateSolution(int maxIterations = 100);
    }
}