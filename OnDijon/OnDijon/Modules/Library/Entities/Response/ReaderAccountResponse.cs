using OnDijon.Modules.Library.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Response
{
    public class ReaderAccountResponse :  Common.Entities.Response.Response
    {
        public List<ReaderAccount> UserAccount { get; set; }
    }
}
