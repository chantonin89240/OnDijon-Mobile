using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities;
using OnDijon.Modules.Library.Entities.Model;
using OnDijon.Modules.Library.Entities.Response;
using OnDijon.Modules.Library.Services;
using OnDijon.Modules.Library.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using OnDijon.Common.Extensions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace OnDijon.Modules.Library.ViewModels
{
    public class ProfilCardsViewModel : BaseViewModel
    {

        readonly ISession _session;
        private IAccountReaderService _accountReaderService;


        private ObservableCollection<ReaderAccount> _readerAccountList;
        public ObservableCollection<ReaderAccount> ReaderAccountList
        {
            get { return _readerAccountList; }
            set { Set(ref _readerAccountList, value); }
        }

        public ICommand AddCardCommand { get; set; }

        public ProfilCardsViewModel(INavigationService navigationService,
                                    ITranslationService translationService,
                                    ISession session,
                                    IPopupService popupService,
                                    IAccountReaderService accountReaderService, ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _accountReaderService = accountReaderService;
            ReaderAccountList = new ObservableCollection<ReaderAccount>();
            AddCardCommand = new Command(() => AddCard());

            AssociateReaderAccount = App.Locator.GetInstance<AssociateReaderAccountViewModel>();;
            AssociateReaderAccount.InitReaderAccount = InitReaderAccount;
            AssociateReaderAccount.OnProfil = true;
        }

        public void LoadData()
        {
            InitReaderAccount();
        }


        public void InitReaderAccount()
        {
            CallApi(async () =>
            {
                string ediId = _session.Profile.Guid;
                ReaderAccountResponse response = await _accountReaderService.GetAccountByProfil();
                ManageApiResponses(response, new DefaultCallbackManager<ReaderAccountResponse>(PopupService)
                {
                    OnSuccess = (res) => {
                        ReaderAccountList.Clear();
                        response.UserAccount.ForEach(userA =>
                        {
                            ReaderAccountList.Add(userA);
                        });
                    }
                });
            });
        }

        public void AddCard()
        {
	        PopupService.Show(new PopupAddCardView(AssociateReaderAccount));
        }

        public ICommand _DissociateReaderAccountCommand { get; set; }
        public ICommand DissociateReaderAccountCommand
        {
            get
            {
                return _DissociateReaderAccountCommand ?? (_DissociateReaderAccountCommand = new DelegateCommand<ReaderAccount>((readerAccount) =>
                                                                                                                                {

	                                                                                                                                PopupService.Show(PopupEnum.PopupInfo, "Attention", "Êtes-vous sûr de vouloir dissocier cette carte de votre compte ?", "Oui", () =>
	                                                                                                                                                                                                                                                               {
		                                                                                                                                                                                                                                                               CallApi(async () =>
		                                                                                                                                                                                                                                                                       {
			                                                                                                                                                                                                                                                                       DissociateReaderAccountResponse response = await _accountReaderService.DissociateReaderAccount(readerAccount.IdBorrower);

			                                                                                                                                                                                                                                                                       ManageApiResponses(response, new DefaultCallbackManager<DissociateReaderAccountResponse>(PopupService)
			                                                                                                                                                                                                                                                                                                    {
				                                                                                                                                                                                                                                                                                                    OnSuccess = (res) =>
				                                                                                                                                                                                                                                                                                                                {

					                                                                                                                                                                                                                                                                                                                ReaderAccountList.Clear();
					                                                                                                                                                                                                                                                                                                                NavigationService.GoBackToPageKey(Locator.ProfileView);
					                                                                                                                                                                                                                                                                                                                NavigationService.NavigateTo(Locator.ProfileView);
					                                                                                                                                                                                                                                                                                                                //InitReaderAccount(); // carousel out of range quand on l'utilise
				                                                                                                                                                                                                                                                                                                                }
			                                                                                                                                                                                                                                                                                                    });
		                                                                                                                                                                                                                                                                       });
	                                                                                                                                                                                                                                                               }, "Non");
                                                                                                                                }));
            }
        }

        #region AssociateReaderAccount => AssociateReaderAccount

        private AssociateReaderAccountViewModel _AssociateReaderAccount;

        public AssociateReaderAccountViewModel AssociateReaderAccount { get => _AssociateReaderAccount; set => SetProperty(ref _AssociateReaderAccount, value); }

        #endregion


    }
}
