using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Booking.Entities.Requests;
using OnDijon.Modules.Booking.Entities.Responses;
using System;
using System.Threading.Tasks;

namespace OnDijon.Modules.Booking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<ScheduleListResponse> GetScheduleList(string city, string sessionEditId, string institutionEditId, int nbOfPerson, DateTime start, DateTime end);
        Task<InstitutionListResponse> GetBookingInstitutionsList(string city, string site);
        Task<Response> SendBook(BookingRequest data, string city);
        Task<BookingInformationsResponse> GetBookingInformation(string editId, string city);
        Task<Response> CancelBooking(string bookingEditId, string city);
    }
}