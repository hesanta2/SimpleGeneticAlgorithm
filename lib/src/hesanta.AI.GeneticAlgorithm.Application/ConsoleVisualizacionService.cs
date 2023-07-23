using hesanta.AI.GA.Domain;
using System;
using System.Diagnostics;
using System.Drawing;
using TrueColorConsole;

namespace hesanta.AI.GA.Application
{
    public class ConsoleVisualizacionService<T>
        where T : IGene
    {
        private readonly IGeneticAlgorithmService<T> service;
        private readonly Stopwatch stopWatch = new Stopwatch();
        private Point setupBoundingBox;
        private Point iterationBoundingBox;
        private Point solutionBoundingBox;

        public ConsoleVisualizacionService(IGeneticAlgorithmService<T> geneticAlgorithmService)
        {
            service = geneticAlgorithmService;
            VTConsole.Enable();
        }

        public void InitializeConsole()
        {
            Console.CursorVisible = false;
            var setupTemplate = SetupTemplate();
            setupBoundingBox = setupTemplate.Item2;
            VTConsole.Write(setupTemplate.Item1, Color.WhiteSmoke);
            iterationBoundingBox = new Point(4, setupBoundingBox.Y + 5);
            solutionBoundingBox = new Point(0, iterationBoundingBox.Y + 5);

            WriteSetup();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            service.OnStart += (s, e) =>
            {
                stopWatch.Start();
                WriteSetup();
            };

            service.OnStartIterations += (s, e) =>
            {
                WriteIteration(service.GeneticAlgorithm.CurrentIteration);
            };

            service.OnIterate += (s, currentIteration) =>
            {
                //if (stopWatch.ElapsedMilliseconds % 1000 < 700 && !service.ThereIsSolution) return;

                WriteIteration(service.GeneticAlgorithm.CurrentIteration);
            };

            service.OnFinish += (s, solution) =>
            {
                WriteIteration(service.GeneticAlgorithm.CurrentIteration);
                stopWatch.Stop();
            };
        }

        private void WriteSetup()
        {
            Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y);
            Console.Write($"{service.MaxIterations}");
            Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 1);
            Console.Write($"{service.GeneticAlgorithm.PopulationNumber}");
            Console.SetCursorPosition(setupBoundingBox.X, setupBoundingBox.Y + 2);
            Console.Write($"{service.GeneticAlgorithm.GensPerChromosome}");
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
            Console.Write($"Exact solution found    [");
            Color color = service.GeneticAlgorithm.ThereIsSolution ? Color.FromArgb(84, 255, 0) : Color.Red;
            VTConsole.Write($"{service.GeneticAlgorithm.ThereIsSolution}", color);
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

            Color color = ConsoleColorRtoG(service.GeneticAlgorithm.BestChromosome.Fitness);
            toWrite = $"[{Math.Round(service.GeneticAlgorithm.BestChromosome.Fitness, 4)}]";
            centerString = new string(' ', Console.WindowWidth / 2 - toWrite.ToString().Length / 2);
            VTConsole.Write($"{centerString}{toWrite}{centerString}", color);

            toWrite = $"[{service.GeneticAlgorithm.BestChromosome.Chromosome}]";
            if (toWrite.ToString().Length < Console.WindowWidth)
                centerString = new string(' ', Console.WindowWidth / 2 - toWrite.ToString().Length / 2);
            else
                centerString = "";
            VTConsole.Write($"{centerString}{toWrite}{centerString}", Color.WhiteSmoke);
        }

        private (string, Point) SetupTemplate()
        {
            return ($@"
    ╔═══════════════════════════╗
    ║           SETUP           ║
    ╠═══════════════════════════╣
    ║ Max iterations:           ║
    ║ Population    :           ║
    ║ Genes         :           ║
    ╚═══════════════════════════╝
", new Point(23, 4));
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
