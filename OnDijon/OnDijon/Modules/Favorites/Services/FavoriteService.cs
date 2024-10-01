using BruTile.Wms;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Abris.Entities.Response;
using OnDijon.Modules.Demands.Entities.Dto;
using OnDijon.Modules.Demands.Entities.Responses;
using OnDijon.Modules.Favorites.Entities.Dto;
using OnDijon.Modules.Favorites.Entities.Models;
using OnDijon.Modules.Favorites.Entities.Response;
using OnDijon.Modules.Favorites.Services.Interfaces;
using OnDijon.Modules.JobOffer.Entities.Dto;
using OnDijon.Modules.JobOffer.Entities.Models;
using OnDijon.Modules.JobOffer.Entities.Requests;
using OnDijon.Modules.JobOffer.Entities.Responses;
using OnDijon.Modules.UsefulContact.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Exception = System.Exception;

namespace OnDijon.Modules.Favorites.Services
{
    class FavoriteService : IFavoriteService
    {
        readonly IHttpService _httpService;

        public FavoriteService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<FavoriteAddressesListResponse> GetFavorites(string idUser)
        {
            List<AddressDto> sources = await GetFavoritesAsync(idUser);

            var response = new FavoriteAddressesListResponse();
            var favorisModel = new List<FavoriteModel>();
            sources.ForEach(item =>
            {
                favorisModel.Add(new FavoriteModel()
                {
                    Id = item.Id,
                    ProfilId = item.ProfilId,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    CodePostal = item.CodePostal,
                    Ville = item.Ville,
                    Rue = item.Rue,
                    Pays = item.Pays,
                });
            });
            response.FavAdressesList = favorisModel;

            return response;
        }

        public async Task<List<AddressDto>> GetFavoritesAsync(string idUser)
        {
            List<AddressDto> _AdressList = new List<AddressDto>();
            try
            {
                FavoriteAddressesListResponse data = new FavoriteAddressesListResponse();
                string json = JsonConvert.SerializeObject(data);
                 _AdressList = await _httpService.GetAsync<List<AddressDto>>(Constants.API_DIIAGE + Constants.Favoris + Constants.byGuid + idUser, HttpMethod.Get, json);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
            return _AdressList;
        }
        public async Task UpdateAddressAsync(FavoriteModel address)
        {
            try
            {
                string json = JsonConvert.SerializeObject(address);
                await _httpService.PutAsync<AdressDto>(Constants.API_DIIAGE + Constants.Favoris ,  HttpMethod.Put, json);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }
        }

        public async Task DeleteFavAsync(FavoriteModel favorite)
        {
            await _httpService.DeleteAsync<FavoriteModel>(Constants.API_DIIAGE + Constants.Favoris + favorite.Id, HttpMethod.Delete, null);
        }

        public async Task<FavoriteAddressesListResponse> AddFavorite(Placemark placemark, string guid)
        {
            AddressDto sources = await AddFavoriteAsync(placemark, guid);

            var response = new FavoriteAddressesListResponse();
            if (sources != null)
            {
                var favorisModel = new List<FavoriteModel>
                {
                    new FavoriteModel()
                    {
                        Id = sources.Id,
                        ProfilId = sources.ProfilId,
                        Latitude = sources.Latitude,
                        Longitude = sources.Longitude,
                        CodePostal = sources.CodePostal,
                        Ville = sources.Ville,
                        Rue = sources.Rue,
                        Pays = sources.Pays,
                    }
                };
                response.FavAdressesList = favorisModel;
            }

            return response;
        }

        public async Task<AddressDto> AddFavoriteAsync(Placemark placemark, string guid)
        {
            AddressDto _favoriteAddress = new AddressDto();
            
            try { 
                ProfilModel user = await GetUser(guid); 
                if (user != null)
                {
                    string body = BuildRequestBody(placemark, user.Id);
                    _favoriteAddress = await _httpService.PostBisAsync<AddressDto>(Constants.API_DIIAGE + Constants.Favoris, HttpMethod.Post, body);
                }
            } catch (Exception ex) { Console.WriteLine(ex.Message); }

            return _favoriteAddress;
        }

        public async Task<ProfilModel> GetUser(string guid)
        {
            List<ProfilDto> sources = await GetUsersAsync();

           
            var profilModelList = new List<ProfilModel>();
            sources.ForEach(item =>
            {
                profilModelList.Add(new ProfilModel()
                {
                    Id = item.Id,
                    Nom = item.Nom, 
                    Prenom = item.Prenom, 
                    Guid = item.Guid, 
                });
            });

            ProfilModel user = profilModelList.FirstOrDefault(x => x.Guid == guid);
            return user;
        }

        public async Task<List<ProfilDto>> GetUsersAsync()
        {
            List<ProfilDto> _UsersList = new List<ProfilDto>();
            try
            {

                _UsersList = await _httpService.GetBisAsync<List<ProfilDto>>(Constants.API_DIIAGE + Constants.Profil, HttpMethod.Get);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
                Crashes.TrackError(ex);
            }


            return _UsersList;
        }

        private string BuildRequestBody(Placemark placemark, int id)
        {
            string Ville = "";
            string Rue = "";
            string Pays = "";
            int CodePostal = 0;

            if (placemark != null)
            {

                if (!string.IsNullOrEmpty(placemark.FeatureName))
                {
                    Rue = placemark.FeatureName+" ";
                }
                if (!string.IsNullOrEmpty(placemark.Thoroughfare))
                {
                    Rue += placemark.Thoroughfare;
                }

                if (!string.IsNullOrEmpty(placemark.Locality))
                {
                    Ville = placemark.Locality;
                }

                if (!string.IsNullOrEmpty(placemark.PostalCode))
                {
                    CodePostal = int.Parse(placemark.PostalCode, 0);
                }

                if (!string.IsNullOrEmpty(placemark.CountryName))
                {
                    Pays = placemark.CountryName;
                }
            }

            FavoriteModel resource = new FavoriteModel() { ProfilId = id, Latitude = placemark.Location.Latitude, Longitude = placemark.Location.Longitude, CodePostal = CodePostal, Ville = Ville, Rue = Rue, Pays = Pays };
            return JsonConvert.SerializeObject(resource);
        }
    }
}
