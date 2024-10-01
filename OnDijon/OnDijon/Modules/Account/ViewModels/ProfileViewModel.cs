using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Entities.Models;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Library.ViewModels;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.Account.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ISession _session;

        public ICommand OnUpdateProfileCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ProfileModel Profile { get; set; }
        
        public ProfilCardsViewModel ProfileCardsViewModel { get; set; }

        public ProfileViewModel(INavigationService navigationService,
                                ITranslationService translationService,
                                IAccountService accountService,
                                ISession session,
                                IPopupService popupService,
                                ILoggerService loggerService) 
	        : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _accountService = accountService;

            OnUpdateProfileCommand = new DelegateCommand(() => { ChangeProfile(); });

            DeleteCommand = new DelegateCommand(() => { Delete(); });

            ProfileCardsViewModel = App.Locator.GetInstance<ProfilCardsViewModel>();
            
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
	        GetProfile();
	        ProfileCardsViewModel.LoadData();
        }

        public void GetProfile()
        {
            Profile = _session.Profile;
            RaisePropertyChanged(nameof(Profile));
        }

        private void ChangeProfile()
        {
            NavigateTo(Locator.ChangeProfileInfoView);
        }

        private void Delete()
        {
            NavigateTo(Locator.DeleteAccountPage);
        }
    }
}