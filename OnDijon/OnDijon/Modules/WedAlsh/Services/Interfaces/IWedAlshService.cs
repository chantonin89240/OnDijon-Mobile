using OnDijon.Modules.WedAlsh.Entities.Request;
using OnDijon.Modules.WedAlsh.Entities.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDijon.Modules.WedAlsh.Services.Interfaces
{
    public interface IWedAlshService
    {
        Task<WedAlshRegistrationsResponse> GetRegistrations(string profilEditId);

        Task<WedAlshSchedulesResponse> UpdateRegistrations(string registrationEditId, List<ScheduleAction> changeList);
    }
}
