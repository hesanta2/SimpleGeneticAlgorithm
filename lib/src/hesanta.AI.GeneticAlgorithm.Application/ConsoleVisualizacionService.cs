using hesanta.AI.GA.Domain;
using System.Diagnostics;
using System.Drawing;
using TrueColorConsole;

namespace hesanta.AI.GA.Application
{
    public class ConsoleVisualizacionService<T>
        where T : IGene
    {
        private readonly IGeneticAlgorithmProcessor<T> service;
        private readonly Func<IChromosome<T>, string> chromosomeToString;
        private readonly Stopwatch stopWatch = new Stopwatch();
        private Point setupBoundingBox;
        private Point iterationBoundingBox;
        private Point solutionBoundingBox;

        public ConsoleVisualizacionService(IGeneticAlgorithmProcessor<T> geneticAlgorithmService, Func<IChromosome<T>, string> chromosomeToString = null)
        {
            service = geneticAlgorithmService;
            this.chromosomeToString = chromosomeToString;
            VTConsole.Enable();
        }

        public void InitializeConsole()
        {
            Console.CursorVisible = false;
            var setupTemplate = SetupTemplate();
            setupBoundingBox = setupTemplate.Item2;
            VTConsole.Write(setupTemplate.Item1, Color.WhiteSmoke);
            iterationBoundingBox = new Point(4, setupBoundingBox.Y + 9);
            solutionBoundingBox = new Point(0, iterationBoundingBox.Y + 5);

            WriteSetup();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            service.OnAlgorithmStart += (s, e) =>
            {
                stopWatch.Start();
                WriteSetup();
            };

            service.OnIterationStart += (s, e) =>
            {
                WriteIteration(service.AlgorithmInstance.CurrentIteration);
            };

            service.OnIterationProcess += (s, currentIteration) =>
            {
                //if (stopWatch.ElapsedMilliseconds % 1000 < 700 && !service.ThereIsSolution) return;

                WriteIteration(service.AlgorithmInstance.CurrentIteration);
            };

            service.OnAlgorithmComplete += (s, solution) =>
            {
                WriteIteration(service.AlgorithmInstance.CurrentIteration);
                stopWatch.Stop();
            };
        }

        private void WriteSetup()
        {
            Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y);
            Console.Write($"{service.MaximumIterations}");
            Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 1);
            Console.Write($"{service.AlgorithmInstance.PopulationNumber}");
            Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 2);
            Console.Write($"{service.AlgorithmInstance.GensPerChromosome}");
            Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 3);
            Console.Write($"{service.AlgorithmInstance.ElitismRate}");
            Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 4);
            Console.Write($"{service.AlgorithmInstance.StaleGenerationThreshold}");
        }


        private void WriteIteration(int currentIteration)
        {
            int leftMargin = iterationBoundingBox.X;
            int topPosition = iterationBoundingBox.Y;

            Console.SetCursorPosition(leftMargin, topPosition++);
            Console.Write($"Elapsed time            [{(decimal)stopWatch.ElapsedMilliseconds / 1000}s]");

            Console.SetCursorPosition(leftMargin, topPosition++);
            Console.Write($"Current iteration       [{currentIteration}]");

            Console.SetCursorPosition(leftMargin, topPosition++);
            Console.Write($"Stale generations       [{service.AlgorithmInstance.StaleGenerations}] ");

            Console.SetCursorPosition(leftMargin, topPosition++);
            Console.Write($"Mutation rate           [{Math.Round(service.AlgorithmInstance.BestChromosome.Chromosome.MutationRate, 2)}]   ");

            Console.SetCursorPosition(leftMargin, topPosition++);
            Console.Write($"Exact solution found    [");
            Color color = service.AlgorithmInstance.ThereIsSolution ? Color.FromArgb(84, 255, 0) : Color.Red;
            VTConsole.Write($"{service.AlgorithmInstance.ThereIsSolution}", color);
            VTConsole.Write("]", Color.WhiteSmoke);

            WriteBestSolution();
        }

        private void WriteBestSolution()
        {
            int leftMargin = solutionBoundingBox.X;
            int topPosition = solutionBoundingBox.Y;

            Console.SetCursorPosition(leftMargin, topPosition++);

            string toWrite = "BEST SOLUTION";
            string centerString = new string(' ', Console.WindowWidth / 2 - toWrite.Length / 2);
            VTConsole.Write($"{centerString}BEST SOLUTION{centerString}", Color.WhiteSmoke);

            Color color = ConsoleColorRtoG(service.AlgorithmInstance.BestChromosome.Fitness);
            toWrite = $"[{Math.Round(service.AlgorithmInstance.BestChromosome.Fitness, 4)}]";
            WriteCentered(toWrite, color);

            var bestSolutionChromosome = service.AlgorithmInstance.BestChromosome.Chromosome;
            toWrite = $"{(chromosomeToString == null ? bestSolutionChromosome.ToString() : chromosomeToString(bestSolutionChromosome))}";
            WriteCentered("");
            WriteCentered(toWrite);
        }

        private void WriteCentered(string toWrite, Color? color = null)
        {
            string centerString;
            if (toWrite.ToString().Length < Console.WindowWidth)
            {
                centerString = new string(' ', Console.WindowWidth / 2 - toWrite.ToString().Length / 2);
            }
            else
            {
                centerString = "";
            }
            VTConsole.Write($"{centerString}{toWrite}{centerString}", color ?? Color.WhiteSmoke);
        }

        private (string, Point) SetupTemplate()
        {
            return ($@"
    ╔════════════════════════════════════════╗
    ║                   SETUP                ║
    ╠════════════════════════════════════════╣
    ║ Max iterations :                       ║
    ║ Population     :                       ║
    ║ Genes          :                       ║
    ║ Elitism rate   :                       ║
    ║ Stale threshold:                       ║
    ╚════════════════════════════════════════╝
", new Point(32, 4));
        }


        private Color ConsoleColorRtoG(decimal percentage)
        {
            var colorFrom = Color.Red;
            var colorTo = Color.FromArgb(84, 255, 0);

            var r = colorFrom.R + (int)((colorTo.R - colorFrom.R) * percentage);
            var g = colorFrom.G + (int)((colorTo.G - colorFrom.G) * percentage);
            var b = colorFrom.B + (int)((colorTo.B - colorFrom.B) * percentage);

            return Color.FromArgb(r, g, b);
        }

    }
}
