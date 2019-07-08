using System;
using System.Collections.Generic;

namespace hesanta.AI.GA.Domain
{
    public interface IChromosome<TGene> : ICloneable 
        where TGene : IGene
    {
        List<TGene> Genes { get; }
        void Recombine(IChromosome<TGene> chromosomeToRecombine);

        void Randomize();
        string ToString();
        IChromosome<TGene> Mutate();
        bool Equals(object obj);
        int GetHashCode();
    }
}