 using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Library.Entities.Model;
using OnDijon.Modules.Library.Entities.Response;
using OnDijon.Modules.Library.Services;
using OnDijon.Modules.Library.Views.Popup;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Enums;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace OnDijon.Modules.Library.ViewModels
{
    public class CatalogDetailViewModel : BaseViewModel
    {
        private readonly IReservationService _reservationService;
        private readonly IDocumentService _documentService;


        public IList<ReaderAccount> AccountReaderList { get; set; }

        private Resource _record;
        public Resource Record { get => _record; set => Set(ref _record, value); }

        private bool _hasHoldings;
        public bool HasHoldings { get => _hasHoldings; set => Set(ref _hasHoldings, value); }

        private bool _hasAccountReader;
        public bool HasAccountReader { get => _hasAccountReader; set => Set(ref _hasAccountReader, value); }

        private ObservableCollection<Holding> _holdings;
        public ObservableCollection<Holding> Holdings { get => _holdings; set { 
                Set(ref _holdings, value);
                // _holdings.ToList().ForEach(a =>
                //{
                //    RaisePropertyChanged(nameof(a));
                //    RaisePropertyChanged(nameof(a.IsReservable));
                //});
                } }


        public ICommand OpenReservationCommand { get; set; }
        public ICommand PlaceReservationCommand { get; set; }

        public CatalogDetailViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      IReservationService reservationService,
                                      IDocumentService documentService,
                                      ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _reservationService = reservationService;
            _documentService = documentService;
            OpenReservationCommand = new Command<Holding>(PopupPlaceReservation, CanExecuteOpenReservationCommand);
            PlaceReservationCommand = new DelegateCommand<string>((IdBorrower) =>
            {
                ClosePopupPlaceReservation();
                PlaceReservation(IdBorrower);
            });
        }

        private bool CanExecuteOpenReservationCommand(Holding arg)
        {
            return arg?.IsReservable ?? false;
        }

        public override void Cleanup()
        {
            base.Cleanup();
            Record = null;
            HasHoldings = false;
            HasAccountReader = false;
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            
            if (parameters.ContainsKey(Constants.CatalogDetailResourceNavigationParameterKey))
			{
				Record = parameters.GetValue<Resource>(Constants.CatalogDetailResourceNavigationParameterKey);
			}
            if (parameters.ContainsKey(Constants.CatalogDetailAccountReaderListNavigationParameterKey))
			{
				AccountReaderList = parameters.GetValue<IList<ReaderAccount>>(Constants.CatalogDetailAccountReaderListNavigationParameterKey);
			}
            if (Record != null && !string.IsNullOrEmpty(Record.Id))
            {
                IsLoading = true;
                HasAccountReader = AccountReaderList.Any();
                Getholdings();
            }
        }

        public void Getholdings()
        {
            CallApi(async () =>
            {
                HoldingResponse response = await _documentService.GetHoldings(Record.Id);
                ManageApiResponses(response, new DefaultCallbackManager<HoldingResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.Holdings.Any())
                        {
                            Holdings = new ObservableCollection<Holding>(res.Holdings);
                            //RaisePropertyChanged(nameof(Holdings));
                            HasHoldings = true;
                        }
                    },
                });
            });
        }


        public async void PopupPlaceReservation(Holding holding)
        {
            await PopupNavigation.Instance.PushAsync(new AccountPopupView(this));
        }
        public async void ClosePopupPlaceReservation()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        public void PlaceReservation(string IdBorrower)
        {
            CallApi(async () =>
            {
                Response response = await _reservationService.PlaceReservation(IdBorrower, Record.Id);
                ManageApiResponses(response, new DefaultCallbackManager<Response>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.IsSuccessful())
                        {
                            PopupService.Show(PopupEnum.PopupSuccess,"Réservation", "Réservation réussie", "Ok");
                        }
                    },
                });
            });
        }
    }
}
