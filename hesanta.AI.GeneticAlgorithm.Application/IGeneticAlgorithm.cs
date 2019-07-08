using System;
using System.Collections.Generic;
using hesanta.AI.GA.Domain;

namespace hesanta.AI.GA.Application
{
    public interface IGeneticAlgorithm<TGene>
        where TGene : IGene
    {
        event EventHandler<IChromosome<TGene>> OnInitializePopulationChromosome;
        event EventHandler<(IChromosome<TGene>, IChromosome<TGene>)> OnNextPopulationSelectParents;
        event EventHandler<(IChromosome<TGene>, IChromosome<TGene>)> OnNextPopulationParentsRecombination;
        event EventHandler<(IChromosome<TGene>, IChromosome<TGene>)> OnNextPopulationCreateChildren;

        ICollection<IChromosome<TGene>> Chromosomes { get; }
        IFitnessChromosome<TGene> BestChromosome { get; }
        int PopulationNumber { get; }
        bool ThereIsSolution { get; }
        int CurrentIteration { get; }
        ICollection<IFitnessChromosome<TGene>> FitnessChromosomes { get; }
        int GensPerChromosome { get; }

        void InitializePopulation();
        void EvaluateFitness();
        void GetNextPopulation();
    }
}