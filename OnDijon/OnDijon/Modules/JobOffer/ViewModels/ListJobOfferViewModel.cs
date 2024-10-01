using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.JobOffer.Entities.Models;
using OnDijon.Modules.JobOffer.Entities.Responses;
using OnDijon.Modules.JobOffer.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.JobOffer.ViewModels
{
    public class ListJobOfferViewModel : BaseViewModel
    {
        readonly IJobOfferService _JobOfferService;

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

        private List<JobOfferModel> _ListJobOffer;
        public List<JobOfferModel> ListJobOffer
        {
            get
            {
                return _ListJobOffer;
            }
            set
            {
                Set(ref _ListJobOffer, value);
            }
        }
        #endregion

        #region Commands
        public ICommand CloseCommand { get; }
        public ICommand SelectJobOfferCommand { get; set; }
        public ICommand GoToApplicationFormCommmand { get; set; }
        #endregion

        #region Appearing & Cleanup
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            GetJobOfferList();
        }

        public override void Cleanup()
        {
            base.Cleanup();
            SelectedJobOffer = null;
            ListJobOffer.Clear();
        }
        #endregion

        public void GetJobOfferList()
        {
            CallApi(async () =>
            {
                ListJobOfferResponse resp = await _JobOfferService.GetJobOffer();
                ManageApiResponses(resp, new DefaultCallbackManager<ListJobOfferResponse>(PopupService)
                {
                    OnSuccess = (res) =>

                    {
                        ListJobOffer = res.JobOfferList;
                    }
                });
            });
        }
        private void GetJobOfferDetail(JobOfferModel jobOffer)
        {
	        // DO Refacto : changer en Parametres de navigation 
            SelectedJobOffer = jobOffer;
            //Retire les tabulations qui génèrent des erreurs d'affichage
            SelectedJobOffer.Content = SelectedJobOffer.Content.Replace("\t", "");
            SelectedJobOffer.Conditions = SelectedJobOffer.Conditions.Replace("\t","");
            INavigationParameters param = new NavigationParameters
            {
                { Constants.JobOfferNavigationParameterKey, JsonConvert.SerializeObject(SelectedJobOffer ?? new JobOfferModel())},
            };
            //lancer la view
            NavigationService.NavigateAsync(Locator.DetailJobOfferPage,param);
        }

        private void GoToApplicationForm()
        {
	        // Refacto : changé en Parametres de navigation 
            var navParams = new NavigationParameters();
            navParams.Add(Constants.JobOfferNavigationParameterKey, JsonConvert.SerializeObject(SelectedJobOffer ?? new JobOfferModel()));
            navParams.Add(Constants.ApplicationFormFirstPartNavigationParameterKey, true );
            navParams.Add(Constants.IsBreadcrumbVisibleNavigationParameterKey, true );
            
            
            //lancer la view
            NavigationService.NavigateAsync(Locator.ApplicationFormPage, navParams);
        }

        public ListJobOfferViewModel(INavigationService navigationService,
                                     ITranslationService translationService,
                                     IPopupService popupService,
                                     IJobOfferService JobOfferService,
                                     ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _JobOfferService = JobOfferService;
            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            SelectJobOfferCommand = new DelegateCommand<JobOfferModel>(GetJobOfferDetail);
            GoToApplicationFormCommmand = new Command(GoToApplicationForm);
            ListJobOffer = new List<JobOfferModel>();

        }
    }
}
