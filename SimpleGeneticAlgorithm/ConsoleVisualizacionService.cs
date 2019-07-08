using hesanta.AI.GA.Application;
using hesanta.AI.GA.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace hesanta.AI.GA.SimpleGeneticAlgorithm
{
    public class ConsoleVisualizacionService<TGene> where TGene : IGene
    {
        private readonly IGeneticAlgorithmService<TGene> service;
        private readonly Stopwatch stopWatch = new Stopwatch();
        private Point setupBoundingBox;
        private Point iterationBoundingBox;
        private Point detailsBoundingBox;

        public ConsoleVisualizacionService(IGeneticAlgorithmService<TGene> geneticAlgorithmService)
        {
            this.service = geneticAlgorithmService;
        }

        public void InitializeConsole()
        {
            Console.CursorVisible = false;
            var setupTemplate = this.setupTemplate();
            setupBoundingBox = setupTemplate.Item2;
            Console.Write(setupTemplate.Item1);
            iterationBoundingBox = new Point(4, setupBoundingBox.Y + 5);
            detailsBoundingBox = new Point(4, iterationBoundingBox.Y + 10);

            WriteSetup();
            this.RegisterEvents();
        }

        private void RegisterEvents()
        {
            this.service.OnStart += (s, e) =>
            {
                this.stopWatch.Start();
                WriteSetup();
            };

            this.service.OnStartIterations += (s, e) =>
            {
                WriteIteration(this.service.GeneticAlgorithm.CurrentIteration);
            };

            this.service.OnIterate += (s, currentIteration) =>
            {
                //if (stopWatch.ElapsedMilliseconds % 1000 < 700 && !service.ThereIsSolution) return;

                WriteIteration(this.service.GeneticAlgorithm.CurrentIteration);
            };

            this.service.OnFinish += (s, solution) =>
            {
                WriteIteration(this.service.GeneticAlgorithm.CurrentIteration);
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
            Console.Write($"Elapsed time        [{(decimal)stopWatch.ElapsedMilliseconds / 1000}s]");

            Console.SetCursorPosition(leftMargin, topPosition++);
            Console.Write($"Current iteration   [{currentIteration}]");

            Console.SetCursorPosition(leftMargin, topPosition++);
            Console.Write($"Solution found      [");
            Console.ForegroundColor = service.GeneticAlgorithm.ThereIsSolution ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{ service.GeneticAlgorithm.ThereIsSolution}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("]");

            Console.SetCursorPosition(leftMargin, topPosition++);
            Console.Write($"Best solution       [");
            Console.ForegroundColor = ConsoleColorRtoG(service.GeneticAlgorithm.BestChromosome.Fitness);
            Console.Write(Math.Round(service.GeneticAlgorithm.BestChromosome.Fitness, 4));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"]-[{service.GeneticAlgorithm.BestChromosome.Chromosome}]");
        }

        private (string, Point) setupTemplate()
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

        private ConsoleColor ConsoleColorRtoG(decimal percentage)
        {
            List<ConsoleColor> colors = new List<ConsoleColor> { ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow, ConsoleColor.Gray, ConsoleColor.DarkCyan, ConsoleColor.DarkGreen, ConsoleColor.Green };

            int indexColor = ((int)(percentage * 100)) * (colors.Count - 1) / 100;

            return colors[indexColor];
        }

    }
}
