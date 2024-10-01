using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Model;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Entities.Models;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.JobOffer.Entities;
using OnDijon.Modules.JobOffer.Entities.Models;
using OnDijon.Modules.JobOffer.Entities.Requests;
using OnDijon.Modules.JobOffer.Entities.Responses;
using OnDijon.Modules.JobOffer.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Extensions;
using Xamarin.Forms;

namespace OnDijon.Modules.JobOffer.ViewModels
{
    public class ApplicationFormViewModel : BaseViewModel
    {
        readonly ISession _session;
        readonly IJobOfferService _JobOfferService;

        #region Commands
        public ICommand CloseCommand { get; }
        public ICommand SendJobApplication { get; set; }
        public ICommand GoDashboardCommand { get; set; }
        public ICommand GoBack { get; set; }
        public ICommand SelectCityCommand { get; set; }
        public ICommand SelectJobTypeCommand { get; set; }
        #endregion

        #region Properties
        public ProfileModel Profile { get; set; }
        private IList<string> _documentExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".odt" };
        public IList<string> DocumentExtensions
        {
            get
            {
                return _documentExtensions;
            }
        }

        private ValidatableJobApplicationRequest _JobApplicationDocRequest;
        public ValidatableJobApplicationRequest JobApplicationDocRequest
        {
            get
            {
                return _JobApplicationDocRequest;
            }
            set
            {
                Set(ref _JobApplicationDocRequest, value);
            }
        }

        private string _Mr;
        public string Mr
        {
            get => _Mr;
            set
            {
                Set(ref _Mr, value);
                CivilityErrorDisplay = false;
            }
        }

        private string _Mrs;
        public string Mrs
        {
            get => _Mrs;
            set
            {
                Set(ref _Mrs, value);
                CivilityErrorDisplay = false;
            }
        }

        private string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                Set(ref _Title, value);
            }
        }

        private bool _CivilityErrorDisplay;
        public bool CivilityErrorDisplay
        {
            get => _CivilityErrorDisplay;
            set
            {
                Set(ref _CivilityErrorDisplay, value);
            }
        }


        private string _CVTitle;
        public string CVTitle
        {
            get
            {
                return _CVTitle;
            }
            set
            {
                Set(ref _CVTitle, value);
            }
        }

        private string _CoverLetterTitle;
        public string CoverLetterTitle
        {
            get
            {
                return _CoverLetterTitle;
            }
            set
            {
                Set(ref _CoverLetterTitle, value);
            }
        }


        private bool _CancelReinitialize = false;
        public bool CancelReinitialize
        {
            get
            {
                return _CancelReinitialize;
            }
            set
            {
                Set(ref _CancelReinitialize, value);
            }
        }

        private AddressModel _ApplicantAddress;
        public AddressModel ApplicantAddress
        {
            get
            {
                return _ApplicantAddress;
            }
            set
            {
                Set(ref _ApplicantAddress, value);
            }
        }

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


        private List<string> _ListCitiesSpontaneousApplication;
        public List<string> ListCitiesSpontaneousApplication
        {
            get { return _ListCitiesSpontaneousApplication; }
            set { Set(ref _ListCitiesSpontaneousApplication, value); }
        }

        private bool _FirstPartForm;
        public bool FirstPartForm
        {
            get { return _FirstPartForm; }
            set { Set(ref _FirstPartForm, value); }
        }

        private bool _SecondPartForm;
        public bool SecondPartForm
        {
            get { return _SecondPartForm; }
            set { Set(ref _SecondPartForm, value); }
        }

        private bool _ThirdPartForm;
        public bool ThirdPartForm
        {
            get { return _ThirdPartForm; }
            set
            {
                Set(ref _ThirdPartForm, value);
                if(_ThirdPartForm == true)
                {
                    DisplayGoBackPopup = true;
                }
                else
                {
                    DisplayGoBackPopup = false;
                }
            }
        }

        private bool _FourthPartForm;
        public bool FourthPartForm
        {
            get { return _FourthPartForm; }
            set { Set(ref _FourthPartForm, value); }
        }

        private List<string> _ListTypeJobOffer;
        public List<string> ListTypeJobOffer
        {
            get { return _ListTypeJobOffer; }
            set { Set(ref _ListTypeJobOffer, value); }
        }

        private bool _DisplayBreadcrumbView;
        public bool DisplayBreadcrumbView
        {
            get { return _DisplayBreadcrumbView; }
            set { Set(ref _DisplayBreadcrumbView, value); }
        }

        private bool _DisplayGoBackPopup;
        public bool DisplayGoBackPopup
        {
            get { return _DisplayGoBackPopup; }
            set { Set(ref _DisplayGoBackPopup, value); }
        }
        #endregion

        #region Appearing & Cleanup
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            if (parameters.TryGetValue(Constants.JobOfferNavigationParameterKey, out string jobOffer))
                SelectedJobOffer = JsonConvert.DeserializeObject<JobOfferModel>(jobOffer);
            if (parameters.TryGetValue(Constants.IsFormNavigationParameterKey, out bool thirdPart))
                ThirdPartForm = thirdPart;
            if (parameters.TryGetValue(Constants.ApplicationFormFirstPartNavigationParameterKey, out bool firstPart))
                FirstPartForm = firstPart;
            if (parameters.TryGetValue(Constants.IsBreadcrumbVisibleNavigationParameterKey, out bool isBCVisible))
                DisplayBreadcrumbView = isBCVisible;
            if (CancelReinitialize == false)
            {
                InitData(); 
                ConstructTitle();
                CancelReinitialize = true;
            }

        }

        private void ConstructTitle()
        {
            if(SelectedJobOffer != null && SelectedJobOffer.Title != null)
            {
                Title = "Poste ciblé : " + SelectedJobOffer.Title;
            }
            else
            {
                Title = "Pour une candidature spontanée";
            }

        }
        public void InitData()
        {
            JobApplicationDocRequest = new ValidatableJobApplicationRequest()
            {
                JobApplicationRequest = new JobApplicationRequest() { ApplicantAddressModel = new AddressModel() }
            };
            InitJobApplicationRequestFromProfile();
            ListCitiesSpontaneousApplication = new List<string> { "Dijon", "Quetigny" };
        }

        public void InitJobApplicationRequestFromProfile()
        {
            Mr = Profile.Gender == RecipeUIConstants.GenderMale ? RecipeUIConstants.GenderMale : null;
            Mrs = Profile.Gender == RecipeUIConstants.GenderFemale ? RecipeUIConstants.GenderFemale : null;
            JobApplicationDocRequest.ApplicantEmail.Value = Profile.Mail;
            JobApplicationDocRequest.ApplicantName.Value = Profile.Name;
            JobApplicationDocRequest.ApplicantFirstName.Value = Profile.FirstName;
            JobApplicationDocRequest.ApplicantPhone.Value = Profile.PhoneNumber;
        }

        public override void Cleanup()
        {
            if (CancelReinitialize == false)
            {
                base.Cleanup();
                JobApplicationDocRequest = new ValidatableJobApplicationRequest()
                {
                    JobApplicationRequest = new JobApplicationRequest() { ApplicantAddressModel = new AddressModel() }
                };
                CivilityErrorDisplay = false;
                CVTitle = null;
                CoverLetterTitle = null;
                SelectedJobOffer = null;
                Mr = null;
                Mrs = null;
                ApplicantAddress = new AddressModel();
                SelectedJobOffer = new JobOfferModel();
                ListCitiesSpontaneousApplication = new List<string>();
                ListTypeJobOffer = new List<string>();
                FirstPartForm = false;
                SecondPartForm = false;
                ThirdPartForm = false;
                FourthPartForm = false;
                DisplayBreadcrumbView = true;
                DisplayGoBackPopup = false;
            }
        }

        private void CleanAndClose()
        {
            CancelReinitialize = false;
            Cleanup();
            _session.Cleanup();
            NavigationService.NavigateAsync(Locator.DashboardView);
        }
        #endregion

        public void SendJobApplicationCommand()
        {
            CompleteRequest();
            if (JobApplicationDocRequest.Validate())
            {
                SendJobApplicationAPICall();
                CancelReinitialize = true;
            }
            CivilityErrorDisplay = string.IsNullOrEmpty(JobApplicationDocRequest.Civility.Value);
        }

        private void CompleteRequest()
        {
            JobApplicationDocRequest.JobApplicationRequest.Civility = JobApplicationDocRequest.Civility.Value;
            JobApplicationDocRequest.JobApplicationRequest.ApplicantEmail = JobApplicationDocRequest.ApplicantEmail.Value;
            JobApplicationDocRequest.JobApplicationRequest.ApplicantName = JobApplicationDocRequest.ApplicantName.Value;
            JobApplicationDocRequest.JobApplicationRequest.ApplicantFirstName = JobApplicationDocRequest.ApplicantFirstName.Value;
            JobApplicationDocRequest.JobApplicationRequest.ApplicantPhone = JobApplicationDocRequest.ApplicantPhone.Value;
            JobApplicationDocRequest.JobApplicationRequest.UserId = Profile.Guid;
            JobApplicationDocRequest.JobApplicationRequest.ApplicantCVTitle = CVTitle;
            JobApplicationDocRequest.JobApplicationRequest.ApplicantCoverLetterTitle = CoverLetterTitle;
            JobApplicationDocRequest.JobApplicationRequest.City = SelectedJobOffer.City;
            JobApplicationDocRequest.JobApplicationRequest.Type = SelectedJobOffer.Type;
            JobApplicationDocRequest.JobApplicationRequest.ApplicantCV = JobApplicationDocRequest.ApplicantCV.Value;
            JobApplicationDocRequest.JobApplicationRequest.ApplicantCoverLetter = JobApplicationDocRequest.ApplicantCoverLetter.Value;
            JobApplicationDocRequest.JobApplicationRequest.ApplicantAddressModel = JobApplicationDocRequest.ApplicantAddressModel.Value;
            if (SelectedJobOffer != null)
            {
                JobApplicationDocRequest.JobApplicationRequest.EditIdJobOffer = SelectedJobOffer.EditId;
            }
            JobApplicationDocRequest.Civility.Value = new[] { Mr, Mrs }.FirstOrDefault(civility => !string.IsNullOrEmpty(civility));
        }

        public void SendJobApplicationAPICall()
        {
            CallApi(async () =>
            {

                Response response = await _JobOfferService.SendApplication(JobApplicationDocRequest.JobApplicationRequest);
                ManageApiResponses(response, new DefaultCallbackManager<Response>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        FourthPartForm = true;
                        ThirdPartForm = false;
                    },
                    OnError = (res) =>
                    {
                        PopupService.Show(PopupEnum.PopupError, "Une erreur est survenue", "Votre demande n'a pas pu aboutir, veuillez réessayer ultérieurement.", "Retour", () =>
                        {
                            CleanAndClose();
                        });
                    }
                });
            });
        }

        public void GoBackCommand()
        {
            CancelReinitialize = false;
            if(DisplayGoBackPopup == true)
            {
                PopupService.Show(PopupEnum.PopupInfo, "Attention", "Vous allez perdre votre saisie actuelle.", "Quitter", () =>
                {
                    NavigateTo(Locator.ListJobOfferPage, true );
                }, "Annuler");
            }
            else
            {
                NavigationService.NavigateTo(Locator.ListJobOfferPage, true);
            }
        }

        private void SelectCity(string Selectedcity)
        {
            SelectedJobOffer.City = Selectedcity;
            FirstPartForm = false;
            SecondPartForm = true;
            GetTypeJobOffer();
        }

        private void SelectJobType(string SelectedJobType)
        {
            SelectedJobOffer.Type = SelectedJobType;
            SecondPartForm = false;
            ThirdPartForm = true;
        }

        public void GetTypeJobOffer()
        {
            CallApi(async () =>
            {
                ListTypeJobOfferResponse resp = await _JobOfferService.GetTypeJobOffer(SelectedJobOffer.City);
                ManageApiResponses(resp, new DefaultCallbackManager<ListTypeJobOfferResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        ListTypeJobOffer = res.TypeJobOfferList;
                    }
                });
            });
        }
        public ApplicationFormViewModel(INavigationService navigationService,
                                        ITranslationService translationService,
                                        IPopupService popupService,
                                        ISession session,
                                        IJobOfferService JobOfferService,
                                        ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            Profile = _session.Profile;
            _JobOfferService = JobOfferService;

            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            SendJobApplication = new Command(SendJobApplicationCommand);
            GoBack = new Command(GoBackCommand);
            GoDashboardCommand = new Command(CleanAndClose);
            SelectCityCommand = new DelegateCommand<string>(SelectCity);
            SelectJobTypeCommand = new DelegateCommand<string>(SelectJobType);
        }
    }
}
