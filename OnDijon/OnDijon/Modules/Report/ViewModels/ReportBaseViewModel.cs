using System.Windows.Input;
using OnDijon.Common.Extensions;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using AsyncAwaitBestPractices.MVVM;
using System.Threading.Tasks;

namespace OnDijon.Modules.Report.ViewModels
{
    public class ReportBaseViewModel : BaseViewModel
    {

        readonly ISession _session;


        public ICommand CloseCommand { get; }

        public ReportBaseViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IPopupService popupService,
                                   ISession session, 
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;

            CloseCommand = new AsyncCommand(OnClose);
        }


        private async Task OnClose()
        {
                PopupService.Show(PopupEnum.PopupInfo, "Attention", "Attention vous allez perdre votre saisie actuelle, voulez-vous continuer ?", "Quitter", async () =>
                {
                    await Close();
                }, "Annuler");

        }

        private async Task Close()
        {
            //TODO Cleanup : Vide toutes les infos en cas d'annulation d'un report 
            _session.Cleanup();
            await NavigationService.GoBackToPageKey(Locator.ReportsUserView);
        }
    }
}
