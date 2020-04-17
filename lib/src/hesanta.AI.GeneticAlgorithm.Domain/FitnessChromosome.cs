using System.Collections.Generic;

namespace hesanta.AI.GA.Domain
{
    public class FitnessChromosome<TGene> : IFitnessChromosome<TGene>
        where TGene : IGene
    {
        public decimal Fitness { get; }
        public IChromosome<TGene> Chromosome { get; }

        public FitnessChromosome(decimal fitness, IChromosome<TGene> chromosome)
        {
            Fitness = fitness;
            Chromosome = chromosome;
        }

        public override bool Equals(object obj)
        {
            var fitnessChromosome = obj as FitnessChromosome<TGene>;

            if (fitnessChromosome == null) return false;

            if (this.Fitness != fitnessChromosome.Fitness || this.Chromosome.Equals(fitnessChromosome.Chromosome))
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"[{Fitness}]-[{Chromosome}]";
        }

        public override int GetHashCode()
        {
            var hashCode = 1979341316;
            hashCode = hashCode * -1521134295 + Fitness.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IChromosome<TGene>>.Default.GetHashCode(Chromosome);
            return hashCode;
        }
    }
}
