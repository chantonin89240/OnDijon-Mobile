using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Abris.Entities.Dto;
using OnDijon.Modules.Abris.Entities.Models;
using OnDijon.Modules.Abris.Entities.Response;
using OnDijon.Modules.Abris.Serv.Interfaces;
using OnDijon.Modules.Favorites.Entities.Dto;
using OnDijon.Modules.Favorites.Entities.Models;
using OnDijon.Modules.Favorites.Entities.Response;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OnDijon.Modules.Abris.Serv
{
    class AbrisService : IAbrisService
    {
        readonly IHttpService _httpService;

        public AbrisService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<AbrisListResponse> GetAbris()
        {
            List<AbrisDto> sources = await GetAbrisAsync();

            List<ShelterStateDto> sourcesShelter = await GetShelterStatesAsync();

            var response = new AbrisListResponse();
            var abrisModel = new List<AbrisModel>();
            sources.ForEach(item =>
            {
                abrisModel.Add(new AbrisModel()
                {
                    DataSetId = item.DataSetId,
                    RecordId = item.RecordId,
                    Extensible = item.Extensible,
                    Quartier = item.Quartier,
                    Nom = item.Nom,
                    Aire = item.Aire,
                    GeoPointLat = item.GeoPointLat,
                    GeoPointLon = item.GeoPointLon,
                    NbPlaces = int.Parse(sourcesShelter.LastOrDefault(x=>x.IdAbris == item.RecordId).Available),
                    NbPlacesInitial = item.NbPlacesInitial,
                    CodComm = item.CodComm,
                });
            });
            response.AbrisList = abrisModel;

            return response;
        }

        public async Task<List<AbrisDto>> GetAbrisAsync()
        {
            List<AbrisDto> _AbrisList = new List<AbrisDto>();
            try
            {
                _AbrisList = await _httpService.GetBisAsync<List<AbrisDto>>(Constants.API_DIIAGE + Constants.Abri, HttpMethod.Get);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return _AbrisList;
        }

        public async Task<List<ShelterStateDto>> GetShelterStatesAsync()
        {
            List<ShelterStateDto> _SheltersList = new List<ShelterStateDto>();
            try
            {
                _SheltersList = await _httpService.GetBisAsync<List<ShelterStateDto>>(Constants.API_DIIAGE + Constants.ShelterState, HttpMethod.Get);

            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return _SheltersList;
        }

    }
}
