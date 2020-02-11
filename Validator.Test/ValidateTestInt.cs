using System;

namespace Validator.Test
{
    public class ValidateTestInt : IEquatable<ValidateTestInt>,
        IComparable<ValidateTestInt>
    {
        private int Value { get; }

        public ValidateTestInt(int value)
        {
            Value = value;
        }

        public bool Equals(ValidateTestInt other)
        {
            if (other == null) return false;
            return Value == other.Value;
        }

        public int CompareTo(ValidateTestInt other)
        {
            if (other == null) return 1;
            return Value - other.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator ValidateTestInt(int value)
            => new ValidateTestInt(value);
    }
}