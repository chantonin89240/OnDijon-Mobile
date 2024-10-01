using OnDijon.Modules.Diary.Entities.Response;
using System;
using System.Threading.Tasks;

namespace OnDijon.Modules.Diary.Services.Interfaces
{
    public interface IDiaryService
    {
        Task<EventListResponse> GetEventsByDate(DateTime startDay, DateTime endDate, int pageNumber, int resultSize);
        Task<EventListResponse> GetEventsByDiary(string DiaryEditId, DateTime startDay, DateTime endDate, int pageNumber, int resultSize);
        Task<EventListResponse> GetEventsByRequest(string Request, DateTime startDay, int pageNumber, int resultSize);
        Task<EventResponse> GetEventDetails(string EventEditId);
        Task<DiarySuggestionResponse> GetSuggestions(string query, DateTime startDate);
    }
}
