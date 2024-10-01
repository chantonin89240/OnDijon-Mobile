using OnDijon.Modules.JobOffer.Entities.Requests;
using OnDijon.Modules.JobOffer.Entities.Responses;
using OnDijon.Common.Entities.Response;
using System.Threading.Tasks;

namespace OnDijon.Modules.JobOffer.Services.Interfaces
{
    public interface IJobOfferService
    {
        Task<ListJobOfferResponse> GetJobOffer();
        Task<Response> SendApplication(JobApplicationRequest data);
        Task<ListTypeJobOfferResponse> GetTypeJobOffer(string city);
    }
}
