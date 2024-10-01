using OnDijon.Modules.School.Entities.Response;
using System.Threading.Tasks;

namespace OnDijon.Modules.School.Services.Interfaces
{
    public interface IRSCalendar
    {
        Task<SchoolRestaurantCalendarListResponse> GetMenusList();

    }
}
