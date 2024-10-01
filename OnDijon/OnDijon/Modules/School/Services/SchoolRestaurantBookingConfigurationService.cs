using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils;
using Newtonsoft.Json;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.SchoolServices.Interfaces;
using OnDijon.Modules.School.Entities.Response;
using OnDijon.Modules.School.Entities.Models;
using OnDijon.Modules.School.Entities.Dto;
using OnDijon.Modules.School.Entities.Request;
using Microsoft.AppCenter.Crashes;

namespace OnDijon.Modules.School.Services
{
    public class SchoolRestaurantBookingConfigurationService : ISchoolRestaurantBookingConfigurationService
    {
        IHttpService _httpService;
        private DateTime ChangeDateSession = new DateTime(2022, 6, 11, 9, 0, 0);

        public SchoolRestaurantBookingConfigurationService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<SessionResponse> GetActiveSessionByCityContext()
        {
            var sources = await GetActiveSessionByCityContextAsync();

            var response = Utils.Translate<SessionResponse, SessionsDto>(sources);

            if (response.IsSuccessful())
            {
                response.SessionsEditIdByCityContext = sources.SessionEditIdByCityContext.ToDictionary(d => d.Key, d => d.Value);
            }
            return response;
        }

        private async Task<SessionsDto> GetActiveSessionByCityContextAsync()
        {
            SessionsDto result = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.POP_SERVICES, Constants.POP_GET_ACTIVESESSIONBYCITYCONTEXT);
                SessionRequest data = new SessionRequest()
                {
                    Key = Constants.ONDIJON_KEY
                };
                string json = JsonConvert.SerializeObject(data);

                result = await _httpService.PostAsync<SessionsDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }


        public async Task<AppointmentListResponse> GetBookingList(string childEditId, string sessionEditId, string cityContext)
        {
            var sources = await GetChildAppointmentListAsync(childEditId, sessionEditId, cityContext);

            var response = Utils.Translate<AppointmentListResponse, AppointmentListDto>(sources);

            if (response.IsSuccessful())
            {
                response.childEditId = sources.childEditId;
                if (sources.Appointments != null)
                {
                    response.AppointmentList = sources.Appointments.Select(item =>
                    {
                        return new AppointmentModel()
                        {
                            ActivityEditId = item.ActivityEditId,
                            CalendarEditId = item.CalendarEditId,
                            RegistrationEditId = item.RegistrationEditId,
                            ActivityTitle = item.ActivityTitle,
                            Date = item.Date,
                            Scheduled = item.Scheduled,
                            ActivityCode = item.ActivityCode,
                            IsClosed = item.IsClosed,
                            IsModified = false,
                            SpecialDayLabel = item.SpecialDayLabel
                        };
                    }).ToList();
                }
            }
            return response;
        }

        private async Task<AppointmentListDto> GetChildAppointmentListAsync(string childEditId, string sessionEditId, string cityContext)
        {
            AppointmentListDto result = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.POP_SERVICES, Constants.POP_GET_CHILDAPPOINTMENTSCHEDULESBYCITYCONTEXT);
                AppointmentChildByContextRequest data = new AppointmentChildByContextRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ChildEditId = childEditId,
                    SessionEditId = sessionEditId,
                    CityContext = cityContext
                };
                string json = JsonConvert.SerializeObject(data);

                result = await _httpService.PostAsync<AppointmentListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }

        public async Task<WeeklyScheduleResponse> GetParentSchedule(string childEditId, string sessionEditId, string cityContext)
        {
            var sources = await GetParentScheduleAsync(childEditId, sessionEditId, cityContext);

            var response = Utils.Translate<WeeklyScheduleResponse, WeeklyScheduleDto>(sources);

            if (response.IsSuccessful())
            {
                response.StartDate = sources.StartDate;
                response.EndDate = sources.EndDate;
                response.EditId = sources.EditId;
                response.ChildEditId = childEditId;
                if (sources.Schedule != null)
                {
                    response.Schedule = sources.Schedule.Select(item =>
                    {
                        return new CalendarActivityModel()
                        {
                            EditId = item.EditId,
                            ActivityCode = item.ActivityCode,
                            ActivityDay = item.ActivityDay,
                            ActivityEditId = item.ActivityEditId,
                            ActivityTitle = item.ActivityTitle,
                            IsCheck = item.IsCheck,
                            Order = item.Order
                        };
                    }).ToList();
                }
            }
            return response;
        }

        private async Task<WeeklyScheduleDto> GetParentScheduleAsync(string childEditId, string sessionEditId, string cityContext)
        {
            WeeklyScheduleDto result = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.POP_SERVICES, Constants.POP_GET_PARENTSCHEDULEBYCITYCONTEXT);
                ParentScheduleRequest data = new ParentScheduleRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ChildEditId = childEditId,
                    SessionEditId = sessionEditId,
                    CityContext = cityContext
                };
                string json = JsonConvert.SerializeObject(data);

                result = await _httpService.PostAsync<WeeklyScheduleDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }


        public async Task<Response> SendBookingByCityContext(IList<AppointmentModel> datas, string cityContext)
        {
            WsDMDto sources = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.POP_SERVICES, Constants.POP_UPDATE_SCHEDULEDAPPOINTMENTBYCITYCONTEXT);
                var dtos = datas.Select(item =>
                {
                    return new AppointmentUpdateDto()
                    {
                        ActivityEditId = item.ActivityEditId,
                        CalendarEditId = item.CalendarEditId,
                        RegistrationEditId = item.RegistrationEditId,
                        Scheduled = item.Scheduled,
                    };
                }).ToList();
                AppointmentsUpdateByCityContextRequest data = new AppointmentsUpdateByCityContextRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    Appointments = dtos,
                    CityContext = cityContext
                };
                string json = JsonConvert.SerializeObject(data);
                sources = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return Utils.Translate<Response, WsDMDto>(sources);
        }

        public async Task<Response> UpdateParentScheduleByCityContext(WeeklyScheduleModel datas, string cityContext)
        {
            WsDMDto sources = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.POP_SERVICES, Constants.POP_UPDATE_PARENTSCHEDULEBYCITYCONTEXT);
                UpdateWeeklyScheduleByCityContextRequest request = new UpdateWeeklyScheduleByCityContextRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    StartDate = datas.StartDate,
                    EndDate = datas.EndDate,
                    EditId = datas.EditId,
                    CityContext = cityContext,
                };
                request.PlannedSchedule = datas.Schedule.Select(item =>
                {
                    return new CalendarActivityRequest()
                    {
                        EditId = item.EditId,
                        IsCheck = item.IsCheck
                    };
                }).ToList();
                string json = JsonConvert.SerializeObject(request);
                sources = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return Utils.Translate<Response, WsDMDto>(sources);
        }


        public async Task<ChildDietResponse> GetChildDietByCityContext(string id, string cityContext)
        {
            var sources = await GetChildDietAsync(id, cityContext);

            var response = Utils.Translate<ChildDietResponse, ChildDietDto>(sources);

            if (response.IsSuccessful())
            {
                response.HasOption = sources.HasOption;
                response.OptionDiet = sources.OptionDiet;
                response.PossibleStandardDiets = sources.PossibleStandardDiets;
                response.StandardDiet = sources.StandardDiet;
            }
            return response;
        }

        private async Task<ChildDietDto> GetChildDietAsync(string id, string cityContext)
        {
            ChildDietDto result = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.POP_SERVICES, Constants.POP_GET_CHILD_DIET_BYCITYCONTEXT);
                ChildDietRequest data = new ChildDietRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ChildEditId = id,
                    CityContext = cityContext
                };
                string json = JsonConvert.SerializeObject(data);

                result = await _httpService.PostAsync<ChildDietDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }


        public async Task<Response> UpdateChildDietByCityContext(string id, string standardDiet, bool option, string cityContext)
        {
            var sources = await UpdateChildDietAsync(id, standardDiet, option, cityContext);

            var response = Utils.Translate<Response, WsDMDto>(sources);

            return response;
        }

        private async Task<WsDMDto> UpdateChildDietAsync(string id, string standardDiet, bool option, string cityContext)
        {
            WsDMDto result = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.POP_SERVICES, Constants.POP_UPDATE_CHILD_DIET_BYCITYCONTEXT);
                UpdateChildDietRequest data = new UpdateChildDietRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ChildEditId = id,
                    StandardDiet = standardDiet,
                    Option = option,
                    CityContext = cityContext
                };
                string json = JsonConvert.SerializeObject(data);

                result = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return result;
        }
    }
}