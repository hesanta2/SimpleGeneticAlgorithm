using hesanta.AI.GA.Domain;
using System;

namespace hesanta.AI.GA.Application
{
    public class GeneticAlgorithmService<TGene> : IGeneticAlgorithmService<TGene>
        where TGene : IGene
    {
        public int MaxIterations { get; private set; }
        public event EventHandler OnStart;
        public event EventHandler<IFitnessChromosome<TGene>> OnFinish;
        public event EventHandler<int> OnIterate;
        public event EventHandler OnStartIterations;

        public IGeneticAlgorithm<TGene> GeneticAlgorithm { get; }
        public bool ThereIsSolution { get; private set; }

        public GeneticAlgorithmService(IGeneticAlgorithm<TGene> geneticAlgorithm)
        {
            GeneticAlgorithm = geneticAlgorithm;
        }

        public IFitnessChromosome<TGene> GetIterateSolution(int maxIterations = 100)
        {
            this.MaxIterations = maxIterations;

            this.OnStart?.Invoke(this, EventArgs.Empty);

            this.GeneticAlgorithm.InitializePopulation();

            this.GeneticAlgorithm.EvaluateFitness();

            this.OnStartIterations?.Invoke(this, EventArgs.Empty);

            while (!this.GeneticAlgorithm.ThereIsSolution && this.GeneticAlgorithm.CurrentIteration < maxIterations)
            {
                this.GeneticAlgorithm.GetNextPopulation();
                this.GeneticAlgorithm.EvaluateFitness();

                this.OnIterate?.Invoke(this, this.GeneticAlgorithm.CurrentIteration);
            }

            this.OnFinish?.Invoke(this, this.GeneticAlgorithm.BestChromosome);

            return this.GeneticAlgorithm.BestChromosome;
        }
    }
}
