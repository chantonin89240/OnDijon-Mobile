using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Entities;
using OnDijon.Modules.Account.Pages;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OnDijon.Modules.Account.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICacheService _cacheService;


        public ICommand SignUp { get; }

        public ICommand DisplayCGUCommand { get; }

        public ICommand DisplayDataProtectionCommand { get; }
        
        public ICommand GenderSelectionChangedCommand => new Command((o =>
                                                                      {
                                                                          if (Profile?.Gender != null)
                                                                              Profile.Gender.Value = o as string;
                                                                      }));

        public ValidatableProfile Profile { get; set; }

        #region bool => CguAccepted

        private bool _cguAccepted;

        public bool CguAccepted
        {
            get => _cguAccepted;
            set => SetProperty(ref _cguAccepted, value);
        }

        #endregion

        #region bool => DataProtectionAccepted

        private bool _dataProtectionAccepted;

        public bool DataProtectionAccepted
        {
            get => _dataProtectionAccepted;
            set => SetProperty(ref _dataProtectionAccepted, value);
        }

        #endregion

        public SignUpViewModel(INavigationService navigationService,
                               IAccountService accountService,
                               ITranslationService translationService,
                               IPopupService popupService,
                               ICacheService cacheService, 
                               ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _accountService = accountService;
            _cacheService = cacheService;

            SignUp = new DelegateCommand(() =>
            {
                CreateAccount();
            });

            DisplayCGUCommand = new DelegateCommand(async () => { await DisplayCgu(); });

            DisplayDataProtectionCommand = new DelegateCommand(() =>
            {
                //TODO
            });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
	        InitAccount();
        }

        public void InitAccount()
        {
            Profile = new ValidatableProfile();
            RaisePropertyChanged(nameof(Profile));
        }

        private void CreateAccount()
        {
            if (Profile.Validate() && Profile.ValidatePassword())
            {
                CallApi(async () =>
                {
                    Response response = await _accountService.Create(Profile.Password.Value, Profile.Content);
                    ManageApiResponses(response, new CallbackManager<Response>
                    {
                        OnSuccess = CreateAccountOnSuccess,
                        OnError = (res) =>
                        {
                            IsLoading = false;
                            PopupService.Show(PopupEnum.PopupError, "Une erreur est survenue", response.Message, "Retour");
                        }
                    });
                });
            }
        }

        private async void CreateAccountOnSuccess(Response response)
        {
            PopupService.Show(PopupEnum.PopupSuccess, "Compte créé", "Veuillez vous connecter à votre nouveau compte !", response.Message, "Revenir à la connexion", async () => await NavigationService.GoBackAsync());
        }

        private async Task DisplayCgu()
        {
            await PopupNavigation.Instance.PushAsync(new CguPopupView());
        }
        
      
    }
}
