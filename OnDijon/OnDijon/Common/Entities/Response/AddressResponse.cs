using OnDijon.Common.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Common.Entities.Response
{
    public class AddressResponse : Response
    {
        public List<AddressModel> AddressModel { get; set; }
    }
}
