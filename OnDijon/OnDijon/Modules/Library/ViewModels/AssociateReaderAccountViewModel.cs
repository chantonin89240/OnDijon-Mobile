using System;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Entities;
using OnDijon.Modules.Library.Entities.Response;
using OnDijon.Modules.Library.Services;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.Library.ViewModels
{
    public class AssociateReaderAccountViewModel : BaseViewModel
    {

        private IAccountReaderService _accountReaderService;
     
        private string _login;
        private string _password;


        public string Login
        {
            get { return _login; }
            set { Set(ref _login, value, "Login"); }
        }
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value, "Password");}
        }

        public bool OnProfil = false;


        public ICommand AssociateAccountCommand { get; set; }
        public AssociateReaderAccountViewModel(INavigationService navigationService,
                                               ITranslationService translationService,
                                               IPopupService popupService,
                                               IAccountReaderService accountReaderService, 
                                               ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _accountReaderService = accountReaderService;
            AssociateAccountCommand = new DelegateCommand(AssociateAccount);
        }


        public override void Cleanup()
        {
            Login = null;
            Password = null ;

        }
        
        public Action InitReaderAccount { get; set; }

        private void AssociateAccount()
        {

            CallApi(async () =>
            {
                AssociateReaderAccountResponse response = await _accountReaderService.AssociateReaderAccount(Login, Password);
                
                ManageApiResponses(response, new DefaultCallbackManager<AssociateReaderAccountResponse>(PopupService)
                {
                    OnSuccess = (res) => {
                        if (res.Success)
                        {
	                        InitReaderAccount?.Invoke();
	                     
                            Cleanup();
                            PopupService.Show(PopupEnum.PopupSuccess, "Association de carte", "La carte a bien été associée.", "Ok");
                        }
                        else
                            PopupService.Show(PopupEnum.PopupError, "Association impossible", "Votre carte BM n'est pas reconnue", "Continuer");
                    }
                });
            });

        }

     

    }
}
