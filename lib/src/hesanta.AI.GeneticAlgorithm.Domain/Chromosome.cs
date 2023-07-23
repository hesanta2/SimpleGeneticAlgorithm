using System;
using System.Collections.Generic;

namespace hesanta.AI.GA.Domain
{
    public class Chromosome<T> : IChromosome<T>
        where T : IGene
    {
        public List<T> Genes { get; } = new List<T>();
        public int NumberOfGens { get; }

        public Chromosome(int numberOfGens)
        {
            NumberOfGens = numberOfGens;
            InitializeGens();
        }


        private void InitializeGens()
        {
            for (int i = 0; i < NumberOfGens; i++)
            {
                var gen = Activator.CreateInstance<T>();
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
            var chromosome = obj as IChromosome<T>;

            if (chromosome == null) return false;
            if (Genes.Count != chromosome.Genes.Count) return false;

            for (int i = 0; i < Genes.Count; i++)
            {
                var gene = Genes[i];
                var compareGene = chromosome.Genes[i];
                if (gene.Equals(compareGene)) return false;
            }

            return true;
        }

        public void Recombine(IChromosome<T> chromosomeToRecombine)
        {
            if (Genes.Count != chromosomeToRecombine.Genes.Count) throw new InvalidOperationException($"Chromosomes used for recombination must has the same size. {this} = {chromosomeToRecombine}");

            int halfCount = Genes.Count / 2;

            for (int i = halfCount + 1; i < Genes.Count; i++)
            {
                var gene1 = Genes[i];
                var gene2 = chromosomeToRecombine.Genes[i];
                if (i > halfCount)
                {
                    chromosomeToRecombine.Genes[i] = gene1;
                    Genes[i] = gene2;
                }
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
            hashCode = hashCode * -1521134295 + NumberOfGens.GetHashCode();
            return hashCode;
        }

        public object Clone()
        {
            var clone = new Chromosome<T>(NumberOfGens);
            for (int i = 0; i < Genes.Count; i++)
            {
                var gene = Genes[i];
                clone.Genes[i] = (T)gene.Clone();
            }

            return clone;
        }
    }
}