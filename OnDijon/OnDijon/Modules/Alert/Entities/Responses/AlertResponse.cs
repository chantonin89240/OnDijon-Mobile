using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Alert.Entities.Models;

namespace OnDijon.Modules.Alert.Entities.Responses
{
    public class AlertResponse : Response
    {
        public AlertModel Alert { get; set; }
    }
}
