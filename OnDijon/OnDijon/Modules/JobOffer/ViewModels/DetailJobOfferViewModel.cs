using Newtonsoft.Json;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.JobOffer.Entities.Models;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.JobOffer.ViewModels
{
    public class DetailJobOfferViewModel : BaseViewModel
    {

        #region Commands
        public ICommand CloseCommand { get; }
        public ICommand GoToAplicationPageCommand { get; set; }
        #endregion

        #region Properties
        private JobOfferModel _SelectedJobOffer;
        public JobOfferModel SelectedJobOffer
        {
            get
            {
                return _SelectedJobOffer;
            }
            set
            {
                Set(ref _SelectedJobOffer, value);
            }
        }

        #endregion

        #region Appearing & Cleanup
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);        
            if(parameters.TryGetValue(Constants.JobOfferNavigationParameterKey,out string jobOffer))
            {
                var jobModel = JsonConvert.DeserializeObject<JobOfferModel>(jobOffer);
                SelectedJobOffer = jobModel;
            }
        }
        #endregion

        private void GoToAplicationPage()
        {
	        // DO Refacto : changer en Parametres de navigation 
            INavigationParameters param = new NavigationParameters
            {
                { Constants.JobOfferNavigationParameterKey, JsonConvert.SerializeObject(SelectedJobOffer)},
                { Constants.IsFormNavigationParameterKey, true},
                { Constants.IsBreadcrumbVisibleNavigationParameterKey, false}
            };
            NavigationService.NavigateAsync(Locator.ApplicationFormPage,param);

        }
        public DetailJobOfferViewModel(INavigationService navigationService,
                                       ITranslationService translationService,
                                       IPopupService popupService,
                                       ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            GoToAplicationPageCommand = new Command(() => GoToAplicationPage());

        }
    }
}

