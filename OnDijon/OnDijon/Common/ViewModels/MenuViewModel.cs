using AsyncAwaitBestPractices.MVVM;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Extensions;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Enums;
using OnDijon.Modules.Account.Pages;
using OnDijon.Modules.Account.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace OnDijon.Common.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        readonly ISession _session;
        readonly IAccountService _accountService;

        public bool IsProfilVisible => _session.IsConnected();

        public bool IsConnected => _session.IsConnected();

        public string FirstName => _session.Profile?.FirstName;

#if DEBUG || STAGING
        public string Version => $"V {AppInfo.VersionString} PP";
#else
        public string Version => $"V {AppInfo.VersionString}";
#endif

        public string LoginText => _session.IsConnected() ? "Se déconnecter" : "Se connecter";

        public ICommand CloseMenu { get; private set; }

        public ICommand GoToProfileCommand { get; private set; }

        public ICommand GoToDashboardCommand { get; private set; }

        public ICommand GoToServicesCommand { get; private set; }

        public ICommand GoToScopesCommand { get; private set; }

        public ICommand GoToMySpaceCommand { get; private set; }

        public ICommand GoToManageFavoritesCommand { get; private set; }

        public ICommand LoginLogoutCommand { get; private set; }

        public ICommand ContactSupportCommand { get; private set; }

        public ICommand GoToLegalNoticeCommand { get; private set; }

        public ICommand GoToAlertsCommand { get; private set; }

        public ICommand GoToAgendaCommand { get; private set; }


        public MenuViewModel(INavigationService navigationService,
                             ITranslationService translationService,
                             IPopupService popupService,
                             ISession session,
                             IAccountService accountService, ILoggerService loggerService )
            : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _accountService = accountService;

            InitializeCommands();
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            InitializeCommands();
        }


        private async Task ManageLogin()
        {
            if (_session.IsConnected())
            {
                Logout();
            }
            else
            {
                await Login();
            }
        }

        private async Task Login()
        {
            await NavigationService.NavigateTo(Locator.LoginPage, true);
        }

        private void Logout()
        {
            PopupService.Show(PopupEnum.PopupInfo, "Etes-vous sûr de vouloir vous déconnecter ?", "OK", () =>
            {

                CallApi(async () =>
                {
                    Response response = await _accountService.Disconnect(_session.Profile.Guid);
                    ManageApiResponses(response, new CallbackManager<Response>
                    {
                        OnSuccess = async (res) =>
                        {
                            await _accountService.Disconnect();
                            RaisePropertyChanged(nameof(IsProfilVisible));
                            RaisePropertyChanged(nameof(LoginText));
                            await NavigationService.GoBackToPageKey(Locator.DashboardView);
                        },
                        OnError = (res) =>
                        {
                            PopupService.Show(PopupEnum.PopupError, "Une erreur est survenue", response.Message, "Retour");
                        },
                        OnInvalidInformations = async (res) =>
                        {
                            await _accountService.Disconnect();
                            RaisePropertyChanged(nameof(IsProfilVisible));
                            RaisePropertyChanged(nameof(LoginText));
                            await NavigationService.GoBackToPageKey(Locator.DashboardView);
                        },
                    });
                });

            }, "Annuler");
        }

        private async Task DisplayCgu()
        {
            await PopupNavigation.Instance.PushAsync(new CguPopupView());
        }


        public void InitializeCommands()
        
        {
            CloseMenu = new AsyncCommand(NavigationService.GoBackAsync);

            GoToDashboardCommand = new DelegateCommand(async () => { await NavigationService.GoBackToPageKey(Locator.DashboardView); });

            GoToProfileCommand = new DelegateCommand(async () => { await NavigationService.NavigateTo(Locator.ProfileView, true); });

            LoginLogoutCommand = new DelegateCommand(async () => { await ManageLogin(); });

            GoToServicesCommand = new DelegateCommand(async () => { await NavigationService.NavigateTo(Locator.ServicesView, true); });

            GoToScopesCommand = new DelegateCommand(async () => { await NavigationService.NavigateTo(Locator.ScopesView, true); });

            GoToMySpaceCommand = new DelegateCommand(async () => { await NavigationService.NavigateTo(Locator.DemandListPage, true); });

            GoToAlertsCommand = new DelegateCommand(async () => { await NavigationService.NavigateTo(Locator.AlertPage, true); });

            GoToManageFavoritesCommand = new DelegateCommand(async () => { await NavigationService.NavigateTo(Locator.FavoritesPage, true); });

            GoToAgendaCommand = new DelegateCommand(async () => { await NavigationService.NavigateTo(Locator.EventListPage, true); });

            GoToLegalNoticeCommand = new DelegateCommand(async () => { await DisplayCgu(); });

            // TODO: à Modifier (surement page webview ou popup)
            ContactSupportCommand = new DelegateCommand(async () =>
            {
                var mailBody = $@"Informations techniques (ne pas supprimer) :
OS : {DeviceInfo.Platform} {DeviceInfo.VersionString}
Modèle : {DeviceInfo.Manufacturer} {DeviceInfo.Model}
Version de l'application : {AppInfo.VersionString}
";
                await Launcher.OpenAsync($"mailto:{Constants.CONTACT_EMAIL}?body={Uri.EscapeDataString(mailBody)}");
            });
        }

    }
}
