using hesanta.AI.GA.Application;
using hesanta.AI.GA.Domain;
using System;

namespace GeneticCodeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IFitnessFunction<MathGene> fitness = new GeneCharFitnessFunction();
            IGeneticAlgorithm<MathGene> algorithm = new GeneticAlgorithm<MathGene>(300, GeneCharFitnessFunction.TotalGens, fitness);
            IGeneticAlgorithmService<MathGene> service = new GeneticAlgorithmService<MathGene>(algorithm);

            var console = new ConsoleVisualizacionService<MathGene>(service);
            console.InitializeConsole();

            service.GetIterateSolution(maxIterations: 10000);

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.ReadKey();
        }

        private class GeneCharFitnessFunction : IFitnessFunction<MathGene>
        {
            public static int TotalGens => 10;

            private decimal fitnessResult = 12826;

            public decimal GetFitness(IChromosome<MathGene> chromosome)
            {
                decimal fitness = 0;

                decimal result = 0;
                foreach (var gene in chromosome.Genes)
                {
                    result = gene.Operate(result);
                }

                var max = Math.Max(fitnessResult, result);
                var min = Math.Min(fitnessResult, result);

                if (max - min == 0) return 1;

                fitness = 1 / (max - min);

                return fitness;
            }
        }
    }
}
