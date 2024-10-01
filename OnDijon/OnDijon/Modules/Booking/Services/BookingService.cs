using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Booking.Entities.Dto;
using OnDijon.Modules.Booking.Entities.Models;
using OnDijon.Modules.Booking.Entities.Requests;
using OnDijon.Modules.Booking.Entities.Responses;
using OnDijon.Modules.Booking.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnDijon.Modules.Booking.Services
{
    public class BookingService : IBookingService
    {
        readonly IHttpService _httpService;

        public BookingService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        private string GetCityApiUrl(string city, string method)
        {
            return string.Concat(Constants.API_URL, Constants.COMMON_DIRECTORY, city, "/", Constants.BO_SERVICES, method);
        }

        public async Task<ScheduleListResponse> GetScheduleList(string city, string sessionEditId, string institutionEditId, int nbOfPerson, DateTime start, DateTime end)
        {
            ScheduleListDto sources = await GetScheduleListAsync(city, sessionEditId, institutionEditId, nbOfPerson, start, end);
            ScheduleListResponse response = Utils.Translate<ScheduleListResponse, ScheduleListDto>(sources);

            if (response.IsSuccessful())
            {
                response.Schedules = sources.Schedules.Select(item =>
                {
                    return new ScheduleModel()
                    {
                        EditId = item.EditId,
                        EndDate = item.EndDate,
                        StartDate = item.StartDate
                    };
                }).ToList();
            }
            return response;
        }

        public async Task<InstitutionListResponse> GetBookingInstitutionsList(string city, string site)
        {
            InstitutionListDto sources = await GetBookingInstitutionsListAsync(city, site);
            InstitutionListResponse response = Utils.Translate<InstitutionListResponse, InstitutionListDto>(sources);

            if (response.IsSuccessful())
            {
                response.Institutions = sources.Institutions.Select(item =>
                {
                    return new InstitutionModel()
                    {
                        EditId = item.EditId,
                        Address = item.Address,
                        MultiplePerson = item.MultiplePerson,
                        MaxNumberOfPerson = item.MaxNumberOfPerson,
                        OpeningTime = item.OpeningTime,
                        Name = item.Name
                    };
                }).ToList();
                response.SessionEditId = sources.SessionEditId;
            }
            return response;
        }

        public async Task<Response> SendBook(BookingRequest data, string city)
        {
            WsDMDto sources = await SendBookAsync(data, city);
            return Utils.Translate<Response, WsDMDto>(sources);
        }

        public async Task<BookingInformationsResponse> GetBookingInformation(string editId, string city)
        {
            BookingInformationsDto sources = await GetBookingInformationAsync(editId, city);
            BookingInformationsResponse response = Utils.Translate<BookingInformationsResponse, BookingInformationsDto>(sources);
            if (response.IsSuccessful())
            {
                response.Civility = sources.Civility;
                response.FirstName = sources.FirstName;
                response.Name = sources.Name;
                response.NbOfPerson = sources.NbOfPerson;
                response.Institution = sources.Institution;
                response.Day = sources.Day;
            }
            return response;

        }

        public async Task<Response> CancelBooking(string bookingEditId, string city)
        {
            WsDMDto sources = await CancelBookingAsync(bookingEditId, city);
            return Utils.Translate<Response, WsDMDto>(sources);
        }

        private async Task<ScheduleListDto> GetScheduleListAsync(string city, string sessionEditId, string institutionEditId, int nbOfPerson, DateTime start, DateTime end)
        {
            ScheduleListDto _schedules = new ScheduleListDto();
            try
            {
                string url = GetCityApiUrl(city, Constants.BO_GET_SCHEDULES);
                ScheduleRequest data = new ScheduleRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    SessionEditId = sessionEditId,
                    InstitutionEditId = institutionEditId,
                    NbOfPerson = nbOfPerson,
                    StartDate = start,
                    EndDate = end
                };
                string json = JsonConvert.SerializeObject(data);
                _schedules = await _httpService.PostAsync<ScheduleListDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _schedules;
        }

        private async Task<InstitutionListDto> GetBookingInstitutionsListAsync(string city, string site)
        {
            InstitutionListDto _institutions = new InstitutionListDto();
            try
            {
                string url = GetCityApiUrl(city, Constants.BO_GET_BOOKINGINSTITUTIONS);
                InstitutionRequest data = new InstitutionRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Site = site

                };
                string json = JsonConvert.SerializeObject(data);
                _institutions = await _httpService.PostAsync<InstitutionListDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _institutions;
        }

        private async Task<WsDMDto> SendBookAsync(BookingRequest data, string city)
        {
            WsDMDto _res = new WsDMDto();
            try
            {
                string url = GetCityApiUrl(city, Constants.BO_SEND_BOOK);
                data.Key = Constants.ONDIJON_KEY;
                string json = JsonConvert.SerializeObject(data);
                _res = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _res;
        }

        private async Task<BookingInformationsDto> GetBookingInformationAsync(string bookingEditId, string city)
        {
            BookingInformationsDto _res = new BookingInformationsDto();
            try
            {
                string url = GetCityApiUrl(city, Constants.BO_GET_BOOKING_INFORMATIONS);
                BookingInformationRequest data = new BookingInformationRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    BookingEditId = bookingEditId

                };
                string json = JsonConvert.SerializeObject(data);
                _res = await _httpService.PostAsync<BookingInformationsDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _res;
        }

        private async Task<WsDMDto> CancelBookingAsync(string bookingEditId, string city)
        {
            WsDMDto _res = new WsDMDto();
            try
            {
                string url = GetCityApiUrl(city, Constants.BO_CANCEL_BOOKING);
                BookingInformationRequest data = new BookingInformationRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    BookingEditId = bookingEditId

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
