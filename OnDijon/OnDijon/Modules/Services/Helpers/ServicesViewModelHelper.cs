using System.Collections.Generic;
using System.Linq;
using OnDijon.Modules.Services.Entities.Dto;
using OnDijon.Modules.Services.Entities.Models;

namespace OnDijon.Modules.Services.Helpers
{
    public static class ServicesViewModelHelper
    {
        public static List<ServiceLayout> TranslateToLayoutService(List<ServiceDto> services)
        {
            int row = 0, column = 0;
            
            var serviceAbris = new ServiceLayout()
            {
                Code = "ABRIS",
                Icon = "",
                Id = "ehbferbgs54",
                IsFavourite = false,
                IsRequiredConnection = false,
                MaintenanceMessage = "TestTry",
                StatusCode = System.Net.HttpStatusCode.OK,
                Title = "Abris",
                Visibility = "visible"
            };
            services.Add(serviceAbris);

            return services.Select(s =>
                                   {
                                       ServiceLayout serviceLayout = new ServiceLayout()
                                                                     {
                                                                         Code = s.Code,
                                                                         Icon = s.Icon,
                                                                         Id = s.Id,
                                                                         IsFavourite = s.IsFavourite,
                                                                         IsRequiredConnection = s.IsRequiredConnection,
                                                                         MaintenanceMessage = s.MaintenanceMessage,
                                                                         Message = s.Message,
                                                                         StatusCode = s.StatusCode,
                                                                         Title = s.Title,
                                                                         Visibility = s.Visibility
                                                                     };
                                       serviceLayout.Row = row;
                                       serviceLayout.Column = column % 2;
                                       column++;
                                       row = column % 2 == 0 ? row + 1 : row;
                                       return serviceLayout;
                                   }).ToList();
        }
    }
}