using OnDijon.Modules.Abris.Entities.Dto;
using OnDijon.Modules.Abris.Entities.Response;
using OnDijon.Modules.Favorites.Entities.Models;
using OnDijon.Modules.Favorites.Entities.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace OnDijon.Modules.Abris.Serv.Interfaces
{
    public interface IAbrisService
    {
        Task<AbrisListResponse> GetAbris();
    }
}
