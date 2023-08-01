using hesanta.AI.GeneticAlgorithm.Chromosome;
using hesanta.AI.GeneticAlgorithm.Gene;
using System.Diagnostics;
using System.Drawing;
using TrueColorConsole;

namespace hesanta.AI.GeneticAlgorithm.Console
{
    public class ConsoleVisualizacionService<T>
        where T : IGene
    {
        private readonly IGeneticAlgorithmSolver<T> service;
        private readonly Func<IChromosome<T>, string> chromosomeToString;
        private readonly Stopwatch stopWatch = new Stopwatch();
        private Point setupBoundingBox;
        private Point iterationBoundingBox;
        private Point solutionBoundingBox;

        public ConsoleVisualizacionService(IGeneticAlgorithmSolver<T> geneticAlgorithmService, Func<IChromosome<T>, string> chromosomeToString = null)
        {
            service = geneticAlgorithmService;
            this.chromosomeToString = chromosomeToString;
            VTConsole.Enable();
        }

        public void InitializeConsole()
        {
            System.Console.CursorVisible = false;
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
            service.OnAlgorithmStartEvent += (s, e) =>
            {
                stopWatch.Start();
                WriteSetup();
            };

            service.OnIterationStartEvent += (s, e) =>
            {
                WriteIteration(service.GeneticAlgorithm.CurrentIteration);
            };

            service.OnIterationProcessEvent += (s, currentIteration) =>
            {
                //if (stopWatch.ElapsedMilliseconds % 1000 < 700 && !service.ThereIsSolution) return;

                WriteIteration(service.GeneticAlgorithm.CurrentIteration);
            };

            service.OnAlgorithmCompleteEvent += (s, solution) =>
            {
                WriteIteration(service.GeneticAlgorithm.CurrentIteration);
                stopWatch.Stop();
            };
        }

        private void WriteSetup()
        {
            System.Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y);
            System.Console.Write($"{service.MaxIterations}");
            System.Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 1);
            System.Console.Write($"{service.GeneticAlgorithm.PopulationNumber}");
            System.Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 2);
            System.Console.Write($"{service.GeneticAlgorithm.GensPerChromosome}");
            System.Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 3);
            System.Console.Write($"{service.GeneticAlgorithm.ElitistSelectionRate}");
            System.Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 4);
            System.Console.Write($"{service.GeneticAlgorithm.NonImprovingGenerationThreshold}");
        }


        private void WriteIteration(int currentIteration)
        {
            int leftMargin = iterationBoundingBox.X;
            int topPosition = iterationBoundingBox.Y;

            System.Console.SetCursorPosition(leftMargin, topPosition++);
            System.Console.Write($"Elapsed time            [{(decimal)stopWatch.ElapsedMilliseconds / 1000}s]");

            System.Console.SetCursorPosition(leftMargin, topPosition++);
            System.Console.Write($"Current iteration       [{currentIteration}]");

            System.Console.SetCursorPosition(leftMargin, topPosition++);
            System.Console.Write($"Stale generations       [{service.GeneticAlgorithm.NonImprovingGenerationCount}] ");

            System.Console.SetCursorPosition(leftMargin, topPosition++);
            System.Console.Write($"Mutation rate           [{Math.Round(service.GeneticAlgorithm.BestChromosome.Chromosome.MutationRate, 2)}]   ");

            System.Console.SetCursorPosition(leftMargin, topPosition++);
            System.Console.Write($"Exact solution found    [");
            Color color = service.GeneticAlgorithm.SolutionFound ? Color.FromArgb(84, 255, 0) : Color.Red;
            VTConsole.Write($"{service.GeneticAlgorithm.SolutionFound}", color);
            VTConsole.Write("]", Color.WhiteSmoke);

            WriteBestSolution();
        }

        private void WriteBestSolution()
        {
            int leftMargin = solutionBoundingBox.X;
            int topPosition = solutionBoundingBox.Y;

            System.Console.SetCursorPosition(leftMargin, topPosition++);

            string toWrite = "BEST SOLUTION";
            string centerString = new string(' ', System.Console.WindowWidth / 2 - toWrite.Length / 2);
            VTConsole.Write($"{centerString}BEST SOLUTION{centerString}", Color.WhiteSmoke);

            Color color = ConsoleColorRtoG(service.GeneticAlgorithm.BestChromosome.Fitness);
            toWrite = $"[{Math.Round(service.GeneticAlgorithm.BestChromosome.Fitness, 4)}]";
            WriteCentered(toWrite, color);

            var bestSolutionChromosome = service.GeneticAlgorithm.BestChromosome.Chromosome;
            toWrite = $"{(chromosomeToString == null ? bestSolutionChromosome.ToString() : chromosomeToString(bestSolutionChromosome))}";
            WriteCentered("");
            WriteCentered(toWrite);
        }

        private static void WriteCentered(string toWrite, Color? color = null)
        {
            string centerString;
            if (toWrite.ToString().Length < System.Console.WindowWidth)
            {
                centerString = new string(' ', System.Console.WindowWidth / 2 - toWrite.ToString().Length / 2);
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


        private static Color ConsoleColorRtoG(decimal percentage)
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
