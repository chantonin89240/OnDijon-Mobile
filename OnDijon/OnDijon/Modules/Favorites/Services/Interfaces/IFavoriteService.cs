using OnDijon.Modules.Abris.Entities.Response;
using OnDijon.Modules.Demands.Entities.Responses;
using OnDijon.Modules.Favorites.Entities.Dto;
using OnDijon.Modules.Favorites.Entities.Models;
using OnDijon.Modules.Favorites.Entities.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OnDijon.Modules.Favorites.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task UpdateAddressAsync(FavoriteModel adress);

        Task<List<AddressDto>> GetFavoritesAsync(string idUser);
        Task DeleteFavAsync(FavoriteModel favorite);
        Task<FavoriteAddressesListResponse> GetFavorites(string idUser);
        Task<FavoriteAddressesListResponse> AddFavorite(Placemark placemark, string guid);

    }
}
