﻿using hesanta.AI.GeneticAlgorithm.Chromosome;
using hesanta.AI.GeneticAlgorithm.Gene;

namespace hesanta.AI.GeneticAlgorithm
{
    public class GeneticAlgorithm<T> : IGeneticAlgorithm<T>
        where T : IGene
    {
        private readonly Random random = new Random((int)DateTime.Now.Ticks);
        private readonly Func<int, T> createChromosomeInstanceFunc;
        private double initialMutationRate;

        public event EventHandler<IChromosome<T>> OnInitializePopulationChromosome;
        public event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationSelectParents;
        public event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationParentsRecombination;
        public event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationCreateChildren;

        public decimal Error { get; private set; }
        public double ElitistSelectionRate { get; set; }
        public int NonImprovingGenerationCount { get; private set; } = 0;
        public decimal MaxFitnessScore { get; private set; } = 0;
        public int NonImprovingGenerationThreshold { get; set; }
        public bool SolutionFound { get; private set; } = false;
        public int CurrentIteration { get; private set; }
        public int PopulationNumber { get; }
        public int GensPerChromosome { get; }
        public Func<IChromosome<T>, decimal> FitnessFunc { get; }
        public ICollection<IChromosome<T>> Chromosomes { get; private set; } = new List<IChromosome<T>>();
        public ICollection<IFitnessChromosome<T>> FitnessChromosomes { get; private set; } = new List<IFitnessChromosome<T>>();
        public IFitnessChromosome<T> BestChromosome
        {
            get
            {
                if (!FitnessChromosomes.Any())
                {
                    return null;
                }
                decimal maxFitness = FitnessChromosomes.Max(cf => cf.Fitness);
                return FitnessChromosomes.FirstOrDefault(cf => cf.Fitness == maxFitness);
            }
        }

        public GeneticAlgorithm(int population, int gens, Func<IChromosome<T>, decimal> fitnessFunc, decimal error = 0.001M, double elitismRate = 0.1, int staleGenerationThreshold = 100, Func<int, T> createChromosomeInstanceFunc = null)
        {
            PopulationNumber = population;
            GensPerChromosome = gens;
            Error = error;
            ElitistSelectionRate = elitismRate;
            NonImprovingGenerationThreshold = staleGenerationThreshold;
            this.createChromosomeInstanceFunc = createChromosomeInstanceFunc;
            FitnessFunc = fitnessFunc;
        }


        public void CreateInitialPopulation()
        {
            for (int i = 0; i < PopulationNumber; i++)
            {
                var chromosome = new Chromosome<T>(GensPerChromosome, createInstanceFunc: createChromosomeInstanceFunc);
                chromosome.Randomize();
                Chromosomes.Add(chromosome);

                OnInitializePopulationChromosome?.Invoke(this, chromosome);
            }

            if (Chromosomes.Any())
            {
                initialMutationRate = Chromosomes.First().MutationRate;
            }
        }

        public void EvaluateChromosomeFitness()
        {
            FitnessChromosomes.Clear();
            foreach (var chromosome in Chromosomes)
            {
                var fitnessValue = FitnessFunc(chromosome);
                if (1 - fitnessValue < Error) SolutionFound = true;
                var fitnessChromosome = new FitnessChromosome<T>(fitnessValue, chromosome);
                FitnessChromosomes.Add(fitnessChromosome);
            }

            decimal maxFitness = FitnessChromosomes.Max(cf => cf.Fitness);

            if (maxFitness > MaxFitnessScore)
            {
                MaxFitnessScore = maxFitness;
                NonImprovingGenerationCount = 0;

                foreach (var chromosome in Chromosomes)
                {
                    chromosome.MutationRate = initialMutationRate;
                }
            }
            else
            {
                NonImprovingGenerationCount++;
            }

            if (NonImprovingGenerationCount % NonImprovingGenerationThreshold == 0 && NonImprovingGenerationCount > 0)
            {
                foreach (var chromosome in Chromosomes)
                {
                    chromosome.MutationRate += 0.1;
                    if (chromosome.MutationRate > 1)
                    {
                        chromosome.MutationRate = 1;
                    }
                }
            }

            CurrentIteration++;
        }

        public void CreateNextGeneration()
        {
            var parents = ChooseParentsForNextGeneration();
            var parentOne = parents.Item1;
            var parentTwo = parents.Item2;

            OnNextPopulationSelectParents?.Invoke(this, (parentOne, parentTwo));

            var eliteIndividuals = new List<IChromosome<T>>();
            if (ElitistSelectionRate > 0)
            {
                int eliteCount = (int)(PopulationNumber * ElitistSelectionRate);
                eliteIndividuals = FitnessChromosomes
                    .OrderByDescending(fc => fc.Fitness)
                    .Take(eliteCount)
                    .Select(fc => fc.Chromosome)
                    .ToList();
            }

            GenerateChildPopulationFromParents(parentOne, parentTwo);

            foreach (var elite in eliteIndividuals)
            {
                Chromosomes.Add((IChromosome<T>)elite.Clone());
            }
        }

        private void GenerateChildPopulationFromParents(IChromosome<T> parentOne, IChromosome<T> parentTwo)
        {
            parentOne.Crossover(parentTwo);
            OnNextPopulationParentsRecombination?.Invoke(this, (parentOne, parentTwo));

            Chromosomes.Clear();
            FitnessChromosomes.Clear();
            for (int i = 0; i < PopulationNumber; i++)
            {
                var parent = random.Next(2) == 0 ? parentOne : parentTwo;
                var newChild = parent.Mutate();
                Chromosomes.Add(newChild);
                OnNextPopulationCreateChildren?.Invoke(this, (parent, newChild));
            }
        }

        private Tuple<IChromosome<T>, IChromosome<T>> ChooseParentsForNextGeneration()
        {
            var maxFitnessValue = FitnessChromosomes.Max(cf => cf.Fitness);
            var firstParent = FitnessChromosomes.Where(cf => cf.Fitness == maxFitnessValue).First().Chromosome;

            var secondMaxFitnessValue = FitnessChromosomes.Where(cf => cf.Chromosome != firstParent).Max(cf => cf.Fitness);
            var secondParent = FitnessChromosomes.Where(cf => cf.Fitness == secondMaxFitnessValue && cf.Chromosome != firstParent).First().Chromosome;

            return Tuple.Create(firstParent, secondParent);
        }
    }
}