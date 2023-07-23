using System;

namespace hesanta.AI.GA.Domain
{
    public abstract class Gene<T> : IGene<T>
    {
        public virtual object RawValue { get; protected set; }
        public virtual T Value
        {
            get => (T)Convert.ChangeType(RawValue, typeof(T));
            set => RawValue = value;
        }

        public Gene(T initialValue = default)
        {
            RawValue = initialValue;
        }

        public abstract object Clone();

        public abstract void Mutate();

        public abstract void Randomize();

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return RawValue.GetHashCode();
        }

        public override string ToString()
        {
            return RawValue.ToString();
        }
    }

    public interface IGene<T> : IGene
    {
        T Value { get; }
    }

    public interface IGene : ICloneable
    {
        object RawValue { get; }
        bool Equals(object obj);
        void Randomize();
        void Mutate();
        int GetHashCode();
    }
}