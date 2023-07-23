using hesanta.AI.GA.Application;
using hesanta.AI.GA.Domain;
using System;

namespace Sample
{
    class Program
    {
        private static readonly string fitnesSentence = "There is an example to calculate a multiple chars conforming a sentencense using a super GENETIC ALGORITHM!!!";

        static void Main()
        {
            IGeneticAlgorithm<CharGene> algorithm = new GeneticAlgorithm<CharGene>(300, fitnesSentence.Length, EvalFitness);
            IGeneticAlgorithmProcessor<CharGene> processor = new GeneticAlgorithmProcessor<CharGene>(algorithm);

            var console = new ConsoleVisualizacionService<CharGene>(processor, (chromosome) => $"[{string.Join("", chromosome.Genes)}]");
            console.InitializeConsole();

            processor.GetIterateSolution(maxIterations: 1000);

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
                    fitness += (decimal)1 / (decimal)genesNumber;
                }
            }

            return fitness;
        }

    }
}
