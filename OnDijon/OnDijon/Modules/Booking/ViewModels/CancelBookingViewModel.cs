using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Extensions;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Booking.Entities.Models;
using OnDijon.Modules.Booking.Entities.Responses;
using OnDijon.Modules.Booking.Services.Interfaces;
using OnDijon.Modules.Demands.Entities.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Utils;
using Xamarin.Forms;

namespace OnDijon.Modules.Booking.ViewModels
{
    public class CancelBookingViewModel : BaseViewModel
    {
        readonly ISession _session;
        readonly IBookingService _BookingService;

        private bool _cancel;
        public bool Cancel { get => _cancel; set => Set(ref _cancel, value); }

        private bool _reminderVisible;
        public bool ReminderVisible { get => _reminderVisible; set => Set(ref _reminderVisible, value); }

        private bool _confirmationVisible;
        public bool ConfirmationVisible { get => _confirmationVisible; set => Set(ref _confirmationVisible, value); }

        private bool _cancelErrorIsVisible;
        public bool CancelErrorIsVisible { get => _cancelErrorIsVisible; set => Set(ref _cancelErrorIsVisible, value); }

        private ActionModel _action;
        public ActionModel Action
        {
            get => _action;
            set
            {
                Set(ref _action, value);
                GetBookingInformations();
            }
        }

        private BookingInformationsModel _bookingInformations;
        public BookingInformationsModel BookingInformations { get => _bookingInformations; set => Set(ref _bookingInformations, value); }

        public ICommand ConfirmCommand { get; set; }
        public ICommand GoDashboardCommand { get; set; }
        


        public CancelBookingViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      ISession session,
                                      IBookingService bookingService, 
                                      ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _BookingService = bookingService;

            ConfirmCommand = new Command(CancelBooking);
            //TODO Cleanup : vide les infos avant navigation
            GoDashboardCommand = new DelegateCommand(async () => { await CleanAndClose(); });
            InitData();
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
	        if (parameters.TryGetValue<ActionModel>(Constants.CancelBookingActionNavigationParameterKey, out var action))
	        {
		        Action = action;
	        }
        }

        public void InitData()
        {
            ReminderVisible = true;
            ConfirmationVisible = false;
            CancelErrorIsVisible = false;
        }

        public void CancelBooking()
        {
            if (Cancel)
            {
                CancelErrorIsVisible = false;
                CallApi(async () =>
                {
                    Response response = await _BookingService.CancelBooking(Action.ObjectEditId, Action.CityContext);
                    ManageApiResponses(response, new DefaultCallbackManager<Response>(PopupService)
                    {
                        OnSuccess = (res) =>
                        {
                            ReminderVisible = false;
                            ConfirmationVisible = true;
                        },
                        OnError = (res) =>
                        {
                            PopupService.Show(PopupEnum.PopupError, "Une erreur est survenue", "Votre demande n'a pas pu aboutir, veuillez réessayer ultérieurement.", "Retour", async () =>
                            {
                                await CleanAndClose();
                            });
                        }
                    });
                });
            }
            else
            {
                CancelErrorIsVisible = true;
            }
        }

        public void GetBookingInformations()
        {
            CallApi(async () =>
            {
                BookingInformationsResponse response = await _BookingService.GetBookingInformation(Action.ObjectEditId, Action.CityContext);
                ManageApiResponses(response, new DefaultCallbackManager<BookingInformationsResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        BookingInformations = new BookingInformationsModel()
                        {
                            Civility = res.Civility,
                            Day = res.Day,
                            FirstName = res.FirstName,
                            Institution = res.Institution,
                            Name = res.Name,
                            NbOfPerson = res.NbOfPerson
                        };
                    },
                    OnError = (res) =>
                    {
                        PopupService.Show(PopupEnum.PopupError, "Une erreur est survenue", "Votre demande n'a pas pu aboutir, veuillez réessayer ultérieurement.", "Retour", async () =>
                        {
                            await CleanAndClose();
                        });
                    }
                });
            });
        }


        private async Task CleanAndClose()
        {
            Cleanup();
            _session.Cleanup();
            await NavigationService.NavigateTo(Locator.DashboardView);
        }

        public override void Cleanup()
        {
            base.Cleanup();
            ReminderVisible = true;
            ConfirmationVisible = false;
            CancelErrorIsVisible = false;
        }
    }
}
