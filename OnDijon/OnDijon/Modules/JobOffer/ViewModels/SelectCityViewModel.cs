using Newtonsoft.Json;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.JobOffer.Entities.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.JobOffer.ViewModels
{
    public class SelectCityViewModel : BaseViewModel
    {

        public ICommand CloseCommand { get; }
        public ICommand SelectCityCommand { get; set; }

        #region Properties
        private List<string> _ListCitiesSpontaneousApplication;
        public List<string> ListCitiesSpontaneousApplication
        {
            get { return _ListCitiesSpontaneousApplication; }
            set { Set(ref _ListCitiesSpontaneousApplication, value); }
        }
        private string _SelectedCitySpontaneousApplication;
        public string SelectedCitySpontaneousApplication
        {
            get { return _SelectedCitySpontaneousApplication; }
            set { Set(ref _SelectedCitySpontaneousApplication, value); }
        }
        #endregion

        #region Appearing & Cleanup
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            ListCitiesSpontaneousApplication = new List<string> { "Dijon", "Quetigny" };
        }

        public override void Cleanup()
        {
            base.Cleanup();
            ListCitiesSpontaneousApplication.Clear();
        }
        #endregion

        private void SelectCity (string Selectedcity)
        {
	        // DO Refacto : changer en Parametres de navigation 
            var jobOffer = new JobOfferModel() { City = Selectedcity};
            INavigationParameters param = new NavigationParameters
            {
                { Constants.JobOfferNavigationParameterKey, JsonConvert.SerializeObject(jobOffer)},
            };
            NavigationService.NavigateAsync(Locator.ApplicationFormPage);
        }

        public SelectCityViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IPopupService popupService,
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            CloseCommand = new Command(() => 
                                           NavigationService.GoBackAsync());
            SelectCityCommand = new DelegateCommand<string>(SelectCity);
            ListCitiesSpontaneousApplication = new List<string>();
        }
    }
}
