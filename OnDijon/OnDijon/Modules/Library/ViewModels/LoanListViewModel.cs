using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Library.Entities.Response;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using OnDijon.Common.Entities;
using OnDijon.Modules.Library.Services;
using Xamarin.Forms.Internals;
using OnDijon.Modules.Library.Entities.Dto.Model;
using OnDijon.Common.Utils;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.Library.ViewModels
{
    public class LoanListViewModel : BaseViewModel
    {
        private ILoanService LoanService;
        private IDocumentService DocumentService;

        private bool _loanListIsEmpty;
        public bool LoanListIsEmpty { get => _loanListIsEmpty; set => Set(ref _loanListIsEmpty, value); }

        private ObservableCollection<LoanViewModel> _loanList;
        public ObservableCollection<LoanViewModel> LoanList { get => _loanList; set => Set(ref _loanList, value);}

        public ICommand RenewLoanCommand { get; set; }

        public LoanListViewModel(INavigationService navigationService,
                                 ITranslationService translationService,
                                 IPopupService popupService,
                                 ILoanService loanService,
                                 IDocumentService documentService,
                                 ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {

            LoanService = loanService;
            DocumentService = documentService;
            LoanList = new ObservableCollection<LoanViewModel>();
            RenewLoanCommand = new DelegateCommand<LoanViewModel>(RenewLoan);
        }

        internal void UpdateLoanList(List<LoanDto> loans)
        {
            LoanList.Clear();
            loans.ForEach(l => LoanList.Add(new LoanViewModel() { Loan = l}));
            LoanListIsEmpty = !LoanList.Any();
            LoadImage();
        }

        private void LoadImage()
        {
            LoanList.ForEach(async (loan) =>
            {
                if (!string.IsNullOrEmpty(loan.Loan.RecordId))
                {
                    LibraryDocResponse response = await DocumentService.GetImageUrl(loan.Loan.RecordId);
                    ManageApiResponses(response, new CallbackManager<LibraryDocResponse>()
                    {
                        OnSuccess = (res) =>
                        {
                            loan.ImageUrl = res.Picture;
                        },
                        OnInvalidInformations = (res) =>
                        {
                            ShowError(string.Empty, new System.Exception("Chargement image BM impossible : " + Constants.BM_GET_Image + " ; " + res.Message), false);
                        }
                    });
                }
            });
        }

        public override void Cleanup()
        {
            LoanList = new ObservableCollection<LoanViewModel>();
            LoanListIsEmpty = true;
        }

        private void RenewLoan(LoanViewModel loan)
        {
            CallApi(async () =>
            {
                RenewLoanResponse response = await LoanService.RenewLoan(loan.Loan);
                ManageApiResponses(response, new DefaultCallbackManager<RenewLoanResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        RenewLoanDto RenewLoan = response.RenewLoan;
                        bool IsRenew = RenewLoan.IsRenewed;
                        string txt = IsRenew ? "Prolongation réussie pour : " + loan.Title : RenewLoan.NotRenewedReason;
                        string title = IsRenew ? "Prolongation" : "Prolongation échouée";

                        //App.Locator.LibraryMain.UpdateAccountCard(DisplayCardType.Loan);
                        if (IsRenew)
                        {
                            loan.Loan = RenewLoan.Loan;
                            PopupService.Show(PopupEnum.PopupSuccess, title, txt, "Ok");
                        }
                        else
                            PopupService.Show(PopupEnum.PopupError, title, txt, "Continuer");
                    }
                });
            });
        }
    }

}
