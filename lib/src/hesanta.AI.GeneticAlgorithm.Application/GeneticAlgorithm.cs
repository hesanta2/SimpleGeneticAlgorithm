using hesanta.AI.GA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hesanta.AI.GA.Application
{
    public class GeneticAlgorithm<T> : IGeneticAlgorithm<T>
        where T : IGene
    {
        private readonly Random random = new Random((int)DateTime.Now.Ticks);

        public event EventHandler<IChromosome<T>> OnInitializePopulationChromosome;
        public event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationSelectParents;
        public event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationParentsRecombination;
        public event EventHandler<(IChromosome<T>, IChromosome<T>)> OnNextPopulationCreateChildren;

        public decimal Error { get; private set; }
        public bool ThereIsSolution { get; private set; } = false;
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
                decimal maxFitness = FitnessChromosomes.Max(cf => cf.Fitness);
                return FitnessChromosomes.FirstOrDefault(cf => cf.Fitness == maxFitness);
            }
        }

        public GeneticAlgorithm(int population, int gens, Func<IChromosome<T>, decimal> fitnessFunc, decimal error = 0.001M)
        {
            PopulationNumber = population;
            GensPerChromosome = gens;
            Error = error;
            FitnessFunc = fitnessFunc;
        }


        public void InitializePopulation()
        {
            for (int i = 0; i < PopulationNumber; i++)
            {
                var chromosome = new Chromosome<T>(GensPerChromosome);
                chromosome.Randomize();
                Chromosomes.Add(chromosome);

                OnInitializePopulationChromosome?.Invoke(this, chromosome);
            }
        }

        public void EvaluateFitness()
        {
            FitnessChromosomes.Clear();
            foreach (var chromosome in Chromosomes)
            {
                var fitnessValue = FitnessFunc(chromosome);
                if (1 - fitnessValue < Error) ThereIsSolution = true;
                var fitnessChromosome = new FitnessChromosome<T>(fitnessValue, chromosome);
                //if (!FitnessChromosomes.Contains(fitnessChromosome))
                FitnessChromosomes.Add(fitnessChromosome);
            }

            CurrentIteration++;
        }

        public void GetNextPopulation()
        {
            var parents = SelectParents();
            var parentOne = parents.Item1;
            var parentTwo = parents.Item2;

            OnNextPopulationSelectParents?.Invoke(this, (parentOne, parentTwo));

            CreateChildrenPopulation(parentOne, parentTwo);
        }

        private void CreateChildrenPopulation(IChromosome<T> parentOne, IChromosome<T> parentTwo)
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

        private Tuple<IChromosome<T>, IChromosome<T>> SelectParents()
        {
            var maxFitnessValue = FitnessChromosomes.Max(cf => cf.Fitness);
            var firstParent = FitnessChromosomes.Where(cf => cf.Fitness == maxFitnessValue).First().Chromosome;

            var secondMaxFitnessValue = FitnessChromosomes.Where(cf => cf.Chromosome != firstParent).Max(cf => cf.Fitness);
            var secondParent = FitnessChromosomes.Where(cf => cf.Fitness == secondMaxFitnessValue && cf.Chromosome != firstParent).First().Chromosome;

            return Tuple.Create(firstParent, secondParent);
        }
    }
}