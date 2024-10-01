using OnDijon.Common.Entities.Response;
using OnDijon.Modules.UsefulContact.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.UsefulContact.Entities.Responses
{
    public class ContactDomainListResponse : Response
    {
        public IList<ContactDomainModel> ContactDomainList { get; set; }
    }
}
