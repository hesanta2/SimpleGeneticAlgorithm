using System;
using System.Collections.Generic;

namespace hesanta.AI.GA.Domain
{
    public class Chromosome<TGene> : IChromosome<TGene> where TGene : IGene
    {
        public List<TGene> Genes { get; } = new List<TGene>();
        public int NumberOfGens { get; }

        public Chromosome(int numberOfGens)
        {
            NumberOfGens = numberOfGens;
            this.InitializeGens();
        }


        private void InitializeGens()
        {
            for (int i = 0; i < this.NumberOfGens; i++)
            {
                var gen = Activator.CreateInstance<TGene>();
                this.Genes.Add(gen);
            }
        }

        public void Randomize()
        {
            foreach (var gen in this.Genes)
            {
                gen.Randomize();
            }
        }

        public override bool Equals(object obj)
        {
            var chromosome = obj as Chromosome<TGene>;

            if (chromosome == null) return false;
            if (this.Genes.Count != chromosome.Genes.Count) return false;

            for (int i = 0; i < this.Genes.Count; i++)
            {
                var gene = this.Genes[i];
                var compareGene = chromosome.Genes[i];
                if (gene.Equals(compareGene)) return false;
            }

            return true;
        }

        public void Recombine(IChromosome<TGene> chromosomeToRecombine)
        {
            if (this.Genes.Count != chromosomeToRecombine.Genes.Count) throw new InvalidOperationException($"Chromosomes used for recombination must has the same size. {this} = {chromosomeToRecombine}");

            int halfCount = this.Genes.Count / 2;

            for (int i = halfCount + 1; i < this.Genes.Count; i++)
            {
                var gene1 = this.Genes[i];
                var gene2 = chromosomeToRecombine.Genes[i];
                if (i > halfCount)
                {
                    chromosomeToRecombine.Genes[i] = gene1;
                    this.Genes[i] = (TGene)gene2;
                }
            }
        }

        public IChromosome<TGene> Mutate()
        {
            var mutation = Clone() as IChromosome<TGene>;
            var random = new Random((int)DateTime.Now.Ticks);

            int index = random.Next(mutation.Genes.Count);

            mutation.Genes[index].Mutate();

            return mutation;
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", this.Genes)}]";
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
            var clone = new Chromosome<TGene>(this.NumberOfGens);
            for (int i = 0; i < this.Genes.Count; i++)
            {
                var gene = this.Genes[i];
                clone.Genes[i] = (TGene)(gene.Clone() as IGene);
            }

            return clone;
        }
    }
}