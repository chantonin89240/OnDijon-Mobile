using OnDijon.Common.Entities.Model;

namespace OnDijon.Modules.JobOffer.Entities.Requests
{
    public class JobApplicationRequest
    {
        public string Key { get; set; }
        public string UserId { get; set; }
        public string Civility { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantPhone { get; set; }
        public string ApplicantCV { get; set; }
        public string ApplicantCVTitle { get; set; }
        public string ApplicantCoverLetter { get; set; }
        public string ApplicantCoverLetterTitle { get; set; }
        public AddressModel ApplicantAddressModel { get; set; }
        public string EditIdJobOffer { get; set; }
        public string Message { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
    }
}
