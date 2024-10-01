using OnDijon.Common.Entities.Response;
using OnDijon.Modules.UsefulContact.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.UsefulContact.Entities.Responses
{
    public class ContactListResponse : Response
    {
        public IList<ContactDto> ContactList { get; set; }
    }
}
