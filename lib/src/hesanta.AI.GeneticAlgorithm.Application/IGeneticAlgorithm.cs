using hesanta.AI.GA.Domain;
using System;
using System.Collections.Generic;

namespace hesanta.AI.GA.Application
{
    public interface IGeneticAlgorithm<T>
        where T : IGene
    {
        event EventHandler<IChromosome<T>> OnInitializePopulationChromosome;
        event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationSelectParents;
        event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationParentsRecombination;
        event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationCreateChildren;

        ICollection<IChromosome<T>> Chromosomes { get; }
        IFitnessChromosome<T> BestChromosome { get; }
        int PopulationNumber { get; }
        bool ThereIsSolution { get; }
        int CurrentIteration { get; }
        ICollection<IFitnessChromosome<T>> FitnessChromosomes { get; }
        int GensPerChromosome { get; }

        void InitializePopulation();
        void EvaluateFitness();
        void GetNextPopulation();
    }
}