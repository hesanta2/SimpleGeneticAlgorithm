using hesanta.AI.GA.Application;
using hesanta.AI.GA.Domain;
using System;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            IFitnessFunction<CharGene> fitness = new GeneCharFitnessFunction();
            IGeneticAlgorithm<CharGene> algorithm = new GeneticAlgorithm<CharGene>(300, GeneCharFitnessFunction.FitnesSentence.Length, fitness);
            IGeneticAlgorithmService<CharGene> service = new GeneticAlgorithmService<CharGene>(algorithm);

            var console = new ConsoleVisualizacionService<CharGene>(service);
            console.InitializeConsole();

            service.GetIterateSolution(maxIterations: 1000);

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.ReadKey();
        }

        private class GeneCharFitnessFunction : IFitnessFunction<CharGene>
        {
            public static string FitnesSentence = "There is a example for calculate a multiple chars that are conforming a sentencense using a super GENETIC ALGORITHM!!!";
            public decimal GetFitness(IChromosome<CharGene> chromosome)
            {
                decimal fitness = 0;
                int genesNumber = chromosome.Genes.Count;

                for (int i = 0; i < FitnesSentence.Length; i++)
                {
                    var character = FitnesSentence[i];

                    if (chromosome.Genes[i].Value == character.ToString())
                        fitness += (decimal)1 / (decimal)genesNumber;
                }

                return fitness;
            }
        }
    }
}
