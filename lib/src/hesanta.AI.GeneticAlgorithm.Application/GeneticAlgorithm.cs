using hesanta.AI.GA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hesanta.AI.GA.Application
{
    public class GeneticAlgorithm<TGene> : IGeneticAlgorithm<TGene> where TGene : IGene
    {
        private readonly Random random = new Random((int)DateTime.Now.Ticks);

        public event EventHandler<IChromosome<TGene>> OnInitializePopulationChromosome;
        public event EventHandler<(IChromosome<TGene>, IChromosome<TGene>)> OnNextPopulationSelectParents;
        public event EventHandler<(IChromosome<TGene>, IChromosome<TGene>)> OnNextPopulationParentsRecombination;
        public event EventHandler<(IChromosome<TGene>, IChromosome<TGene>)> OnNextPopulationCreateChildren;

        public decimal Error { get; private set; }
        public bool ThereIsSolution { get; private set; } = false;
        public int CurrentIteration { get; private set; }
        public int PopulationNumber { get; }
        public int GensPerChromosome { get; }
        public IFitnessFunction<TGene> FitnessFunction { get; }
        public ICollection<IChromosome<TGene>> Chromosomes { get; private set; } = new List<IChromosome<TGene>>();
        public ICollection<IFitnessChromosome<TGene>> FitnessChromosomes { get; private set; } = new List<IFitnessChromosome<TGene>>();
        public IFitnessChromosome<TGene> BestChromosome
        {
            get
            {
                decimal maxFitness = this.FitnessChromosomes.Max(cf => cf.Fitness);
                return FitnessChromosomes.FirstOrDefault(cf => cf.Fitness == maxFitness);
            }
        }

        public GeneticAlgorithm(int population, int gens, IFitnessFunction<TGene> fitnessFunction, decimal error = 0.001M)
        {
            PopulationNumber = population;
            GensPerChromosome = gens;
            FitnessFunction = fitnessFunction;
            Error = error;
        }

        public void InitializePopulation()
        {
            for (int i = 0; i < this.PopulationNumber; i++)
            {
                var chromosome = new Chromosome<TGene>(this.GensPerChromosome);
                chromosome.Randomize();
                this.Chromosomes.Add(chromosome);

                OnInitializePopulationChromosome?.Invoke(this, chromosome);
            }
        }

        public void EvaluateFitness()
        {
            FitnessChromosomes.Clear();
            foreach (var chromosome in this.Chromosomes)
            {
                var fitnessValue = this.FitnessFunction.GetFitness(chromosome);
                if (1 - fitnessValue < this.Error) this.ThereIsSolution = true;
                var fitnessChromosome = new FitnessChromosome<TGene>(fitnessValue, chromosome);
                //if (!FitnessChromosomes.Contains(fitnessChromosome))
                FitnessChromosomes.Add(fitnessChromosome);
            }

            this.CurrentIteration++;
        }

        public void GetNextPopulation()
        {
            var parents = SelectParents();
            var parentOne = parents.Item1;
            var parentTwo = parents.Item2;

            this.OnNextPopulationSelectParents?.Invoke(this, (parentOne, parentTwo));

            CreateChildrenPopulation(parentOne, parentTwo);
        }

        private void CreateChildrenPopulation(IChromosome<TGene> parentOne, IChromosome<TGene> parentTwo)
        {
            parentOne.Recombine(parentTwo);
            this.OnNextPopulationParentsRecombination?.Invoke(this, (parentOne, parentTwo));

            this.Chromosomes.Clear();
            this.FitnessChromosomes.Clear();
            for (int i = 0; i < PopulationNumber; i++)
            {
                var parent = random.Next(2) == 0 ? parentOne : parentTwo;
                var newChild = parent.Mutate();
                this.Chromosomes.Add(newChild);
                this.OnNextPopulationCreateChildren?.Invoke(this, (parent, newChild));
            }
        }

        private Tuple<IChromosome<TGene>, IChromosome<TGene>> SelectParents()
        {
            var maxFitnessValue = this.FitnessChromosomes.Max(cf => cf.Fitness);
            var firstParent = this.FitnessChromosomes.Where(cf => cf.Fitness == maxFitnessValue).First().Chromosome;

            var secondMaxFitnessValue = this.FitnessChromosomes.Where(cf => cf.Chromosome != firstParent).Max(cf => cf.Fitness);
            var secondParent = this.FitnessChromosomes.Where(cf => cf.Fitness == secondMaxFitnessValue && cf.Chromosome != firstParent).First().Chromosome;

            return Tuple.Create(firstParent, secondParent);
        }
    }
}