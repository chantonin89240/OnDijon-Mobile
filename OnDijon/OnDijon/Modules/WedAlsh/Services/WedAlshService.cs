using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.Utils.Tools;
using OnDijon.Modules.WedAlsh.Entities.Dto;
using OnDijon.Modules.WedAlsh.Entities.Models;
using OnDijon.Modules.WedAlsh.Entities.Request;
using OnDijon.Modules.WedAlsh.Entities.Response;
using OnDijon.Modules.WedAlsh.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static OnDijon.Common.Utils.RecipeUIConstants;

namespace OnDijon.Modules.WedAlsh.Services
{
    class WedAlshService : IWedAlshService
    {

        readonly IHttpService _httpService;

        public WedAlshService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<WedAlshRegistrationsResponse> GetRegistrations(string profilEditId)
        {
            WedAlshRegistrationsDto sources = await GetRegistrationsAsync(profilEditId);

            WedAlshRegistrationsResponse response = Utils.Translate<WedAlshRegistrationsResponse, WedAlshRegistrationsDto>(sources);

            if (response.IsSuccessful())
            {
                response.Childs = new List<WedAlshChildModel>();
                int i = 0;
                sources.Childs.ForEach((c) => { 
                    var child = new WedAlshChildModel()
                    {
                        BirthDate = c.BirthDate,
                        EducatedInPrivateSchool = c.EducatedInPrivateSchool,
                        FirstName = c.FirstName,
                        Handicap = c.Handicap,
                        PersonNumber = c.PersonNumber,
                        LastName = c.LastName,
                        Meal = c.Meal,
                        SchoolLevel = c.SchoolLevel,
                        Civility = c.Civility,
                        ImageSource = c.Civility == "Madame" ? ImageTool.convertSourceImage(KidGirlAvatarSourceList.All[new Random().Next(0, KidGirlAvatarSourceList.All.Length - 1)]) : ImageTool.convertSourceImage(KidBoyAvatarSourceList.All[new Random().Next(0, KidBoyAvatarSourceList.All.Length - 1)]),
                        Color = CardTool.GetColor(i++, CardTool.ColorsPerisco)
                    };
                    child.School = new WedAlshSchoolModel()
                    {
                        EditId = c.School.EditId,
                        Title = c.School.Title
                    };
                    child.Registrations = new List<WedAshRegistrationDetailsModel>();
                    c.Registrations.ForEach(registration => {
                        WedAshRegistrationDetailsModel registrationToAdd = new WedAshRegistrationDetailsModel()
                        {
                            EditId = registration.EditId,
                            Schedules = new List<WedAlshScheduleModel>()
                        };
                        registrationToAdd.CentreAccueil = new WedAlshRecreationCenterModel()
                        {
                            EditId = registration.CentreAccueil.EditId,
                            Title = registration.CentreAccueil.Title
                        };
                        registration.Schedules.ForEach(schedule => {
                            registrationToAdd.Schedules.Add(new WedAlshScheduleModel()
                            {
                                EditId = schedule.EditId,
                                EndDate = schedule.EndDate,
                                IsAbsent = schedule.IsAbsent,
                                IsBooked = schedule.IsBooked,
                                IsClosed = schedule.IsClosed,
                                IsClosedLabel = schedule.IsClosedLabel,
                                ScheduleType = schedule.ScheduleType,
                                StartDate = schedule.StartDate,
                                State = schedule.State
                            });
                        });
                        child.Registrations.Add(registrationToAdd);
                    });
                    response.Childs.Add(child);
                });
            }
            return response;
        }

        private async Task<WedAlshRegistrationsDto> GetRegistrationsAsync(string profilEditId)
        {
            WedAlshRegistrationsDto _wedAlshRegistrations = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ALSH_SERVICES, Constants.ALSH_GET_REGISTRATIONS);

                WedAlshGetRegistrationRequest r = new WedAlshGetRegistrationRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ProfilEditId = profilEditId

                };
                var json = JsonConvert.SerializeObject(r);

                _wedAlshRegistrations = await _httpService.PostAsync<WedAlshRegistrationsDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _wedAlshRegistrations;
        }

        public async Task<WedAlshSchedulesResponse> UpdateRegistrations(string registrationEditId, List<ScheduleAction> changeList)
        {
            WedAlshSchedulesDto sources = await UpdateRegistrationsAsync(registrationEditId, changeList);

            WedAlshSchedulesResponse response = Utils.Translate<WedAlshSchedulesResponse, WedAlshSchedulesDto>(sources);

            if (response.IsSuccessful())
            {
                response.Schedules = new List<WedAlshScheduleModel>();
                sources.Schedules.ForEach(schedule => {
                    response.Schedules.Add(new WedAlshScheduleModel()
                    {
                        EditId = schedule.EditId,
                        EndDate = schedule.EndDate,
                        IsAbsent = schedule.IsAbsent,
                        IsBooked = schedule.IsBooked,
                        IsClosed = schedule.IsClosed,
                        IsClosedLabel = schedule.IsClosedLabel,
                        ScheduleType = schedule.ScheduleType,
                        StartDate = schedule.StartDate,
                        State = schedule.State
                    });
                });
            }
            return response;
        }

        private async Task<WedAlshSchedulesDto> UpdateRegistrationsAsync(string registrationEditId, List<ScheduleAction> changeList)
        {
            WedAlshSchedulesDto _possibleSchedules = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ALSH_SERVICES, Constants.ALSH_UPDATE_REGISTRATION);

                WedAlshUpdateRegistrationRequest r = new WedAlshUpdateRegistrationRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    RegistrationEditId = registrationEditId,
                    SchedulesChoice = changeList

                };
                var json = JsonConvert.SerializeObject(r);

                _possibleSchedules = await _httpService.PostAsync<WedAlshSchedulesDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _possibleSchedules;
        }

    }
}
