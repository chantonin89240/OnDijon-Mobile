using OnDijon.Modules.UsefulContact.Entities.Responses;
using System.Threading.Tasks;

namespace OnDijon.Modules.UsefulContact.Services.Interfaces
{
    public interface IContactDomainService
    {
        Task<ContactDomainListResponse> GetDomains();
        Task<ContactListResponse> SearchContact(string searchText, string idDomain);
    }
}