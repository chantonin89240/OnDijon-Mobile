using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Strike.Entities.Model;

namespace OnDijon.Modules.Strike.Entities.Responses
{
    public class SessionStrikeResponse : Response
    {
        public SessionStrikeModel SessionStrike { get; set; }
    }
}
