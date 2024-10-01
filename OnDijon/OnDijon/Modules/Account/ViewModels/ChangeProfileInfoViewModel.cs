using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Entities;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils.Validations;
using Xamarin.Forms;
using OnDijon.Modules.Account.Entities.Response;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.Account.ViewModels
{
    public class ChangeProfileInfoViewModel : BaseViewModel
    {
        public ICommand UpdateProfile { get; }

        private readonly IAccountService _accountService;
        private readonly ISession _session;

        private bool _isUpdatePasswordContainerVisible;

        public bool IsUpdatePasswordContainerVisible
        {
            get { return _isUpdatePasswordContainerVisible; }
            set { Set(ref _isUpdatePasswordContainerVisible, value); }
        }

        public ICommand UpdatePasswordCommand { get; }


        public ValidatableProfile Profile { get; set; }

        public ValidatableObject<string> OldPassword { get; set; } = new ValidatableObject<string>();

        public ChangeProfileInfoViewModel(INavigationService navigationService,
                                          ITranslationService translationService,
                                          IPopupService popupService,
                                          ISession session,
                                          IAccountService accountService,
                                          ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _accountService = accountService;
            _session = session;

            UpdatePasswordCommand = new Command(() => IsUpdatePasswordContainerVisible = !IsUpdatePasswordContainerVisible);

            UpdateProfile = new DelegateCommand(() =>
            {
                //try update password only if at least one of the password fields is not empty
                if (IsUpdatePasswordContainerVisible &&
                    (!string.IsNullOrEmpty(OldPassword.Value) ||
                    !string.IsNullOrEmpty(Profile.Password.Value) ||
                    !string.IsNullOrEmpty(Profile.PasswordConfirmation.Value)))
                {
                    UpdatePassword();
                }
                else
                {
                    UpdateUserInfo();
                }
            });

            OldPassword.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Saisir votre mot de passe actuel" });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
	        GetAccount();
        }

        public void GetAccount()
        {
            Profile = new ValidatableProfile { Content = _session.Profile };
            RaisePropertyChanged(nameof(Profile));
        }

        private void UpdateUserInfo()
        {
            if (OldPassword.Validate() && Profile.Validate())
            {
                CallApi(async () =>
                {
                    AuthenticationResponse response = await _accountService.Authenticate(_session.Profile.Mail, OldPassword.Value);
                    ManageApiResponses(response, new CallbackManager<Response>
                    {
                        OnSuccess = AuthenticateOnSuccess,
                        OnInvalidCredentials = AuthenticateOnInvalideCredentials

                    });
                });
            }
        }

        private void AuthenticateOnInvalideCredentials(Response obj)
        {
            OldPassword.Errors = new List<string> { "Mot de passe incorrect" };
        }

        private void AuthenticateOnSuccess(Response obj)
        {
            CallApi(async () =>
            {
                Response response = await _accountService.Update(Profile.Content, Profile.Content.Guid);
                ManageApiResponses(response, new CallbackManager<Response>
                {
                    OnSuccess = UpdateSuccess,
                    OnInvalidInformations = UpdateError
                });
            });
        }

        public ICommand GenderSelectionChangedCommand => new Command((o =>
                                                                      {
                                                                          if (Profile?.Gender != null)
                                                                              Profile.Gender.Value = o as string;
                                                                      }));

        private void UpdatePassword()
        {
            if (OldPassword.Validate() && Profile.ValidatePassword())
            {
                CallApi(async () =>
                {
                    Response response = await _accountService.ChangePassword(Profile.Content.Guid, OldPassword.Value, Profile.Password.Value);
                    ManageApiResponses(response, new CallbackManager<Response>
                    {
                        OnSuccess = UpdateSuccess,
                        OnInvalidCredentials = UpdateError,
                    });
                });
            }
        }

        private void UpdateSuccess(Response response)
        {
            PopupService.Show(PopupEnum.PopupSuccess, "Modifications effectuées", response.Message, "Revenir à mon profil", async () => await NavigationService.GoBackAsync());
        }

        private void UpdateError(Response response)
        {
            PopupService.Show(PopupEnum.PopupError, "Action impossible", response.Message, "Retour");
        }

        public override void Cleanup()
        {
            base.Cleanup();

            IsUpdatePasswordContainerVisible = false;
        }
    }
}
