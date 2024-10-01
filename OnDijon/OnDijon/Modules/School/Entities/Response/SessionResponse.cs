using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.School.Entities.Response
{
    public class SessionResponse : Common.Entities.Response.Response
    {
        public IDictionary<string, string> SessionsEditIdByCityContext { get; set; }
    }
}
