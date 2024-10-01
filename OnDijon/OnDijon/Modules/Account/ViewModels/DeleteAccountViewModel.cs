using OnDijon.Common.Entities;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils.Command;
using OnDijon.Common.Utils.Validations;
using OnDijon.Modules.Account.Services.Interfaces;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Extensions;
using Prism.Navigation;

namespace OnDijon.Modules.Account.ViewModels
{
    public class DeleteAccountViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ISession _session;

        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();

        public bool CanDelete => !string.IsNullOrEmpty(Password.Value);

        public ICommand DeleteCommand { get; set; }

        public DeleteAccountViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      ISession session,
                                      IAccountService accountService,
                                      ILoggerService loggerService ) 
	        : base(navigationService, translationService, popupService, loggerService)
        {
            _accountService = accountService;
            _session = session;

            DeleteCommand = new AsyncBlockingCommand(async () => { await DeleteAsync(); });

            AddValidations();
        }

        private void AddValidations()
        {
            Password.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Mot de passe requis" });
            Password.PropertyChanged += OnPropertyChanged;
        }



        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RaisePropertyChanged(nameof(CanDelete));
            }
        }

        private async Task DeleteAsync()
        {
            if (Password.Validate())
            {
                // Make a request
                Response response = await _accountService.Delete(_session.Profile.Guid, Password.Value);

                ManageApiResponses(response, new CallbackManager<Response>
                {
                    OnSuccess = DeleteAccountOnSuccess,
                    OnInvalidCredentials = (res) =>
                    {
                        PopupService.Show(PopupEnum.PopupError, "Erreur d'identification", res.Message, "Retour");
                    },
                    OnInvalidInformations = (res) =>
                    {
                        PopupService.Show(PopupEnum.PopupError, "Erreur d'identification", res.Message, "Retour");
                    },
                });
            }
        }
        void DeleteAccountOnSuccess(Response obj)
        {
            _accountService.Disconnect();
            NavigationService.GoBackToPageKey(Locator.DashboardView);
        }

    }
}
