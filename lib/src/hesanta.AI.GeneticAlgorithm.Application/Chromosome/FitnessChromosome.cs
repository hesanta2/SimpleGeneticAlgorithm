using hesanta.AI.GeneticAlgorithm.Gene;

namespace hesanta.AI.GeneticAlgorithm.Chromosome
{
    public class FitnessChromosome<T> : IFitnessChromosome<T>
        where T : IGene
    {
        public decimal Fitness { get; }
        public IChromosome<T> Chromosome { get; }

        public FitnessChromosome(decimal fitness, IChromosome<T> chromosome)
        {
            Fitness = fitness;
            Chromosome = chromosome;
        }

        public override bool Equals(object obj)
        {
            var fitnessChromosome = obj as FitnessChromosome<T>;

            if (fitnessChromosome == null) return false;

            if (Fitness != fitnessChromosome.Fitness || Chromosome.Equals(fitnessChromosome.Chromosome))
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
            hashCode = hashCode * -1521134295 + EqualityComparer<IChromosome<T>>.Default.GetHashCode(Chromosome);
            return hashCode;
        }
    }
}
