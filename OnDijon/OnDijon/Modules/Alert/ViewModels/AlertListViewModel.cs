using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Extensions;
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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.Alert.ViewModels
{

    public class AlertListViewModel : BaseViewModel
    {
        private readonly IAlertService _alertService;
        public ICommand DisplayAlertDetail { get; }
        public ICommand SeeAllAlerts { get; }

        private bool _seeAllAlertsVisible;
        public bool SeeAllAlertsVisible { get => _seeAllAlertsVisible; set => Set(ref _seeAllAlertsVisible, value); }

        private int _limit;
        public int Limit { get => _limit; set => Set(ref _limit, value); }

        private IList<AlertModel> _alertList;
        public IList<AlertModel> AlertList { get => _alertList; set => Set(ref _alertList, value); }

        public ICommand GoToScopeList { get; }

        public AlertListViewModel(INavigationService navigationService,
                                  ITranslationService translationService,
                                  IPopupService popupService,
                                  IAlertService alertService,
                                  ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {

            _alertService = alertService;

            SeeAllAlerts = new DelegateCommand(async () => await NavigateToAlertPage());
            DisplayAlertDetail = new DelegateCommand<AlertModel>(async (AlertModel param) => await GoToAlertDetailPage(param)) ;
            GoToScopeList = new Command(() => {
                NavigateTo(Locator.ScopesView);
            });
        }

        private async Task NavigateToAlertPage()
        {
            await NavigationService.NavigateTo(Locator.AlertPage);
        }

        private async Task GoToAlertDetailPage(AlertModel alert)
        {
            INavigationParameters param = new NavigationParameters
            {
                { Constants.AlertNavigationParameterKey, JsonConvert.SerializeObject(alert) }
            };
            await NavigationService.NavigateAsync(Locator.AlertDetailPage,param);
        }

        public void GetAlerts()
        {
            CallApi(async () =>
            {
                AlertListResponse response = await _alertService.GetAlerts();
                ManageApiResponses(response, new DefaultCallbackManager<AlertListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.Alerts.Any())
                        {
                            int take = Limit == 0 || Limit == -1 ? res.Alerts.Count() : Limit;
                            AlertList = new List<AlertModel>(res.Alerts).Take(take).ToList();
                            //App.Locator.Dashboard.AlertVisible = false;
                        }
                        else
                        {
                            AlertList = null;
                            //App.Locator.Dashboard.AlertVisible = false;
                        }
                    },
                });
                        //App.Locator.Dashboard.AlertVisible = true;
            });
        }

    }
}
