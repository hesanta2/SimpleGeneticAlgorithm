using hesanta.AI.GA.Application;
using hesanta.AI.GA.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace hesanta.AI.GA.SimpleGeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            IFitnessFunction<GeneChar> fitness = new GeneCharFitnessFunction();
            IGeneticAlgorithm<GeneChar> algorithm = new GeneticAlgorithm<GeneChar>(300, GeneCharFitnessFunction.FitnesSentence.Length, fitness);
            IGeneticAlgorithmService<GeneChar> service = new GeneticAlgorithmService<GeneChar>(algorithm);

            var console = new ConsoleVisualizacionService<GeneChar>(service);
            console.InitializeConsole();

            Console.ReadKey();

            service.GetIterateSolution(maxIterations: 1000);

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.ReadKey();
        }

        private class GeneCharFitnessFunction : IFitnessFunction<GeneChar>
        {
            public static string FitnesSentence = "There is a example for calculate a multiple chars that are conforming a sentencense using a super GENETIC ALGORITHM!!!";
            public decimal GetFitness(IChromosome<GeneChar> chromosome)
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
