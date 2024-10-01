using OnDijon.Common.Entities;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Library.Entities.Dto.Model;
using OnDijon.Modules.Library.Entities.Model;
using OnDijon.Modules.Library.Entities.Response;
using OnDijon.Modules.Library.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace OnDijon.Modules.Library.ViewModels
{
    public class ReservationListViewModel : BaseViewModel
    {
        private IReservationService ReservationService;
        private IDocumentService DocumentService;

        private bool _reservationListIsEmpty;
        public bool ReservationListIsEmpty { get => _reservationListIsEmpty; set => Set(ref _reservationListIsEmpty, value); }

        private ObservableCollection<ReservationViewModel> _reservationList;
        public ObservableCollection<ReservationViewModel> ReservationList{
            get => _reservationList;
            set
            {
                Set(ref _reservationList, value);
                ReservationListIsEmpty = !(_reservationList.Count > 0);
            }
        }

        public ICommand CancelReservationCommand { get; set; }

        public ReservationListViewModel(INavigationService navigationService,
                                        ITranslationService translationService,
                                        IPopupService popupService,
                                        IReservationService reservationService,
                                        IDocumentService documentService, 
                                        ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            DocumentService = documentService;
            ReservationService = reservationService;
            ReservationList = new ObservableCollection<ReservationViewModel>();
            CancelReservationCommand = new DelegateCommand<ReservationViewModel>(CancelReservation);
        }

        internal void UpdateReservations(List<ReservationDto> reservations)
        {
            ReservationList.Clear();
            reservations.ForEach(r => ReservationList.Add(new ReservationViewModel(r)));
            ReservationListIsEmpty = !ReservationList.Any();
            LoadImage();
        }
        private void LoadImage()
        {
            ReservationList.ForEach(async (reservation) =>
            {
                if (!string.IsNullOrEmpty(reservation.Reservation.RecordId))
                {
                    LibraryDocResponse response = await DocumentService.GetImageUrl(reservation.Reservation.RecordId);
                    ManageApiResponses(response, new CallbackManager<LibraryDocResponse>()
                    {
                        OnSuccess = (res) =>
                        {
                            reservation.ImageUrl = res.Picture;
                        }
                    });
                }
            });
        }

        public override void Cleanup()
        {
            ReservationList = new ObservableCollection<ReservationViewModel>();
            ReservationListIsEmpty = true;
        }

        private void CancelReservation(ReservationViewModel item)
        {
            CallApi(async () =>
            {
                CancelReservationResponse response = await ReservationService.CancelReservation(item.Reservation);
                ManageApiResponses(response, new DefaultCallbackManager<CancelReservationResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        CancelReservation cancel = response.CancelReservation;
                        bool isCanceled = cancel.IsCanceled;
                        string txt = isCanceled ? "Annulation réussie  " : cancel.NotCanceledReason;
                        string title = isCanceled ? "Annulation" : "Annulation échouée";
                        if (isCanceled)
                        {
                            ReservationList.Remove(item);
                            PopupService.Show(PopupEnum.PopupSuccess, title, txt, "Ok");
                        }
                        else
                        {
                            PopupService.Show(PopupEnum.PopupError, title, txt, "Continuer");
                        }
                    }
                });
            });
        }
    }
}
