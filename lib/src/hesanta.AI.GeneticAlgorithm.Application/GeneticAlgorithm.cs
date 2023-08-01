using hesanta.AI.GA.Domain;

namespace hesanta.AI.GA.Application
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
        public double ElitismRate { get; set; }
        public int StaleGenerations { get; private set; } = 0;
        public decimal BestFitness { get; private set; } = 0;
        public int StaleGenerationThreshold { get; set; }
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
            ElitismRate = elitismRate;
            StaleGenerationThreshold = staleGenerationThreshold;
            this.createChromosomeInstanceFunc = createChromosomeInstanceFunc;
            FitnessFunc = fitnessFunc;
        }


        public void InitializePopulation()
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

        public void EvaluateFitness()
        {
            FitnessChromosomes.Clear();
            foreach (var chromosome in Chromosomes)
            {
                var fitnessValue = FitnessFunc(chromosome);
                if (1 - fitnessValue < Error) ThereIsSolution = true;
                var fitnessChromosome = new FitnessChromosome<T>(fitnessValue, chromosome);
                FitnessChromosomes.Add(fitnessChromosome);
            }

            decimal maxFitness = FitnessChromosomes.Max(cf => cf.Fitness);

            if (maxFitness > BestFitness)
            {
                BestFitness = maxFitness;
                StaleGenerations = 0;

                foreach (var chromosome in Chromosomes)
                {
                    chromosome.MutationRate = initialMutationRate;
                }
            }
            else
            {
                StaleGenerations++;
            }

            if (StaleGenerations % StaleGenerationThreshold == 0 && StaleGenerations > 0)
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

        public void GetNextPopulation()
        {
            var parents = SelectParents();
            var parentOne = parents.Item1;
            var parentTwo = parents.Item2;

            OnNextPopulationSelectParents?.Invoke(this, (parentOne, parentTwo));

            var eliteIndividuals = new List<IChromosome<T>>();
            if (ElitismRate > 0)
            {
                int eliteCount = (int)(PopulationNumber * ElitismRate);
                eliteIndividuals = FitnessChromosomes
                    .OrderByDescending(fc => fc.Fitness)
                    .Take(eliteCount)
                    .Select(fc => fc.Chromosome)
                    .ToList();
            }

            CreateChildrenPopulation(parentOne, parentTwo);

            foreach (var elite in eliteIndividuals)
            {
                Chromosomes.Add((IChromosome<T>)elite.Clone());
            }
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