using System.Threading.Tasks;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using System.Windows.Input;
using OnDijon.Common.Utils;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace OnDijon.Modules.Library.ViewModels
{
    public class StreamingOnlineViewModel : BaseViewModel
    {
        private string _url;
        public string Url
        {
            get { return _url; }
            set { Set(ref _url, value); }
        }

        public ICommand CloseWebView { get; }

        public StreamingOnlineViewModel(INavigationService navigationService,
                                        ITranslationService translationService,
                                        IPopupService popupService,
                                        ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            CloseWebView = new DelegateCommand<WebView>(wv => CleanUrlBeforeClose(wv));
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
	        if (parameters.TryGetValue<string>(Constants.StreamingOnlineUrlNavigationParameterKey, out string url))
		        Url = url;
        }

        public void CleanUrlBeforeClose(WebView vw)
        {
            Url = string.Empty;
            vw.Source = new HtmlWebViewSource { Html = "<html></html>" };
            NavigationService.GoBackAsync();
        }
    }
}
