using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Alert.Entities.Models;
using OnDijon.Modules.Alert.Entities.Responses;
using OnDijon.Modules.Alert.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnDijon.Modules.Alert.ViewModels
{
    public class AlertDetailViewModel : BaseViewModel
    {
        private readonly IAlertService _alertService;
        private AlertModel _alert;

        public AlertModel Alert { get => _alert; set => Set(ref _alert, value); }

        public ICommand MaskAsUnReadCommand { get; }

        public AlertDetailViewModel(INavigationService navigationService,
                                    ITranslationService translationService,
                                    IPopupService popupService,
                                    IAlertService alertService,
                                    ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _alertService = alertService;
            MaskAsUnReadCommand = new DelegateCommand(MarkAsUnRead);
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            if(parameters.TryGetValue(Constants.NotificationItemNavigationKey, out string alertSenderId))
                InitAlertFromNotification(alertSenderId);
            else if(parameters.TryGetValue(Constants.AlertNavigationParameterKey,out string alertJson))
            {
                var alert = JsonConvert.DeserializeObject<AlertModel>(alertJson);
                Alert = alert;
            }
            MarkCurrentStatusReadingAs(true);
        }

        public void MarkAsUnRead()
        {
            MarkCurrentStatusReadingAs(false);
            NavigationService.GoBackAsync();
        }

        internal void MarkCurrentStatusReadingAs(bool isReadStatus)
        {
            if (Alert != null)
            {
                UpdateReadStatus(new Dictionary<string, bool>() { { Alert.EditId, isReadStatus } }, isReadStatus);
            }
        }

        internal void UpdateReadStatus(IDictionary<string, bool> alertsToggleDictionary, bool finalStatus)
        {
            CallApi(async () =>
            {
                Response response = await _alertService.UpdateAlertReadStatus(alertsToggleDictionary);
                ManageApiResponses(response, new DefaultCallbackManager<Response>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.IsSuccessful())
                        {
                            Alert.IsRead = finalStatus;
                        }
                    },
                });
            });
        }

        internal void InitAlertFromNotification(string alertSenderEditId)
        {
            CallApi(async () =>
            {
                AlertResponse response = await _alertService.GetAlertFromNotification(alertSenderEditId);
                ManageApiResponses(response, new DefaultCallbackManager<AlertResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.Alert != null)
                        {
                            Alert = res.Alert;
                            MarkCurrentStatusReadingAs(true);
                        }
                    },
                });
            });
        }




    }
}
