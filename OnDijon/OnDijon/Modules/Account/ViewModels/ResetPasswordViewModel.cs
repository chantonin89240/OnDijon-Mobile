using OnDijon.Common.Entities;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils.Helpers;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Entities.Request;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils.Validations;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.Account.ViewModels
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        readonly IAccountService _accountService;
        readonly ISession _session;

        public ICommand ResetPassword { get; }

        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();

        public ResetPasswordViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      IAccountService accountService,
                                      ISession session,
                                      ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _accountService = accountService;
            _session = session;

            ResetPassword = new DelegateCommand(() =>
            {
                SendResetPasswordRequest();
            });

            AddValidations();
        }

        private void AddValidations()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "Email requis" });
            Email.Validations.Add(new PredicateRule<string> { ValidationMessage = "Email incorrect", Predicate = (value) => RegexHelper.CheckEmailRegex(value) });
        }

        private void SendResetPasswordRequest()
        {
            if (Email.Validate())
            {
                ResetPasswordRequest resetPasswordRequest = new ResetPasswordRequest
                {
                    Mail = Email.Value,
                };

                CallApi(async () =>
                {
                    Response response = await _accountService.ResetPassword( Email.Value);
                    ManageApiResponses(response, new CallbackManager<Response>
                    {
                        OnSuccess = ResetPasswordOnSuccess,
                        OnInvalidInformations = ResetPasswordOnInvalidInformations,
                        ProfileNotFound = ResetPasswordOnInvalidInformations,

                    });
                });
            }
        }

        void ResetPasswordOnSuccess(Response obj)
        {
            PopupService.Show(PopupEnum.PopupSuccess, "Mail envoyé", obj.Message, "C'est compris");
        }
        void ResetPasswordOnInvalidInformations(Response obj)
        {
            PopupService.Show(PopupEnum.PopupError, "Oups", obj.Message, "C'est compris");
        }
    }
}
