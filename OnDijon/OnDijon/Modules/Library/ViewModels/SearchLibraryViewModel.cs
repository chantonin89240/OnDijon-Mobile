using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.Library.ViewModels
{
    public class SearchLibraryViewModel : BaseViewModel
    {
        readonly ISession _session;


        private string _url;
        public string Url
        {
            get { return _url; }
            set { Set(ref _url, value); RaisePropertyChanged(nameof(Url)); }
        }
        //public string UrlLogOff
        //{
        //    get { return _url + "?logOff=dijon"; }
        //}


        public ICommand GoMainLibraryCommand { get; set; }

        public SearchLibraryViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      ISession session,
                                      ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            GoMainLibraryCommand = new DelegateCommand(() => { NavigateTo(Locator.LibraryMainPage); });
        }

        //internal void LogOff()
        //{
        //    Url = Constants.INTERNAL_BM_LOGOFF_URL;
        //}
    }
}
