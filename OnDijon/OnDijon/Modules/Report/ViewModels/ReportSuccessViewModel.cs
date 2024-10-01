using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Extensions;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Report.Pages;
using Prism.Commands;
using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;

namespace OnDijon.Modules.Report.ViewModels
{
    public class ReportSuccessViewModel : ReportBaseViewModel
    {

        public ICommand GoToReportListCommand { get; }

        public ReportSuccessViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      ISession session,
                                      ILoggerService loggerService) : base(navigationService, translationService, popupService, session, loggerService)
        {
            GoToReportListCommand = new DelegateCommand(async () => await navigationService.GoBackToPageKey(Locator.ReportsUserView) );
        }

    }
}
