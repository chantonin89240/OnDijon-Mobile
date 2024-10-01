
using System;

namespace OnDijon.Common.Utils.Validators.Validators
{
    public class NotNullValidator<T> : ValidatorBase<T>
    {
        public NotNullValidator(string propertyName, Func<T> propertyValueFunc, string message) 
            : base(propertyName, propertyValueFunc, message)
        {
        }

        protected override bool Validate(T value)
            => value != null;
    }
}
