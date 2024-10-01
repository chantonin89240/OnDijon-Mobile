using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils.Helpers;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Pages;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Extensions;
using OnDijon.Modules.Notifications.Services.Interfaces;
using OnDijon.Common.Permissions;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Command;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.Utils.Validations;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using OnDijon.Modules.Account.Entities.Response;
using OnDijon.Modules.Account.Entities.Models;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.Account.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICacheService _cacheService;
        private readonly ISession _session;
        private readonly INotificationService _notificationService;

        public ICommand LoginCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; }
        public ICommand CreateAccountCommand { get; set; }
        public ICommand SkipLoginCommand { get; set; }
        public ICommand ToggleIsPasswordCommand { get; set; }

        public ValidatableObject<string> Email { 
            get; 
            set; 
        } = new ValidatableObject<string>();

        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();

        public bool CanLogin => !string.IsNullOrEmpty(Email.Value) && !string.IsNullOrEmpty(Password.Value);

        private bool _isPassword = true;
        public bool IsPassword { get => _isPassword; set => Set(ref _isPassword, value); }


        public LoginViewModel(INavigationService navigationService,
                              IAccountService accountService,
                              ITranslationService translationService,
                              IPopupService popupService,
                              ICacheService cacheService,
                              ISession session,
                              INotificationService notificationService,
                              ILoggerService loggerService ) 
	        : base(navigationService, translationService, popupService, loggerService)
        {
            _accountService = accountService;
            _cacheService = cacheService;
            _session = session;
            _notificationService = notificationService;

            LoginCommand = new AsyncBlockingCommand(async () => { await AuthenticateAsync(); });

            ResetPasswordCommand = new DelegateCommand(() =>
            {
                NavigationService.NavigateTo(Locator.ResetPasswordView);
            });

            CreateAccountCommand = new DelegateCommand(() =>
            {
                NavigationService.NavigateTo(Locator.SignUpView);
            });

            SkipLoginCommand = new AsyncBlockingCommand(async () =>
            {
                await CheckCguAccepted();
            });

            ToggleIsPasswordCommand = new DelegateCommand(() => IsPassword = !IsPassword);

            AddValidations();
        }

        public override async Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedFromAsync(parameters);
	        Cleanup();
        }


        public override void Cleanup()
        {
            base.Cleanup();
            Password.Value = string.Empty;
        }

        private void AddValidations()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Email requis" });
            Email.Validations.Add(new PredicateRule<string> { ValidationMessage = "Email incorrect", Predicate = (value) => RegexHelper.CheckEmailRegex(value.Trim()) });
            Password.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Mot de passe requis" });

            Email.PropertyChanged += OnPropertyChanged;
            Password.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RaisePropertyChanged(nameof(CanLogin));
            }
        }


        private async Task AuthenticateAsync()
        {
            if (Email.Validate() && Password.Validate())
            {
                AuthenticationResponse response = await _accountService.Authenticate(Email.Value.Trim(), Password.Value);

                ManageApiResponses(response, new CallbackManager<AuthenticationResponse>
                {
                    OnSuccess = AuthenticateOnSuccess,
                    OnInvalidCredentials = (res) =>
                    {
                        PopupService.Show(PopupEnum.PopupError, "Erreur d'identification", res.Message, "Retour");
                    },
                    OnInvalidInformations = (res) =>
                    {
                        PopupService.Show(PopupEnum.PopupError, "Erreur d'identification", res.Message, "Retour");
                    }
                });
            }
        }

        void AuthenticateOnSuccess(AuthenticationResponse obj)
        {
            _session.Profile = new ProfileModel()
            {
                Guid = obj.Guid
            };
            CallApi(async () =>
            {
                Response response = await _accountService.Get(_session.Profile.Guid);
                ManageApiResponses(response, new CallbackManager<Response>
                {
                    OnSuccess = async (res) =>
                    {
                        await CheckCguAccepted();
                    }
                });
            });
        }

        private async Task CheckCguAccepted()
        {
            var cguAlreadyAccepted = await _cacheService.Get<bool>(Constants.CguAccepted);
            if (cguAlreadyAccepted)
            {
                await InitFirebase();
                NavigationService.NavigateTo(Locator.DashboardView);
            }
            else
            {
                var settings = new PopupViewSettings
                {
                    CloseWhenBackgroundIsClicked = false,
                    DisplayBottomButtons = true,
                    OnAcceptAction = async () =>
                    {
                        await _cacheService.Put(Constants.CguAccepted, true);
                        await InitFirebase();
                        NavigationService.NavigateTo(Locator.DashboardView);
                    }
                };
                await PopupNavigation.Instance.PushAsync(new CguPopupView(settings));
            }
        }

        private async Task InitFirebase()
        {
            IsLoading = true;

            PermissionStatus notificationPermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<NotificationPermission>();
            if (notificationPermission == PermissionStatus.Granted)
            {
                _notificationService.InitFirebase();
                if (_session.IsConnected())
                {
                    var tokenResponse = await _accountService.AddMobileToken(_notificationService.GetFirebaseToken(), _session.Profile);
                    Debug.WriteLine("AddMobileToken response: " + tokenResponse);
                }
            }

            IsLoading = false;
        }
    }
}
