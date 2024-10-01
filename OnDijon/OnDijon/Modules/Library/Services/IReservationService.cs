using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Library.Entities.Dto.Model;
using OnDijon.Modules.Library.Entities.Response;
using System.Threading.Tasks;

namespace OnDijon.Modules.Library.Services
{
    public interface IReservationService
    { 
        Task<CancelReservationResponse> CancelReservation(ReservationDto reservation);
        Task<Response> PlaceReservation(string idBorrower, string reservationId);

    }
}
