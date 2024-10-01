using System;

namespace OnDijon.Common.Utils.Validations
{
    public class PredicateRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public Predicate<T> Predicate { get; set; }

        public bool Check(T value)
        {
            return Predicate(value);
        }
    }
}
