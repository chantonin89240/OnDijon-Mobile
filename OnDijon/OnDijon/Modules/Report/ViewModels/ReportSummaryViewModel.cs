using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Entities;
using OnDijon.Modules.Report.Entities.Request;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils.Services.Interfaces;
using Xamarin.Forms;
using OnDijon.Modules.Report.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.Report.ViewModels
{
    public class ReportSummaryViewModel : ReportBaseViewModel
    {
        private readonly ISession _session;
        private readonly IReportService _reportService;
        private readonly IUserIdService _userIdService;

        public ReportRequest Report => _session.ReportRequest;

        public bool PhotoAvailable => _session.ReportRequest.ReportContent.Photos != null && _session.ReportRequest.ReportContent.Photos.Any();

        public ImageSource PhotoSource { get; private set; }

        public ICommand SendReportCommand { get; }

        public ReportSummaryViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      ISession session,
                                      IReportService reportService,
                                      IUserIdService userIdService,
                                      ILoggerService loggerService) : base(navigationService, translationService, popupService, session, loggerService)
        {
            _session = session;
            _reportService = reportService;
            _userIdService = userIdService;

            SendReportCommand = new DelegateCommand(SendReport, CanSendReport);
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
	        UpdatePhotoSource();
        }

        public void UpdatePhotoSource()
        {
            if (PhotoAvailable)
            {
                PhotoSource = ImageSource.FromStream(() => new MemoryStream(Report.ReportContent.Photos[0]));
                RaisePropertyChanged(nameof(PhotoSource));
                RaisePropertyChanged(nameof(PhotoAvailable));
            }
        }

        private void SendReport()
        {
            CallApi(async () =>
            {
                //registration token Firebase
                _session.ReportRequest.ReportContent.RegistrationToken = await _userIdService.GetUserId();

                Response response = await _reportService.SendReport(_session.ReportRequest);
                ManageApiResponses(response, new CallbackManager<Response>
                {
                    OnSuccess = SendReportOnSuccess,
                });
            });
        }

        private bool CanSendReport()
        {
            return !IsLoading;
        }

        void SendReportOnSuccess(Response response)
        {
            NavigateTo(Locator.ReportSuccessView);

            //TODO Cleanup : Après call API réussi vide toutes les infos du report
            Cleanup();
        }
    }
}
