using OnDijon.Modules.Account.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.Account.Entities.Response
{
    public class GetAccountResponse : Common.Entities.Response.Response
    {
        public ProfileModel Profile { get; set; }

        public IList<MobileRegistrationModel> MobileRegistrations { get; set; }
    }
}
