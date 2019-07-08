using System;

namespace hesanta.AI.GA.Domain
{
    public interface IGene : ICloneable
    {
        string Value { get; }

        bool Equals(object obj);
        void Randomize();
        void Mutate();
        int GetHashCode();
    }


}