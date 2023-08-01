using hesanta.AI.GeneticAlgorithm;
using hesanta.AI.GeneticAlgorithm.Chromosome;
using hesanta.AI.GeneticAlgorithm.Console;
using hesanta.AI.GeneticAlgorithm.Gene;

namespace Example
{
    class Program
    {
        private static readonly string fitnesSentence = "This is an example to calculate multiple characters that make up a sentence using a GENETIC ALGORITHM!!!";

        static void Main()
        {
            IGeneticAlgorithm<CharGene> algorithm = new GeneticAlgorithm<CharGene>(300, fitnesSentence.Length, EvalFitness, staleGenerationThreshold: 10);
            IGeneticAlgorithmSolver<CharGene> processor = new GeneticAlgorithmSolver<CharGene>(algorithm);

            var console = new ConsoleVisualizacionService<CharGene>(processor, (chromosome) => $"[{string.Join("", chromosome.Genes)}]");
            console.InitializeConsole();

            processor.FindSolutionAsync(maxIterations: 1000);

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.ReadKey();
        }

        private static decimal EvalFitness(IChromosome<CharGene> chromosome)
        {
            decimal fitness = 0;
            int genesNumber = chromosome.Genes.Count;

            for (int i = 0; i < fitnesSentence.Length; i++)
            {
                var character = fitnesSentence[i];

                if (chromosome.Genes[i].Value == character)
                {
                    fitness += (decimal)1 / genesNumber;
                }
            }

            return fitness;
        }

    }
}
