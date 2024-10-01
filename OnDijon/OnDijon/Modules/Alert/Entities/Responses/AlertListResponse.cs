using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Alert.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.Alert.Entities.Responses
{
    public class AlertListResponse : Response
    {
        public List<AlertModel> Alerts { get; set; }
    }
}
