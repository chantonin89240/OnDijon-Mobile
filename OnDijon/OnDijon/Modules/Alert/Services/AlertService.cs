using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Alert.Entities.Dto;
using OnDijon.Modules.Alert.Entities.Requests;
using OnDijon.Modules.Alert.Entities.Responses;
using OnDijon.Modules.Alert.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnDijon.Modules.Alert.Entities.Models;
using System.Linq;
using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Account.Services.Interfaces;

namespace OnDijon.Modules.Alert.Services
{
    public class AlertService : IAlertService
    {
        readonly IHttpService _httpService;
        readonly ISession _session;

        public AlertService(ISession session, IHttpService httpService)
        {
            _session = session;
            _httpService = httpService;
        }

        public async Task<AlertResponse> GetAlertFromNotification(string senderAlertEditId)
        {
            AlertResultDto sources = await GetAlertFromNotificationAsync(senderAlertEditId);

            var response = Utils.Translate<AlertResponse, AlertResultDto>(sources);
            if (response.IsSuccessful())
            {
                response.Alert = new AlertModel()
                {
                    Content = sources.Alert.Content,
                    EditId = sources.Alert.EditId,
                    ExternalLink = sources.Alert.ExternalLink,
                    Image = sources.Alert.Image,
                    IsRead = sources.Alert.IsRead,
                    Scope = sources.Alert.Scope,
                    SendingDate = sources.Alert.SendingDate,
                    SubTitle = sources.Alert.SubTitle,
                    Title = sources.Alert.Title,
                    NavigationLink = sources.Alert.NavigationLink
                };
            }
            return response;
        }
        public async Task<AlertResultDto> GetAlertFromNotificationAsync(string senderAlertEditId)
        {
            AlertResultDto _alert = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ALERT_SERVICES, Constants.ALERT_GET_ALERT_BY);

                AlertByUserAndAlertSenderRequest r = new AlertByUserAndAlertSenderRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    UserEditId = _session.Profile.Guid,
                    SenderAlertEditId = senderAlertEditId,
                };
                _alert = await _httpService.PostAsync<AlertResultDto>(new Uri(url), JsonConvert.SerializeObject(r)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _alert;
        }

        public async Task<AlertListResponse> GetAlerts()
        {
            AlertListDto sources = await GetAlertsAsync();

            var response = Utils.Translate<AlertListResponse, AlertListDto>(sources);
            if (response.IsSuccessful())
            {
                response.Alerts = sources.Alerts.Select(item => new AlertModel()
                {
                    Title = item.Title,
                    Content = item.Content,
                    EditId = item.EditId,
                    Image = item.Image,
                    IsRead = item.IsRead,
                    SendingDate = item.SendingDate,
                    Scope = item.Scope,
                    SubTitle = item.SubTitle,
                    NavigationLink = item.NavigationLink
                }).ToList();

            }
            return response;
        }

        public async Task<AlertListDto> GetAlertsAsync()
        {
            AlertListDto _alertList = null;
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ALERT_SERVICES, Constants.ALERT_GET_ALERTS);

                AlertRequest r = new AlertRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    UserEditId = _session.Profile.Guid
                };
                _alertList = await _httpService.PostAsync<AlertListDto>(new Uri(url), JsonConvert.SerializeObject(r)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _alertList;
        }

        public async Task<Response> UpdateAlertReadStatus(IDictionary<string, bool> alertsToggleDictionary)
        {
            WsDMDto sources = await UpdateAlertReadStatusAsync(alertsToggleDictionary.Select(kv => new AlertsToggleDictionary() { EditId = kv.Key, ReadStatus = kv.Value}).ToList());
            return Utils.Translate<Response, WsDMDto>(sources);
        }

        public async Task<WsDMDto> UpdateAlertReadStatusAsync(List<AlertsToggleDictionary> alertsToggleDictionary)
        {
            WsDMDto _res = new WsDMDto();
            try
            {
                string url = string.Concat(Constants.API_URL, Constants.ALERT_SERVICES, Constants.ALERT_UPDATE_READ_STATUS);

                AlertReadRequest r = new AlertReadRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    UserEditId = _session.Profile.Guid,
                    AlertsToggleDictionary = alertsToggleDictionary
                };
                _res = await _httpService.PostAsync<WsDMDto>(new Uri(url), JsonConvert.SerializeObject(r)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return _res;
        }

    }
}
