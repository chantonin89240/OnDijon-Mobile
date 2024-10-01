using System;
using System.Threading.Tasks;
using OnDijon.Common.Utils;
using Newtonsoft.Json;
using System.Linq;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.School.Entities.Models;
using OnDijon.Modules.School.Entities.Response;
using OnDijon.Modules.SchoolServices.Interfaces;
using OnDijon.Modules.School.Entities.Dto;
using OnDijon.Modules.School.Entities.Request;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Utils.Tools;
using static OnDijon.Common.Utils.RecipeUIConstants;
using System.Collections.Generic;

namespace OnDijon.Modules.School.Services
{
    public class SchoolCardConfigurationService : ISchoolCardConfigurationService
    {
        readonly IHttpService _httpService;
        private DateTime ChangeDateSession = new DateTime(2022, 6, 11, 9, 0, 0);

        public SchoolCardConfigurationService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<ChildResponse> GetChilds(string id, IDictionary<string, string> sessionEditIdCityContext)
        {
            var sources = await GetChildRawListDto(id, sessionEditIdCityContext);

            var response = Utils.Translate<ChildResponse, ChildListDto>(sources);

            if (response.IsSuccessful())
            {
                int i = 0;
                response.SchoolCardList = sources.Childs.Select(item =>
                {
                    return new ChildCardModel()
                    {
                        Type = SchoolCardType.Child,
                        Color = CardTool.GetColor(i++, CardTool.ColorsPerisco),
                        ChildModel = new ChildByCityModel()
                        {
                            EditId = item.EditId,
                            PersonNumber = item.PersonNumber,
                            Nom = item.Surname,
                            Prenom = item.Name,
                            Civility = item.Civility,
                            Birthday = item.Birthday,
                            ImageSource = item.Civility == "Madame" ? ImageTool.convertSourceImage(KidGirlAvatarSourceList.All[new Random().Next(0, KidGirlAvatarSourceList.All.Length - 1)]) : ImageTool.convertSourceImage(KidBoyAvatarSourceList.All[new Random().Next(0, KidBoyAvatarSourceList.All.Length - 1)]),
                            CityContext = item.CityContext,
                            SessionEditId = item.SessionEditId,
                        }
                    };
                }).ToList();
                response.SchoolCardList.Add(new ChildCardModel() { Color = CardTool.GetColor(i++, CardTool.ColorsPerisco), Type = SchoolCardType.Restaurant });
                response.SessionScheduledHelper = sources.SessionScheduledHelper;

            }
            return response;
        }

        private async Task<ChildListDto> GetChildRawListDto(string profilEditId, IDictionary<string, string> sessionEditIdCityContext)
        {
            ChildListDto schoolRestaurantChildListDto = null;
            try
            {
                schoolRestaurantChildListDto = new ChildListDto();
                string url = string.Concat(Constants.API_URL, Constants.POP_SERVICES, Constants.POP_GET_CHILDSBYCITYCONTEXT);
                ChildRequest data = new ChildRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ProfileEditId = profilEditId,
                    SessionEditIdCityContext = sessionEditIdCityContext.Select(d => new Dictionary() { Key = d.Key, Value = d.Value }).ToList()
                };
                string json = JsonConvert.SerializeObject(data);
                schoolRestaurantChildListDto = await _httpService.PostAsync<ChildListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return schoolRestaurantChildListDto;
        }
    }
}