using GeneticCodeConsole.NeuralNetworkWeightsGenes;
using hesanta.AI.GA.Application;
using hesanta.AI.GA.Domain;
using hesanta.AI.NeuralNetwork.ActivationFunctions;
using hesanta.AI.NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticCodeConsole.NeuronGenes
{
    public class Runner
    {
        private static double NormalizationFunc(double x) => x / 20;
        private static double DenormalizationFunc(double x) => Math.Round(x * 20);
        private static NeuralNetwork neuralNetwork = new(2, new[] { 2 }, 1, new ReLUActivationFunction(), normalizationFunc: NormalizationFunc);

        public static void Run()
        {
            var genes = NeuronDataGenesConverter.CreateGenesFromNetworkData(neuralNetwork.GetData());
            IGeneticAlgorithm<NeuronGene> algorithm = new GeneticAlgorithm<NeuronGene>(10, genes.Count, EvalFitness, staleGenerationThreshold: 20, createChromosomeInstanceFunc: (index) =>
            {
                return genes[index];
            });

            IGeneticAlgorithmProcessor<NeuronGene> processor = new GeneticAlgorithmProcessor<NeuronGene>(algorithm);

            var console = new ConsoleVisualizacionService<NeuronGene>(processor);
            console.InitializeConsole();
            processor.ComputeIterativeSolutionAsync(maxIterations: 10000).Wait();

            Console.CursorVisible = true;
            Console.SetCursorPosition(0, Console.WindowHeight - 4);

            var neuralNetworkData = NeuronDataGenesConverter.CreateNetworkDataFromGenes(algorithm.BestChromosome.Chromosome.Genes, 2, new[] { 2 }, 1);
            neuralNetwork.SetData(neuralNetworkData);

            //for (int i = 0; i <= 99; i++)
            //{
            //    for (int j = 0; j <= 99; j++)
            //    {
            //        var input = new List<double> { i, j };
            //        neuralNetwork.Predict(input);
            //        var denormalizedOutput = DenormalizationFunc(neuralNetwork.OutputLayer[0].Output);
            //        Console.WriteLine($"Input: {i},{j} -> Output: {denormalizedOutput} - {(i + j == denormalizedOutput ? "OK" : "ERROR")}");
            //    }
            //}

            // Test the neural network with user's inputs
            string userInput;
            while (true)
            {
                Console.WriteLine("Enter two numbers (comma-separated) to test the neural network, or 'q' to quit:");
                userInput = Console.ReadLine();
                if (userInput == "q")
                {
                    break;
                }

                string[] inputs = userInput.Split(',');
                if (inputs.Length != 2)
                {
                    Console.WriteLine("Invalid input. Please provide two numbers separated by a comma.");
                    continue;
                }

                double num1, num2;
                if (!double.TryParse(inputs[0], out num1) || !double.TryParse(inputs[1], out num2))
                {
                    Console.WriteLine("Invalid input. Please provide two numbers.");
                    continue;
                }

                var input = new List<double> { num1, num2 };
                neuralNetwork.Predict(input);
                var denormalizedOutput = DenormalizationFunc(neuralNetwork.OutputLayer[0].Output);
                Console.WriteLine($"Input: {num1},{num2} -> Output: {denormalizedOutput} - {(num1 + num2 == denormalizedOutput ? "OK" : "ERROR")}");
            }

            Console.ReadKey();
        }

        private static decimal EvalFitness(IChromosome<NeuronGene> chromosome)
        {
            // Define tus casos de prueba
            var testCases = new List<Tuple<List<double>, double>>();
            for (int i = 0; i <= 99; i++)
            {
                for (int j = 0; j <= 99; j++)
                {
                    // La función objetivo es la suma de los dos números
                    double expectedResult = i + j;
                    testCases.Add(new Tuple<List<double>, double>(new List<double>() { i, j }, expectedResult));
                }
            }

            decimal totalFitness = 0;
            int numTestCases = testCases.Count;  // número de casos de prueba

            var neuralNetworkData = NeuronDataGenesConverter.CreateNetworkDataFromGenes(chromosome.Genes, 2, new[] { 2 }, 1);
            neuralNetwork.SetData(neuralNetworkData);

            decimal totalError = 0;

            foreach (var testCase in testCases)
            {
                neuralNetwork.Predict(testCase.Item1);
                var output = Math.Round(DenormalizationFunc(neuralNetwork.OutputLayer.First().Output)); // Redondeamos la salida
                totalError += (decimal)Math.Abs(output - testCase.Item2);
            }

            // Normalizamos el error total dividiendo por el número de casos de prueba
            decimal normalizedError = totalError / numTestCases;

            // Suponemos que una red con un error total más bajo es mejor.
            // Podríamos usar cualquier función que disminuya con el error total para calcular la aptitud.
            // Aquí, simplemente invertimos el error total para obtener la aptitud.
            // Nota: Sumamos 1 al error total para evitar dividir por cero.
            var fitness = 1 / (1 + normalizedError);

            return fitness;
        }
    }
}
