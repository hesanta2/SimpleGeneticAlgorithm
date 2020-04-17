using hesanta.AI.GA.Domain;
using System;

namespace hesanta.AI.GA.Application
{
    public interface IGeneticAlgorithmService<TGene>
        where TGene : IGene
    {
        event EventHandler OnStart;
        event EventHandler<IFitnessChromosome<TGene>> OnFinish;
        event EventHandler<int> OnIterate;
        event EventHandler OnStartIterations;

        IGeneticAlgorithm<TGene> GeneticAlgorithm { get; }

        int MaxIterations { get; }

        IFitnessChromosome<TGene> GetIterateSolution(int maxIterations = 100);
    }
}