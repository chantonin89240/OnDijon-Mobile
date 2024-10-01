using OnDijon.Common.Entities.Response;
using OnDijon.Modules.School.Entities.Models;
using OnDijon.Modules.School.Entities.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDijon.Modules.SchoolServices.Interfaces
{
    public interface ISchoolRestaurantBookingConfigurationService
    {
        Task<SessionResponse> GetActiveSessionByCityContext();
        Task<AppointmentListResponse> GetBookingList(string childEditId, string sessionEditId, string cityContext);
        Task<WeeklyScheduleResponse> GetParentSchedule(string childEditId, string sessionEditId, string cityContext);
        Task<Response> SendBookingByCityContext(IList<AppointmentModel> datas, string cityContext);
        Task<Response> UpdateParentScheduleByCityContext(WeeklyScheduleModel datas, string cityContext);
        Task<ChildDietResponse> GetChildDietByCityContext(string id, string cityContext);
        Task<Response> UpdateChildDietByCityContext(string id, string standardDiet, bool option, string cityContext);
    }
}
