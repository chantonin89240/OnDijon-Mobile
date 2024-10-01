using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using OnDijon.Modules.School.Entities.Dto;
using OnDijon.Modules.School.Entities.Response;
using OnDijon.Modules.School.Models;
using OnDijon.Common.Utils;
using OnDijon.Modules.School.Request;
using OnDijon.Modules.School.Services.Interfaces;
using OnDijon.Common.Utils.Services.Interfaces;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Entities;

namespace OnDijon.Modules.School.Services
{
    class RSCalendarService : IRSCalendar
    {

        readonly IHttpService _httpService;

        public RSCalendarService(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<SchoolRestaurantCalendarListResponse> GetMenusList()
        {
            var sources = await GetMenusAsync();

            var response = Utils.Translate<SchoolRestaurantCalendarListResponse, SchoolRestaurantCalendarListDto>(sources);

            if (response.IsSuccessful())
            {
                response.SchoolRestaurantCalendarList = sources.SchoolRestaurantCalendarDays.Select(item =>
                {
                    return new SchoolRestaurantCalendar()
                    {
                        Id = string.IsNullOrEmpty(item.Id) ? "" : item.Id,
                        Nom = item.Nom,
                        Date = item.Date,
                        Ferie = item.Ferie,
                        Entree = new OnDijon.Modules.School.Models.Plat()
                        {
                            Nom = item.Entree.Nom,
                            Bio = item.Entree.Bio,
                            Porc = item.Entree.Porc,
                            ProduitLocaux = item.Entree.ProduitLocaux,
                            CommerceEquitable = item.Entree.CommerceEquitable,
                        },
                        Legume = new OnDijon.Modules.School.Models.Plat()
                        {
                            Nom = item.Legume.Nom,
                            Bio = item.Legume.Bio,
                            Porc = item.Legume.Porc,
                            ProduitLocaux = item.Legume.ProduitLocaux,
                            CommerceEquitable = item.Legume.CommerceEquitable,
                        },
                        Proteine = new OnDijon.Modules.School.Models.Plat()
                        {
                            Nom = item.Proteine.Nom,
                            Bio = item.Proteine.Bio,
                            Porc = item.Proteine.Porc,
                            ProduitLocaux = item.Proteine.ProduitLocaux,
                            CommerceEquitable = item.Proteine.CommerceEquitable,
                        },
                        Fromage = new OnDijon.Modules.School.Models.Plat()
                        {
                            Nom = item.Fromage.Nom,
                            Bio = item.Fromage.Bio,
                            Porc = item.Fromage.Porc,
                            ProduitLocaux = item.Fromage.ProduitLocaux,
                            CommerceEquitable = item.Fromage.CommerceEquitable,
                        },
                        Dessert = new OnDijon.Modules.School.Models.Plat()
                        {
                            Nom = item.Dessert.Nom,
                            Bio = item.Dessert.Bio,
                            Porc = item.Dessert.Porc,
                            ProduitLocaux = item.Dessert.ProduitLocaux,
                            CommerceEquitable = item.Dessert.CommerceEquitable,
                        },
                    };
                }).ToList();
            }
            return response;
        }


        

        private async Task<SchoolRestaurantCalendarListDto> GetMenusAsync()
        {
            SchoolRestaurantCalendarListDto _allMenus = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.RS_SERVICES, Constants.RS_GETDATA);

                var startDay = DateTime.Today.AddDays(-2);
                var endDate = DateTime.Today.AddDays(28);

                SchoolRestaurantCalendarRequest r = new SchoolRestaurantCalendarRequest()
                {
                    Key = Constants.ONDIJON_KEY,

                    //TODO : Change dates
                    BeginningDate = new DateTime(startDay.Year, startDay.Month, startDay.Day),
                    EndingDate = new DateTime(endDate.Year, endDate.Month, endDate.Day)

                };
                var json = JsonConvert.SerializeObject(r);

                _allMenus = await _httpService.PostAsync<SchoolRestaurantCalendarListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _allMenus;
        }

        
    }
}
