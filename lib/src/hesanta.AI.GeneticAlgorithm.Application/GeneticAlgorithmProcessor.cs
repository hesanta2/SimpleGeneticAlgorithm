using hesanta.AI.GA.Domain;
using System;

namespace hesanta.AI.GA.Application
{
    public class GeneticAlgorithmProcessor<T> : IGeneticAlgorithmProcessor<T>
        where T : IGene
    {
        public int MaxIterations { get; private set; }
        public event EventHandler OnStart;
        public event EventHandler<IFitnessChromosome<T>> OnFinish;
        public event EventHandler<int> OnIterate;
        public event EventHandler OnStartIterations;

        public IGeneticAlgorithm<T> GeneticAlgorithm { get; }
        public bool ThereIsSolution { get; private set; }

        public GeneticAlgorithmProcessor(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            GeneticAlgorithm = geneticAlgorithm;
        }

        public IFitnessChromosome<T> GetIterateSolution(int maxIterations = 100)
        {
            MaxIterations = maxIterations;

            OnStart?.Invoke(this, EventArgs.Empty);

            GeneticAlgorithm.InitializePopulation();

            GeneticAlgorithm.EvaluateFitness();

            OnStartIterations?.Invoke(this, EventArgs.Empty);

            while (!GeneticAlgorithm.ThereIsSolution && GeneticAlgorithm.CurrentIteration < maxIterations)
            {
                GeneticAlgorithm.GetNextPopulation();
                GeneticAlgorithm.EvaluateFitness();

                OnIterate?.Invoke(this, GeneticAlgorithm.CurrentIteration);
            }

            OnFinish?.Invoke(this, GeneticAlgorithm.BestChromosome);

            return GeneticAlgorithm.BestChromosome;
        }
    }
}
