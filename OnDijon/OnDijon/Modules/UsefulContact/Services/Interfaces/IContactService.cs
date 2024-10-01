using OnDijon.Modules.UsefulContact.Entities.Responses;
using System;
using System.Threading.Tasks;

namespace OnDijon.Modules.UsefulContact.Services.Interfaces
{
    public interface IContactService
    {
        Task<OpeningTimeResponse> GetOpeningTime(String idContact);
    }
}