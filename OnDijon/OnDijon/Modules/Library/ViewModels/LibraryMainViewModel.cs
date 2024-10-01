using System;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Library.Entities.Model;
using OnDijon.Modules.Library.Entities.Response;
using OnDijon.Modules.Library.Services;
using System.Windows.Input;
using OnDijon.Common.Utils;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OnDijon.Common.Entities;
using OnDijon.Common.Extensions;
using OnDijon.Common.Utils.Tools;
using OnDijon.Modules.Account.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using AsyncAwaitBestPractices.MVVM;
using Newtonsoft.Json;

namespace OnDijon.Modules.Library.ViewModels
{
    public class LibraryMainViewModel : BaseViewModel
    {
        public enum DisplayCardType
        {
            Card,
            Loan,
            Reservation,
            NewCard,
        }

        readonly ISession _session;
        private ILoanService _loanService;
        private IReservationService _reservationService;
        private IAccountReaderService _accountReaderService;


        #region LibraryCardViewModel => LibraryCardViewModel

        private LibraryCardViewModel _libraryCardViewModel;

        public LibraryCardViewModel LibraryCardViewModel { get => _libraryCardViewModel; set => SetProperty(ref _libraryCardViewModel, value); }

        #endregion

        #region LoanListViewModel => LoanListViewModel

        private LoanListViewModel _loanListViewModel;

        public LoanListViewModel LoanListViewModel { get => _loanListViewModel; set => SetProperty(ref _loanListViewModel, value); }

        #endregion

        #region ReservationListViewModel => ReservationListViewModel

        private ReservationListViewModel _reservationListViewModel;

        public ReservationListViewModel ReservationListViewModel { get => _reservationListViewModel; set => SetProperty(ref _reservationListViewModel, value); }

        #endregion

        

        private ReaderAccount _CurrentReaderAccount;
        public ReaderAccount CurrentReaderAccount
        {
            get { return _CurrentReaderAccount; }
            set
            {
                Set(ref _CurrentReaderAccount, value);
                UpdateAccountCard();
            }
        }

        public ObservableCollection<ReaderAccount> AccountChoice { get; set; }

        private bool _isNewCardDisplay;
        public bool IsNewCardDisplay { get => _isNewCardDisplay; set => Set(ref _isNewCardDisplay, value); }

        private bool _haveCards;
        public bool HaveCards { get => _haveCards; set => Set(ref _haveCards, value); }

        private bool _showAddCardButton;
        public bool ShowAddCardButton { get => _showAddCardButton; set => Set(ref _showAddCardButton, value); }

        private bool _isBlockByPassword;
        public bool IsBlockByPassword { get => _isBlockByPassword; set => Set(ref _isBlockByPassword, value); }

        private bool _errorPasswordLabelVisible;
        public bool ErrorPasswordLabelVisible { get => _errorPasswordLabelVisible; set => Set(ref _errorPasswordLabelVisible, value); }

        private string _newPassword;
        public string NewPassword { get => _newPassword; set => Set(ref _newPassword, value); }

        private DisplayCardType _currentCardType;
        public DisplayCardType CurrentCardType { get => _currentCardType; set => Set(ref _currentCardType, value); }

        #region AssociateReaderAccountViewModel => AssociateReaderAccount

        private AssociateReaderAccountViewModel _associateReaderAccount;

        public AssociateReaderAccountViewModel AssociateReaderAccount { get => _associateReaderAccount; set => SetProperty(ref _associateReaderAccount, value); }

        #endregion
        public ICommand CardCommand { get; set; }
        public ICommand CleanupCommand { get; set; }
        public ICommand CarouselChangeCommand { get; set; }
        public ICommand SearchLibraryCommand { get; }
        public ICommand ResendPasswordCommand { get; }

        public LibraryMainViewModel(INavigationService navigationService,
                                    ITranslationService translationService,
                                    IPopupService popupService,
                                    ISession session,
                                    IAccountReaderService accountReaderService,
                                    ILoanService loanService,
                                    IReservationService reservationService, 
                                    ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
	        try
	        {
		        _accountReaderService = accountReaderService;
		        _loanService = loanService;
		        _reservationService = reservationService;
		        _session = session;
		        LibraryCardViewModel = App.Locator.GetInstance<LibraryCardViewModel>();
		        LoanListViewModel = App.Locator.GetInstance<LoanListViewModel>();
		        ReservationListViewModel = App.Locator.GetInstance<ReservationListViewModel>();
		        AccountChoice = new ObservableCollection<ReaderAccount>();
		        IsBlockByPassword = false;
		        ErrorPasswordLabelVisible = false;
		        CardCommand = new Command<DisplayCardType>(DisplayCard);
		        CleanupCommand = new AsyncCommand(CleanupAndClose);
		        CarouselChangeCommand = new Command(item => CurrentReaderAccount = item != null ? item as ReaderAccount : CurrentReaderAccount);
		        SearchLibraryCommand = new AsyncCommand(OpenCatalog);
		        ResendPasswordCommand = new DelegateCommand(ResendPassword);
		        AssociateReaderAccount = App.Locator.GetInstance<AssociateReaderAccountViewModel>();
		        AssociateReaderAccount.InitReaderAccount = InitReaderAccount;
	        }
	        catch (Exception e)
	        {
		        Logger.Error(ex : e);
	        }
          
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            Cleanup();
            InitReaderAccount();
        }

        public void InitReaderAccount()
        {
            CallApi(async () =>
            {
                ReaderAccountResponse response = await _accountReaderService.GetAccountByProfil();
                ManageApiResponses(response, new DefaultCallbackManager<ReaderAccountResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        AccountChoice.Clear();
                        response.UserAccount.ForEach(userA =>
                        {
                            AccountChoice.Add(userA);
                        });
                        HaveCards = AccountChoice.Count > 0;
                        AccountChoice.Add(new ReaderAccount() { TypeAccount = BmCardType.NewCard });
                        CurrentReaderAccount = AccountChoice.First();
                    }
                });
            });
        }

        public void LoadAccountData(BorrowerInformationResponse response)
        {
            if (CurrentReaderAccount.IdBorrower == response.UserInformation.IdBorrower)
            {
                if (CurrentReaderAccount.ImageUri == RecipeUIConstants.AvatarNeutralSource)
                {
                    int index = AccountChoice.IndexOf(CurrentReaderAccount);
                    if (index >= 0)
                    {
                        CurrentReaderAccount.ImageUri = response.ImageUri;
                        CurrentReaderAccount.ImageSource = ImageTool.convertSourceImage(response.ImageUri);
                        AccountChoice[index] = CurrentReaderAccount;
                    }
                }
                CurrentReaderAccount.Uid = response.UserInformation.Uid;
                LoanListViewModel.UpdateLoanList(response.Loans);
                ReservationListViewModel.UpdateReservations(response.Reservations);
            }
        }

        private void LoadAccountInformation()
        {
            IsBlockByPassword = false;
            CallApi(async () =>
            {
                BorrowerInformationResponse response = await _accountReaderService.GetBorrowerInformation(CurrentReaderAccount.IdBorrower);
                ManageApiResponses(response, new DefaultCallbackManager<BorrowerInformationResponse>(PopupService)
                {
                    OnSuccess = LoadAccountData,
                    //Echec de l'authentification 
                    AuthError = (res) => IsBlockByPassword = true,
                });
            });
        }

        public void ResendPassword()
        {
            ErrorPasswordLabelVisible = false;
            CallApi(async () =>
            {
                string ediId = _session.Profile.Guid;
                BorrowerInformationResponse response = await _accountReaderService.UpdateBorrowerPassword(CurrentReaderAccount.IdBorrower, NewPassword);
                ManageApiResponses(response, new DefaultCallbackManager<BorrowerInformationResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        LoadAccountData(res);
                        IsBlockByPassword = false;
                        ErrorPasswordLabelVisible = false;
                        NewPassword = string.Empty;
                    },
                    //MDP saisie incorrect
                    AuthError = (res) => ErrorPasswordLabelVisible = true,
                });
            });
        }


        public void UpdateAccountCard(DisplayCardType displayCardType = DisplayCardType.Card)
        {
            if (CurrentReaderAccount.TypeAccount == BmCardType.Borrower)
            {
                LibraryCardViewModel.Accompt = CurrentReaderAccount;
                ReservationListViewModel.Cleanup();
                LoanListViewModel.Cleanup();
                LoadAccountInformation();
                DisplayCard(displayCardType);
            }
            if (CurrentReaderAccount.TypeAccount == BmCardType.NewCard)
            {
                DisplayCard(DisplayCardType.NewCard);
            }
        }

        private void DisplayCard(DisplayCardType displayCardType)
        {
            CurrentCardType = displayCardType;
            IsNewCardDisplay = displayCardType == DisplayCardType.NewCard;
            if (IsNewCardDisplay)
            {
                IsBlockByPassword = false;
            }
            ShowAddCardButton = IsNewCardDisplay && !HaveCards || !IsNewCardDisplay;
        }

 
        public async Task OpenCatalog()
        {
            var readerList = AccountChoice.Where(a => a.TypeAccount == BmCardType.Borrower).ToList();
            INavigationParameters param = new NavigationParameters
            {
                {Constants.ReaderListNavigationParameterKey,JsonConvert.SerializeObject(readerList) }
            };
            await NavigationService.NavigateAsync(Locator.CatalogSearchPage,param);
        }


        //public void OpenCatalog()
        //{
        //    if (IsNewCardDisplay)
        //    {
        //        SearchLibraryViewModel.Url = "https://internal-bm.dijon.fr/search.aspx?SC=CATALOGUE&QUERY=______________________&";
        //        NavigateTo(Locator.SearchLibraryPage);
        //    }
        //    else
        //    {
        //        CallApi(async () =>
        //        {
        //            AutoConnectUrlResponse response = await _accountReaderService.GetAutoConnectUrl(CurrentReaderAccount.Uid);
        //            ManageApiResponses(response, new DefaultCallbackManager<AutoConnectUrlResponse>(PopupService)
        //            {
        //                OnSuccess = (res) =>
        //                {
        //                    SearchLibraryViewModel.Url = res.Url;
        //                    NavigateTo(Locator.SearchLibraryPage);
        //                }
        //            });
        //        });
        //    }
        //}


        public override void Cleanup()
        {
            base.Cleanup();
            AccountChoice.Clear();
            LoanListViewModel.Cleanup();
            ReservationListViewModel.Cleanup();
            //App.Locator.TopBarViewModel.Cleanup();
            IsBlockByPassword = false;
            ErrorPasswordLabelVisible = false;
            NewPassword = string.Empty;
        }

        public async Task CleanupAndClose()
        {
            Cleanup();
            await NavigationService.GoBackToPageKey(Locator.DashboardView);
        }
    }
}
