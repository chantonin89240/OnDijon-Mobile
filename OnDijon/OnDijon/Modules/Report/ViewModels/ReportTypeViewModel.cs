using System.Collections.Generic;
using System.Windows.Input;
using OnDijon.Common.Entities;
using OnDijon.Modules.Report.Entities.Dto;
using OnDijon.Modules.Report.Entities.Request;
using OnDijon.Modules.Report.Entities.Response;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Report.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using AsyncAwaitBestPractices.MVVM;
using System.Threading.Tasks;
using OnDijon.Common.Utils.Enums;

namespace OnDijon.Modules.Report.ViewModels
{
    public class ReportTypeViewModel : ReportBaseViewModel
    {
        readonly IReportService _reportService;
        readonly ISession _session;

        private IList<ReportTypeDto> _reportTypes;
        public IList<ReportTypeDto> ReportTypes
        {
            get { return _reportTypes; }
            set { Set(ref _reportTypes, value); }
        }

        public ICommand GoToNextPageCommand { get; }

        public ReportTypeViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IPopupService popupService,
                                   IReportService reportService,
                                   ISession session,
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, session, loggerService)
        {
            _reportService = reportService;
            _session = session;

            GoToNextPageCommand = new AsyncCommand<ReportTypeDto>(async (type) =>
                                                                     {

	                                                                     _session.ReportRequest = _session.ReportRequest ?? new ReportRequest();
	                                                                     _session.ReportRequest.ReportContent.ReportTypeCode = type.Code;
	                                                                     _session.ReportRequest.ReportContent.ReportTypeName = type.Name;

	                                                                     await NavigationService.NavigateAsync(Locator.ReportLocalisationView);
                                                                     });
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            PopupService.Show(PopupEnum.PopupInfo, (FormattedString)App.Current.Resources["warningMessage"], "Continuer");
            if (ReportTypes == null)
                QueryReportTypes();
        }

        public void QueryReportTypes()
        {
            CallApi(async () =>
            {
                ReportTypesListResponse response = await _reportService.GetReportTypes();
                ManageApiResponses(response, new CallbackManager<ReportTypesListResponse>
                {
                    OnSuccess = (res) => ReportTypes = new List<ReportTypeDto>(res.ReportTypes)
                });
            });
        }
    }
}
