using hesanta.AI.GeneticAlgorithm.Chromosome;
using hesanta.AI.GeneticAlgorithm.Gene;

namespace hesanta.AI.GeneticAlgorithm
{
    public class GeneticAlgorithmSolver<T> : IGeneticAlgorithmSolver<T>
        where T : IGene
    {
        public int MaxIterations { get; private set; }
        public event EventHandler OnAlgorithmStartEvent;
        public event EventHandler<IFitnessChromosome<T>> OnAlgorithmCompleteEvent;
        public event EventHandler<int> OnIterationProcessEvent;
        public event EventHandler OnIterationStartEvent;
        public IGeneticAlgorithm<T> GeneticAlgorithm { get; }

        public GeneticAlgorithmSolver(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            GeneticAlgorithm = geneticAlgorithm;
        }

        public async Task<IFitnessChromosome<T>> FindSolutionAsync(int maxIterations = 100)
        {
            MaxIterations = maxIterations;
            OnAlgorithmStartEvent?.Invoke(this, EventArgs.Empty);
            GeneticAlgorithm.CreateInitialPopulation();
            GeneticAlgorithm.EvaluateChromosomeFitness();
            OnIterationStartEvent?.Invoke(this, EventArgs.Empty);

            while (!GeneticAlgorithm.SolutionFound && GeneticAlgorithm.CurrentIteration < maxIterations)
            {
                await Task.Run(() =>
                {
                    GeneticAlgorithm.CreateNextGeneration();
                    GeneticAlgorithm.EvaluateChromosomeFitness();
                });

                OnIterationProcessEvent?.Invoke(this, GeneticAlgorithm.CurrentIteration);
            }

            OnAlgorithmCompleteEvent?.Invoke(this, GeneticAlgorithm.BestChromosome);
            return GeneticAlgorithm.BestChromosome;
        }
    }
}
