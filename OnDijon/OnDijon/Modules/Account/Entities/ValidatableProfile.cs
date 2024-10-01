using OnDijon.Common.Utils.Helpers;
using OnDijon.Common.Utils.Validations;
using System;
using OnDijon.Modules.Account.Entities.Models;

namespace OnDijon.Modules.Account.Entities
{
    public class ValidatableProfile : IValidatable
    {
        private ProfileModel _content;
        public ProfileModel Content
        {
            get
            {
                PropertiesToContent();
                return _content;
            }
            set
            {
                _content = value;
                ContentToProperties();
            }
        }

        public ValidatableObject<string> Name { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<string> FirstName { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<string> Mail { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<DateTime?> Birthday { get; set; } = new ValidatableObject<DateTime?>();

        public ValidatableObject<string> PhoneNumber { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<string> Gender { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<string> PasswordConfirmation { get; set; } = new ValidatableObject<string>();

        public bool IsValid
        {
            get => Name.IsValid && FirstName.IsValid && Mail.IsValid && Birthday.IsValid && PhoneNumber.IsValid && Gender.IsValid;
            set => throw new NotImplementedException();
        }

        public ValidatableProfile()
        {
            AddValidations();
        }

        private void AddValidations()
        {
            Name.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Nom requis" });
            Name.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Nom invalide",
                Predicate = (value) => value != null && RegexHelper.CheckAlphabetRegex(value)
            });

            FirstName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Prénom requis" });
            FirstName.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Prénom invalide",
                Predicate = (value) => value != null && RegexHelper.CheckAlphabetRegex(value)
            });

            Mail.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Email requis" });
            Mail.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Email invalide",
                Predicate = (value) => value != null && RegexHelper.CheckEmailRegex(value)
            });

            PhoneNumber.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Téléphone invalide",
                Predicate = (value) => string.IsNullOrEmpty(value) || RegexHelper.CheckMobileNumberRegex(value)
            });

            Gender.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Civilité requise" });

            Password.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Mot de passe requis" });
            Password.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Le mot de passe doit contenir au moins 8 caractères dont au moins une majuscule, une minuscule et un chiffre",
                Predicate = (value) => value != null && RegexHelper.CheckPasswordRegex(value)
            });

            PasswordConfirmation.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Les mots de passe sont différents",
                Predicate = (value) => string.Equals(value, Password.Value)
            });
        }

        public bool Validate()
        {
            return Name.Validate() && FirstName.Validate() && Mail.Validate() && Birthday.Validate() && PhoneNumber.Validate() && Gender.Validate();
        }

        public bool ValidatePassword()
        {
            return Password.Validate() && PasswordConfirmation.Validate();
        }

        private void PropertiesToContent()
        {
            _content = _content ?? new ProfileModel();
            _content.Name = Name.Value;
            _content.FirstName = FirstName.Value;
            _content.Mail = Mail.Value;
            _content.Birthday = Birthday.Value;
            _content.PhoneNumber = PhoneNumber.Value;
            _content.Gender = Gender.Value;
        }

        private void ContentToProperties()
        {
            if (_content != null)
            {
                Name.Value = _content.Name;
                FirstName.Value = _content.FirstName;
                Mail.Value = _content.Mail;
                Birthday.Value = _content.Birthday;
                PhoneNumber.Value = _content.PhoneNumber;
                Gender.Value = _content.Gender;
            }
        }
    }
}
