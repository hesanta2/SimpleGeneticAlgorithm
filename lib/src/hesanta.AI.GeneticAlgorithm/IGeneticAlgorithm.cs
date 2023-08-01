using hesanta.AI.GeneticAlgorithm.Chromosome;
using hesanta.AI.GeneticAlgorithm.Gene;

namespace hesanta.AI.GeneticAlgorithm
{
    public interface IGeneticAlgorithm<T>
        where T : IGene
    {
        event EventHandler<IChromosome<T>> OnInitializePopulationChromosome;
        event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationSelectParents;
        event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationParentsRecombination;
        event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationCreateChildren;

        ICollection<IChromosome<T>> Chromosomes { get; }
        ICollection<IFitnessChromosome<T>> FitnessChromosomes { get; }
        IFitnessChromosome<T> BestChromosome { get; }
        int PopulationNumber { get; }
        int GensPerChromosome { get; }
        int CurrentIteration { get; }
        bool SolutionFound { get; }
        int NonImprovingGenerationThreshold { get; set; }
        int NonImprovingGenerationCount { get; }
        double ElitistSelectionRate { get; set; }

        void CreateInitialPopulation();
        void EvaluateChromosomeFitness();
        void CreateNextGeneration();
    }
}