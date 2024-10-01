using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Alert.Entities.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDijon.Modules.Alert.Services.Interfaces
{
    public interface IAlertService
    {
        Task<AlertListResponse> GetAlerts();
        Task<Response> UpdateAlertReadStatus(IDictionary<string, bool> alertsToggleDictionary);
        Task<AlertResponse> GetAlertFromNotification(string senderAlertEditId);
    }
}