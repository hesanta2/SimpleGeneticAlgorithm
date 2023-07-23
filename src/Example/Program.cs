using hesanta.AI.GA.Application;
using hesanta.AI.GA.Domain;
using System;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            var fitnesSentence = "There is a example to calculate a multiple chars conforming a sentencense using a super GENETIC ALGORITHM!!!";
            IGeneticAlgorithm<CharGene> algorithm = new GeneticAlgorithm<CharGene>(300, fitnesSentence.Length, (chromosome) =>
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
            });
            IGeneticAlgorithmService<CharGene> service = new GeneticAlgorithmService<CharGene>(algorithm);

            var console = new ConsoleVisualizacionService<CharGene>(service);
            console.InitializeConsole();

            service.GetIterateSolution(maxIterations: 1000);

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.ReadKey();
        }

    }
}
