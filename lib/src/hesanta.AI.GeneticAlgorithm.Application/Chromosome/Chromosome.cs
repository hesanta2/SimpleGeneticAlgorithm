using hesanta.AI.GeneticAlgorithm.Gene;

namespace hesanta.AI.GeneticAlgorithm.Chromosome
{
    public class Chromosome<T> : IChromosome<T>
        where T : IGene
    {
        private readonly Func<int, T> createInstanceFunc;

        public List<T> Genes { get; } = new List<T>();
        public double MutationRate
        {
            get => Genes.Any() ? Genes.First().MutationRate : 0;
            set => Genes.ForEach(x => x.MutationRate = value);
        }
        public double MutationAmount
        {
            get => Genes.Any() ? Genes.First().MutationAmount : 0;
            set => Genes.ForEach(x => x.MutationAmount = value);
        }

        public Chromosome(int numberOfGenes, Func<int, T> createInstanceFunc = null)
        {
            this.createInstanceFunc = createInstanceFunc;
            InitializeGenes(numberOfGenes);
        }


        private void InitializeGenes(int numberOfGenes)
        {
            for (int i = 0; i < numberOfGenes; i++)
            {
                var gen = createInstanceFunc == null ? Activator.CreateInstance<T>() : createInstanceFunc(i);
                Genes.Add(gen);
            }
        }

        public void Randomize()
        {
            foreach (var gen in Genes)
            {
                gen.Randomize();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not IChromosome<T> chromosome) return false;
            if (Genes.Count != chromosome.Genes.Count) return false;

            for (int i = 0; i < Genes.Count; i++)
            {
                var gene = Genes[i];
                var compareGene = chromosome.Genes[i];
                if (gene.Equals(compareGene)) return false;
            }

            return true;
        }

        public void Crossover(IChromosome<T> chromosome)
        {
            if (Genes.Count != chromosome.Genes.Count) throw new InvalidOperationException($"Chromosomes used for recombination must have the same size. {this} = {chromosome}");

            var random = new Random();
            int firstPoint = random.Next(Genes.Count);
            int secondPoint = random.Next(firstPoint, Genes.Count);

            for (int i = firstPoint; i < secondPoint; i++)
            {
                var temp = Genes[i];
                Genes[i] = chromosome.Genes[i];
                chromosome.Genes[i] = temp;
            }
        }

        public IChromosome<T> Mutate()
        {
            var mutation = Clone() as IChromosome<T>;
            var random = new Random((int)DateTime.Now.Ticks);

            int index = random.Next(mutation.Genes.Count);

            mutation.Genes[index].Mutate();

            return mutation;
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", Genes)}]";
        }

        public override int GetHashCode()
        {
            var hashCode = -1693470089;
            foreach (var gene in Genes)
            {
                hashCode = hashCode * -1521134295 + gene.GetHashCode();
            }
            hashCode = hashCode * -1521134295 + Genes.Count.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            var clone = new Chromosome<T>(Genes.Count, createInstanceFunc: createInstanceFunc);
            for (int i = 0; i < Genes.Count; i++)
            {
                var gene = Genes[i];
                clone.Genes[i] = (T)gene.Clone();
            }

            return clone;
        }
    }
}