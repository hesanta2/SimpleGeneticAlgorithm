using hesanta.AI.GA.Application;
using hesanta.AI.GA.Domain;
using System;
using System.Collections.Generic;

namespace GeneticCodeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IGeneticAlgorithm<NutritionGene> algorithm = new GeneticAlgorithm<NutritionGene>(100, 4, GetFitness, error: 0.00000000001M);
            IGeneticAlgorithmService<NutritionGene> service = new GeneticAlgorithmService<NutritionGene>(algorithm);
            GetSolution(service);

            /*var results = new List<IFitnessChromosome<NutritionGene>>();
            var iterations = 100;
            for (int i = 0; i < iterations; i++)
            {
                IFitnessFunction<NutritionGene> fi = new GeneCharFitnessFunction();
                IGeneticAlgorithm<NutritionGene> a = new GeneticAlgorithm<NutritionGene>(100, GeneCharFitnessFunction.TotalGens, fi, error: 0.00000000001M);
                IGeneticAlgorithmService<NutritionGene> s = new GeneticAlgorithmService<NutritionGene>(a);
                var result = GetSolution(s);
                results.Add(result);
            }

            double c = 0, g = 0, p = 0, f = 0;
            results.ForEach(r =>
            {
                c += r.Chromosome.Genes[0].Factor;
                g += r.Chromosome.Genes[1].Factor;
                p += r.Chromosome.Genes[2].Factor;
                f += r.Chromosome.Genes[3].Factor;
            });

            Console.WriteLine(c / iterations);
            Console.WriteLine(g / iterations);
            Console.WriteLine(p / iterations);
            Console.WriteLine(f / iterations);*/

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.ReadKey();
        }

        private static IFitnessChromosome<NutritionGene> GetSolution(IGeneticAlgorithmService<NutritionGene> service)
        {
            var console = new ConsoleVisualizacionService<NutritionGene>(service);
            console.InitializeConsole();

            service.GetIterateSolution(maxIterations: 10000);

            return service.GeneticAlgorithm.BestChromosome;
        }

        public static int TotalGens => 4;

        private int fitnessResult1 = 16;
        private int fitnessResult2 = 24;

        public static decimal GetFitness(IChromosome<NutritionGene> chromosome)
        {
            decimal fitness = 0;
            var geneCarbohidrates = chromosome.Genes[0];
            var geneFats = chromosome.Genes[1];
            var geneProteins = chromosome.Genes[2];
            var geneFiber = chromosome.Genes[3];

            var fitnessList = new List<decimal>{
                 GetPartialFitness(geneCarbohidrates, geneFats, geneProteins, geneFiber, 0, 92, 0, 0, 24),
                 GetPartialFitness(geneCarbohidrates, geneFats, geneProteins, geneFiber, 29.0, 42.0, 19.0, 11.5, 16),
                 GetPartialFitness(geneCarbohidrates, geneFats, geneProteins, geneFiber, 3, 3, 16, 0, 3),
                 GetPartialFitness(geneCarbohidrates, geneFats, geneProteins, geneFiber, 78, 3.2, 8.6, 0, 10),
                 GetPartialFitness(geneCarbohidrates, geneFats, geneProteins, geneFiber, 57, 14, 14, 14, 12),
                 GetPartialFitness(geneCarbohidrates, geneFats, geneProteins, geneFiber, 72, 1.3, 2.3, 7.5, 9),
                 GetPartialFitness(geneCarbohidrates, geneFats, geneProteins, geneFiber, 60.1,7.6, 13, 24.7, 11)
                };

            fitnessList.ForEach(f => fitness += f);

            return fitness / fitnessList.Count;
        }

        private static decimal GetPartialFitness(NutritionGene geneCarbohidrates, NutritionGene geneFats, NutritionGene geneProteins, NutritionGene geneFiber, double carbohidrates, double fats, double proteins, double fiber, int fitnessResult)
        {
            decimal fitness;
            double result = (geneCarbohidrates.Factor * carbohidrates) + (geneFats.Factor * fats) + (geneProteins.Factor * proteins) + (geneFiber.Factor * fiber);

            if (result == 0) { return 0; }

            var resultRounded = Math.Round(result);

            if (resultRounded > fitnessResult)
            {
                fitness = (decimal)(fitnessResult) / (decimal)(resultRounded);
            }
            else
            {
                fitness = (decimal)(resultRounded) / (decimal)(fitnessResult);
            }

            if (fitness > 1)
            {
                fitness = 1 - (fitness - 1);
            }

            return fitness;
        }

        public double BipolarSigmoid(double x)
        {
            return (1 - Math.Exp(-x)) / (1 + Math.Exp(-x));
        }

    }
}
