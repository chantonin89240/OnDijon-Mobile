using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.JobOffer.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Favorites.Entities.Dto
{
    public class AddressListDto : WsDMDto
    {
        public List<AddressDto> Adresses;
    }
}
