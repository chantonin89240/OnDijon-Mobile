using OnDijon.Common.Utils.Helpers;
using OnDijon.Common.Entities.Model;
using OnDijon.Common.Utils.Validations;
using OnDijon.Modules.JobOffer.Entities.Requests;
using System;
using System.Linq;

namespace OnDijon.Modules.JobOffer.Entities
{
    public class ValidatableJobApplicationRequest : IValidatable
    {
        private JobApplicationRequest _JobApplicationRequest;
        public JobApplicationRequest JobApplicationRequest
        {
            get
            {
                PropertiesToDto();
                return _JobApplicationRequest;
            }
            set
            {
                _JobApplicationRequest = value;
                DtoToProperties();
            }
        }

        public ValidatableObject<string> Civility { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> ApplicantFirstName { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> ApplicantName { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> ApplicantEmail { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> ApplicantPhone { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> ApplicantCV { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> ApplicantCoverLetter { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<AddressModel> ApplicantAddressModel { get; set; } = new ValidatableObject<AddressModel>();


        public ValidatableJobApplicationRequest()
        {
            AddValidations();
        }

        private void AddValidations()
        {
            ApplicantFirstName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Prénom requis" });
            ApplicantFirstName.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Prénom invalide",
                Predicate = (value) => value != null && RegexHelper.CheckAlphabetRegex(value)
            });

            ApplicantName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Nom requis" });
            ApplicantName.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Nom invalide",
                Predicate = (value) => value != null && RegexHelper.CheckAlphabetRegex(value)
            });

            ApplicantEmail.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Email requis" });
            ApplicantEmail.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Email invalide",
                Predicate = (value) => value != null && RegexHelper.CheckEmailRegex(value)
            });

            ApplicantPhone.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Téléphone requis" });
            ApplicantPhone.Validations.Add(new PredicateRule<string>
            {
                ValidationMessage = "Téléphone invalide",
                Predicate = (value) => value != null && RegexHelper.CheckMobileNumberRegex(value)
            });
            ApplicantCoverLetter.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Lettre de motivation invalide, assurez vous de sélectionner un document au format supporté" });
            ApplicantCV.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "CV invalide, assurez vous de sélectionner un document au format supporté" });
            ApplicantAddressModel.Validations.Add(new PredicateRule<AddressModel>
            {
                ValidationMessage = "Adresse invalide",
                Predicate = (value) => value != null && (value.City != null && value.FullAddress != null)
            });


        }

        public bool IsValid
        {
            get => ApplicantFirstName.IsValid &&
                ApplicantName.IsValid &&
                ApplicantEmail.IsValid &&
                ApplicantPhone.IsValid &&
                Civility.IsValid &&
                ApplicantCoverLetter.IsValid &&
                ApplicantCV.IsValid &&
                ApplicantAddressModel.IsValid;
            set => throw new NotImplementedException();
        }

        public bool Validate()
        {
            return new[]
            {
                Civility.Validate(),
                ApplicantFirstName.Validate(),
                ApplicantName.Validate(),
                ApplicantEmail.Validate(),
                ApplicantPhone.Validate(),
                ApplicantCV.Validate(),
                ApplicantCoverLetter.Validate(),
                ApplicantCoverLetter.Validate(),
                ApplicantAddressModel.Validate()
           }.All(v => v);
        }

        private void PropertiesToDto()
        {
            _JobApplicationRequest = _JobApplicationRequest ?? new JobApplicationRequest();
            _JobApplicationRequest.ApplicantName = ApplicantName.Value;
            _JobApplicationRequest.ApplicantFirstName = ApplicantFirstName.Value;
            _JobApplicationRequest.ApplicantEmail = ApplicantEmail.Value;
            _JobApplicationRequest.ApplicantPhone = ApplicantPhone.Value;
            _JobApplicationRequest.Civility = Civility.Value;
            _JobApplicationRequest.ApplicantCoverLetter = ApplicantCoverLetter.Value;
            _JobApplicationRequest.ApplicantCV = ApplicantCoverLetter.Value;
            _JobApplicationRequest.ApplicantAddressModel = ApplicantAddressModel.Value;
        }


        private void DtoToProperties()
        {
            if (_JobApplicationRequest != null)
            {
                ApplicantName.Value = _JobApplicationRequest.ApplicantName;
                ApplicantFirstName.Value = _JobApplicationRequest.ApplicantFirstName;
                ApplicantEmail.Value = _JobApplicationRequest.ApplicantEmail;
                ApplicantPhone.Value = _JobApplicationRequest.ApplicantPhone;
                Civility.Value = _JobApplicationRequest.Civility;
                ApplicantCoverLetter.Value = _JobApplicationRequest.ApplicantCoverLetter;
                ApplicantCV.Value = _JobApplicationRequest.ApplicantCV;
                ApplicantAddressModel.Value = _JobApplicationRequest.ApplicantAddressModel;
            }

        }
    }
}
