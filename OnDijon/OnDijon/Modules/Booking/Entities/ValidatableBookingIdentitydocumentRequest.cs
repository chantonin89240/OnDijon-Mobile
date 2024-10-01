using OnDijon.Common.Utils.Helpers;
using OnDijon.Common.Utils.Validations;
using OnDijon.Modules.Booking.Entities.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDijon.Modules.Booking.Entities
{
    public class ValidatableBookingIdentitydocumentRequest : IValidatable
    {
        private BookingRequest _bookingRequest;
        public BookingRequest BookingRequest
        {
            get
            {
                PropertiesToDto();
                return _bookingRequest;
            }
            set
            {
                _bookingRequest = value;
                DtoToProperties();
            }
        }

        public ValidatableObject<IEnumerable<string>> RequestReason { get; set; } = new ValidatableObject<IEnumerable<string>>();

        public ValidatableObject<string> DocumentCivility { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<string> DocumentName { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<string> DocumentFirstName { get; set; } = new ValidatableObject<string>();

        public ValidatableObject<DateTime?> DocumentBirthDate { get; set; } = new ValidatableObject<DateTime?>();

        public ValidatableObject<string> ApplicantPhone { get; set; } = new ValidatableObject<string>();


        public bool IsValid
        {
            get => RequestReason.IsValid && DocumentCivility.IsValid && DocumentName.IsValid && DocumentFirstName.IsValid && DocumentBirthDate.IsValid && ApplicantPhone.IsValid;
            set => throw new NotImplementedException();
        }

        public ValidatableBookingIdentitydocumentRequest()
        {
            AddValidations();
        }

        private void AddValidations()
        {
            RequestReason.Validations.Add(new AnyRule { ValidationMessage = "Motif requis" });

            DocumentName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Nom requis" });
            DocumentName.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Nom invalide",
                Predicate = (value) => value != null && RegexHelper.CheckAlphabetRegex(value)
            });

            DocumentFirstName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Prénom requis" });
            DocumentFirstName.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Prénom invalide",
                Predicate = (value) => value != null || RegexHelper.CheckAlphabetRegex(value)
            });

            DocumentBirthDate.Validations.Add(new PredicateRule<DateTime?>
            {
                ValidationMessage = "Date de naissance requise",
                Predicate = (value) => value != null
            });

            ApplicantPhone.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Téléphone requis" });
            ApplicantPhone.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Téléphone invalide",
                Predicate = (value) => value != null && RegexHelper.CheckMobileNumberRegex(value)
            });
        }

        public bool Validate()
        {
            return new[] { 
                DocumentCivility.Validate(), 
                DocumentName.Validate(), 
                DocumentFirstName.Validate(), 
                DocumentBirthDate.Validate(), 
                ApplicantPhone.Validate() 
            }.All(v => v);
        }

        public bool ValidateRequestReason()
        {
            return RequestReason.Validate();
        }

        private void PropertiesToDto()
        {
            _bookingRequest = _bookingRequest ?? new BookingRequest();
            _bookingRequest.RequestReason = RequestReason.Value;
            _bookingRequest.DocumentCivility = DocumentCivility.Value;
            _bookingRequest.DocumentName = DocumentName.Value;
            _bookingRequest.DocumentFirstName = DocumentFirstName.Value;
            _bookingRequest.DocumentBirthDate = DocumentBirthDate.Value;
            _bookingRequest.ApplicantPhone = ApplicantPhone.Value;
        }


        private void DtoToProperties()
        {
            if (_bookingRequest != null)
            {
                RequestReason.Value = _bookingRequest.RequestReason;
                DocumentCivility.Value = _bookingRequest.DocumentCivility;
                DocumentName.Value = _bookingRequest.DocumentName;
                DocumentFirstName.Value = _bookingRequest.DocumentFirstName;
                DocumentBirthDate.Value = _bookingRequest.DocumentBirthDate;
                ApplicantPhone.Value = _bookingRequest.ApplicantPhone;
            }
        }
    }
}
