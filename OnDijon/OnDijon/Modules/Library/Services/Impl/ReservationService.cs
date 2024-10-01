using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnDijon.Modules.Library.Entities.Dto;
using OnDijon.Modules.Library.Entities.Model;
using OnDijon.Modules.Library.Entities.Request;
using OnDijon.Modules.Library.Entities.Response;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Entities;
using OnDijon.Modules.Library.Entities.Dto.Model;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Library.Services.Impl
{
    public class ReservationService : IReservationService
    {
        readonly IHttpService _httpService;
        readonly ISession _session;

        public ReservationService(ISession session, IHttpService httpService)
        {
            _session = session;
            _httpService = httpService;
        }

        private async Task<CancelReservationListDto> CancelReservationAsync(ReservationDto reservation)
        {
            CancelReservationListDto _cancelReservation = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_GET_CANCELRESERVATION);

                CancelReservationRequest r = new CancelReservationRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = _session.Profile.Guid,
                    IdBorrower = reservation.IdBorrower,
                    IdReservations = reservation.Id //+ "ERROR",//Test error
                };
                var json = JsonConvert.SerializeObject(r);

                _cancelReservation = await _httpService.PostAsync<CancelReservationListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _cancelReservation;
        }

        public async Task<CancelReservationResponse> CancelReservation(ReservationDto reservation)
        {
            var sources = await CancelReservationAsync(reservation);

            var response = Utils.Translate<CancelReservationResponse, CancelReservationListDto>(sources);
            if (response.IsSuccessful())
            {

                response.CancelReservation = new CancelReservation()
                {
                    IdBorrower = sources.CancelReservation.IdBorrower,
                    FirstName = sources.CancelReservation.FirstName,
                    IsCanceled = sources.CancelReservation.IsCanceled,
                    Name = sources.CancelReservation.Name,
                    ErrorCode = sources.CancelReservation.ErrorCode,
                    NotCanceledReason = sources.CancelReservation.NotCanceledReason,
                    ReservationId = sources.CancelReservation.ReservationId
                };
              
            }
            return response;
        }

        public async Task<Response> PlaceReservation(string idBorrower, string reservationId)
        {
            WsDMDto sources = await PlaceReservationAsync(idBorrower, reservationId);
            return Utils.Translate<Response, WsDMDto>(sources);
        }

        private async Task<WsDMDto> PlaceReservationAsync(string idBorrower, string reservationId)
        {
            WsDMDto _res = new WsDMDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.BM_SERVICES, Constants.BM_PLACE_RESERVATION);
                PlaceReservationRequest data = new PlaceReservationRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EditId = _session.Profile.Guid,
                    IdBorrower = idBorrower,
                    IdReservation = reservationId,

                };
                string json = JsonConvert.SerializeObject(data);
                _res = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _res;
        }

        
    }

}
