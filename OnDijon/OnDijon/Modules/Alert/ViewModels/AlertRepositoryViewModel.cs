using System.Threading.Tasks;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using Prism.Navigation;

namespace OnDijon.Modules.Alert.ViewModels
{
    public class AlertRepositoryViewModel : BaseViewModel
    {
        private AlertListViewModel _alertListViewModel;
        public AlertListViewModel AlertListViewModel { get => _alertListViewModel; set => Set(ref _alertListViewModel, value); }

        public AlertRepositoryViewModel(INavigationService navigationService,
                                        ITranslationService translationService,
                                        IPopupService popupService,
                                        ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            AlertListViewModel = App.Locator.GetInstance<AlertListViewModel>();         
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            AlertListViewModel.Limit = -1;
            AlertListViewModel.SeeAllAlertsVisible = false;
            AlertListViewModel.GetAlerts();
        }
    }
}