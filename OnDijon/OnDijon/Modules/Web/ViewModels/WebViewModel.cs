using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using Prism.Navigation;
using System.Threading.Tasks;

namespace OnDijon.Modules.Web.ViewModels
{
    public class WebViewModel : BaseViewModel
    {

        private string _url;
        public string Url
        {
            get { return string.IsNullOrEmpty(_url) ? "https://www.metropole-dijon.fr" : _url; }
            set { Set(ref _url, value); }
        }
       

        public WebViewModel(INavigationService navigationService,
                            ITranslationService translationService,
                            IPopupService popupService, 
                            ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
           
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await  base.OnNavigatedToAsync(parameters);
            if (parameters.TryGetValue(Constants.ServiceNavigationKey, out string url))
                Url = url;
            
        }
    }
}
