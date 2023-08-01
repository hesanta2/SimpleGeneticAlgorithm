using hesanta.AI.GA.Domain;
using hesanta.AI.NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticCodeConsole.NeuronGenes
{
    public class NeuronGene : Gene<NeuronData>
    {
        public override double MutationRate { get; set; } = 0.5;
        public override double MutationAmount { get; set; } = 0.1;

        private static readonly Random random = new(DateTime.Now.Millisecond);

        public NeuronGene()
        {
            Value = new NeuronData();
        }

        public override object Clone()
        {
            var clone = new NeuronGene();
            clone.Value.Weights = new List<double>(Value.Weights);
            clone.Value.Bias = Value.Bias;
            clone.MutationRate = MutationRate;
            clone.MutationAmount = MutationAmount;

            return clone;
        }

        public override void Mutate()
        {
            // Mutate weights
            for (int i = 0; i < Value.Weights.Count; i++)
            {
                if (random.NextDouble() < MutationRate)
                {
                    double mutation = (random.NextDouble() - 0.5) * 2 * MutationAmount;
                    Value.Weights[i] += mutation;
                }
            }

            // Mutate bias
            if (random.NextDouble() < MutationRate)
            {
                double mutation = (random.NextDouble() - 0.5) * 2 * MutationAmount;
                Value.Bias += mutation;
            }
        }

        public override void Randomize()
        {
            // Randomize weights
            for (int i = 0; i < Value.Weights.Count; i++)
            {
                Value.Weights[i] = random.NextDouble();
            }

            // Randomize bias
            Value.Bias = random.NextDouble();
        }
    }
}
