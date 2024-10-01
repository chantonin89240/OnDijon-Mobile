using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace OnDijon.Common.Utils.Validations
{
    /// <summary>
    /// https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Mobile/eShopOnContainers/eShopOnContainers.Core/Validations/ValidatableObject.cs
    /// </summary>
    public class ValidatableObject<T> : BindableBase, IValidatable
    {
        private List<string> _errors;
        private T _value;
        private bool _isValid;

        public List<IValidationRule<T>> Validations { get; }

        public List<string> Errors
        {
            get
            {
                return _errors ?? (_errors = new List<string>());
            }
            set
            {
                _errors = value;
                RaisePropertyChanged();
            }
        }

        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged();
            }
        }

        public ValidatableObject()
        {
            _isValid = true;
            Validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}
