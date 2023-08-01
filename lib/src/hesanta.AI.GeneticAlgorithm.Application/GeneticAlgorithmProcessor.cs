using hesanta.AI.GA.Domain;

namespace hesanta.AI.GA.Application
{
    public class GeneticAlgorithmProcessor<T> : IGeneticAlgorithmProcessor<T>
        where T : IGene
    {
        public int MaximumIterations { get; private set; }
        public event EventHandler OnAlgorithmStart;
        public event EventHandler<IFitnessChromosome<T>> OnAlgorithmComplete;
        public event EventHandler<int> OnIterationProcess;
        public event EventHandler OnIterationStart;

        public IGeneticAlgorithm<T> AlgorithmInstance { get; }
        public bool ThereIsSolution { get; private set; }

        public GeneticAlgorithmProcessor(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            AlgorithmInstance = geneticAlgorithm;
        }

        public async Task<IFitnessChromosome<T>> ComputeIterativeSolutionAsync(int maxIterations = 100)
        {
            MaximumIterations = maxIterations;

            OnAlgorithmStart?.Invoke(this, EventArgs.Empty);

            AlgorithmInstance.InitializePopulation();

            AlgorithmInstance.EvaluateFitness();

            OnIterationStart?.Invoke(this, EventArgs.Empty);

            while (!AlgorithmInstance.ThereIsSolution && AlgorithmInstance.CurrentIteration < maxIterations)
            {
                await Task.Run(() =>
                {
                    AlgorithmInstance.GetNextPopulation();
                    AlgorithmInstance.EvaluateFitness();
                });

                OnIterationProcess?.Invoke(this, AlgorithmInstance.CurrentIteration);
            }

            OnAlgorithmComplete?.Invoke(this, AlgorithmInstance.BestChromosome);

            return AlgorithmInstance.BestChromosome;
        }
    }
}
