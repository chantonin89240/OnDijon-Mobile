using OnDijon.Common.Utils;
using System;
using System.Threading.Tasks;
using OnDijon.Common.Utils.Services.Interfaces;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using System.Linq;
using Microsoft.AppCenter.Crashes;
using OnDijon.Modules.Diary.Services.Interfaces;
using OnDijon.Modules.Diary.Entities.Dto;
using OnDijon.Modules.Diary.Entities.Response;
using OnDijon.Modules.Diary.Entities.Request;

namespace OnDijon.Modules.Diary.Services
{
    public class DiaryService : IDiaryService
    {
        readonly IHttpService _httpService;

        public DiaryService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<EventListResponse> GetEventsByDate(DateTime startDay, DateTime endDate, int pageNumber, int resultSize)
        {
            EventListDto sources = await GetEventsByDateAsync(startDay, endDate, pageNumber, resultSize);

            EventListResponse response = Utils.Translate<EventListResponse, EventListDto>(sources);
            if (response.IsSuccessful())
            {
                response.Events = sources.Events;
            }
            return response;
        }
        private async Task<EventListDto> GetEventsByDateAsync(DateTime startDay, DateTime endDate, int pageNumber, int resultSize)
        {
            EventListDto response = new EventListDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.DIARY_SERVICES, Constants.DIARY_GET_EVENTS_BY_DATE);
                DateRequest r = new DateRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    StartDate = startDay,
                    EndDate = endDate,
                    PageNumber = pageNumber,
                    ResultSize = resultSize
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<EventListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }

        public async Task<EventListResponse> GetEventsByDiary(string diaryEditId, DateTime startDay, DateTime endDate, int pageNumber, int resultSize)
        {
            var sources = await GetEventsByDiaryAsync(diaryEditId, startDay, endDate, pageNumber, resultSize);

            var response = Utils.Translate<EventListResponse, EventListDto>(sources);
            if (response.IsSuccessful())
            {
                response.Events = sources.Events;
            }
            return response;
        }
        private async Task<EventListDto> GetEventsByDiaryAsync(string diaryEditId, DateTime startDay, DateTime endDate, int pageNumber, int resultSize)
        {
            EventListDto response = new EventListDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.DIARY_SERVICES, Constants.DIARY_GET_EVENTS_BY_DIARY);
                DiaryRequest r = new DiaryRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    DiaryEditId = diaryEditId,
                    StartDate = startDay,
                    EndDate = endDate,
                    PageNumber = pageNumber,
                    ResultSize = resultSize
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<EventListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }

        public async Task<EventListResponse> GetEventsByRequest(string request, DateTime startDay, int pageNumber, int resultSize)
        {
            var sources = await GetEventsByRequestAsync(request, startDay, pageNumber, resultSize);

            var response = Utils.Translate<EventListResponse, EventListDto>(sources);
            if (response.IsSuccessful())
            {
                response.Events = sources.Events;
            }
            return response;
        }
        private async Task<EventListDto> GetEventsByRequestAsync(string request, DateTime startDay, int pageNumber, int resultSize)
        {
            EventListDto response = new EventListDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.DIARY_SERVICES, Constants.DIARY_GET_EVENTS_BY_REQUEST);
                RequestRequest r = new RequestRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Request = request,
                    StartDate = startDay,
                    PageNumber = pageNumber,
                    ResultSize = resultSize
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<EventListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }

        public async Task<EventResponse> GetEventDetails(string eventEditId)
        {
            var source = await GetEventDetailsAsync(eventEditId);
            var response = Utils.Translate<EventResponse, EventDto>(source);
            if (response.IsSuccessful())
            {
                response.Title = source.Title;
                response.EditId = source.EditId;
                response.Address = source.Address;
                response.City = source.City;
                response.Description = source.Description;
                response.DiaryEditId = source.DiaryEditId;
                response.District = source.District;
                response.EndDate = source.EndDate;
                response.Image = source.Image;
                response.ImageThumbnail = source.ImageThumbnail;
                response.InfoLink = source.InfoLink;
                response.Location = source.Location;
                response.PostalCode = source.PostalCode;
                response.PricingInfo = source.PricingInfo;
                response.StartDate = source.StartDate;
                response.Summary = source.Summary;
                response.Tags = source.Tags != null && !string.IsNullOrEmpty(source.Tags.First()) ? source.Tags : null;
                response.X = source.X;
                response.Y = source.Y;
                response.DiaryName = source.DiaryName;
                response.Scope = source.Scope;
            }
            return response;
        }
        private async Task<EventDto> GetEventDetailsAsync(string eventEditId)
        {
            EventDto response = new EventDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.DIARY_SERVICES, Constants.DIARY_GET_EVENT_DETAILS);
                EventRequest r = new EventRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    EventEditId = eventEditId
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<EventDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
        
        public async Task<DiarySuggestionResponse> GetSuggestions(string query, DateTime startDate)
        {
            var source = await GetSuggestionsAsync(query, startDate);
            var response = Utils.Translate<DiarySuggestionResponse, DiarySuggestionDto>(source);
            if (response.IsSuccessful())
            {
                response.Results = source.Results;
            }
            return response;
        }
        private async Task<DiarySuggestionDto> GetSuggestionsAsync(string query, DateTime startDate)
        {
            DiarySuggestionDto response = new DiarySuggestionDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.DIARY_SERVICES, Constants.DIARY_GET_SUGGESTIONS);
                SuggestDiaryRequest r = new SuggestDiaryRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Request = query,
                    StartDate = startDate
                };
                string json = JsonConvert.SerializeObject(r);
                response = await _httpService.PostAsync<DiarySuggestionDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return response;
        }
    }
}
