using System;
using System.Collections.Generic;

namespace hesanta.AI.GA.Domain
{
    public interface IChromosome<T> : ICloneable
        where T : IGene
    {
        List<T> Genes { get; }
        void Crossover(IChromosome<T> chromosomeToRecombine);

        void Randomize();
        string ToString();
        IChromosome<T> Mutate();
        bool Equals(object obj);
        int GetHashCode();
    }
}