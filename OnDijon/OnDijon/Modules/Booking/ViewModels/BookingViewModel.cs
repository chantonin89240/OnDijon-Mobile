using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Extensions;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Entities.Models;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Booking.Entities;
using OnDijon.Modules.Booking.Entities.Models;
using OnDijon.Modules.Booking.Entities.Responses;
using OnDijon.Modules.Booking.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.Booking.ViewModels
{
    public class BookingViewModel : BaseViewModel
    {

        private const string Site = "EtatCivil";
        readonly ISession _session;
        readonly IBookingService _BookingService;

        #region Properties
        public ProfileModel Profile { get; set; }

        private string _mr;
        public string Mr
        {
            get => _mr;
            set
            {
                Set(ref _mr, value);
                CivilityErrorIsVisible = false;
            }
        }

        private string _mrs;
        public string Mrs
        {
            get => _mrs;
            set
            {
                Set(ref _mrs, value);
                CivilityErrorIsVisible = false;
            }
        }

        private string _editIdSession;
        public string EditIdSession { get => _editIdSession; set => _editIdSession = value; }

        private string _selectedCity;
        public string SelectedCity
        {
            get
            {
                return _selectedCity;
            }
            set
            {
                _selectedCity = value;
                if (_selectedCity != null)
                {
                    GetInstitutionList();
                }
            }
        }
        private string _cni;
        public string CNI
        {
            get => _cni;
            set
            {
                _cni = value;
                RequestReasonErrorIsVisible = !(!string.IsNullOrEmpty(CNI) || !string.IsNullOrEmpty(Passeport));
            }
        }
        private string _passeport;
        public string Passeport
        {
            get => _passeport;
            set
            {
                _passeport = value;
                RequestReasonErrorIsVisible = !(!string.IsNullOrEmpty(CNI) || !string.IsNullOrEmpty(Passeport));
            }
        }

        public int _selectedPersonNumber { get; set; }
        public int SelectedPersonNumber
        {
            get
            {
                return _selectedPersonNumber;
            }
            set
            {
                _selectedPersonNumber = value;
                UpdateSelectedPersonNumber();
            }
        }

        private IList<int> _listNumberOfPerson;
        public IList<int> ListNumberOfPerson
        {
            get => _listNumberOfPerson;
            set
            {
                _listNumberOfPerson = value;
                if (_listNumberOfPerson != null)
                {
                    RaisePropertyChanged(nameof(ListNumberOfPerson));
                }
            }
        }
        private IList<string> _cities;
        public IList<string> Cities
        {
            get => _cities;
            set
            {
                _cities = value;
                if (_cities != null)
                {
                    RaisePropertyChanged(nameof(Cities));
                }
            }
        }

        #endregion

        #region ModelObject
        private InstitutionModel _selectedInstitution;
        private ViewScheduleModel _selectedDateSchedule;
        private IList<ViewScheduleModel> _schedulesDate;
        private IList<ScheduleModel> _schedules;
        private IList<InstitutionModel> _institutions;
        private ScheduleModel _selectedSchedule;
        private ValidatableBookingIdentitydocumentRequest _identityDocumentRequest;

        public InstitutionModel SelectedInstitution
        {
            get
            {
                return _selectedInstitution;
            }
            set
            {
                Set(ref _selectedInstitution, value);
                if (_selectedInstitution != null)
                {
                    UpdateSelectedInstitution();
                }
            }
        }
        public ViewScheduleModel SelectedDateSchedule
        {
            get
            {
                return _selectedDateSchedule;
            }
            set
            {
                _selectedDateSchedule = value;
                if (_selectedDateSchedule != null)
                {
                    UpdateSelectedDate();
                }
            }
        }
        public IList<ViewScheduleModel> SchedulesDate { get => _schedulesDate; set => Set(ref _schedulesDate, value); }
        public IList<ScheduleModel> Schedules { get => _schedules; set => Set(ref _schedules, value); }
        public IList<InstitutionModel> Institutions { get => _institutions; set => Set(ref _institutions, value); }
        public ScheduleModel SelectedSchedule
        {
            get
            {
                return _selectedSchedule;
            }
            set
            {
                _selectedSchedule = value;
                if (_selectedSchedule != null)
                {
                    UpdateSelectedSchedule();
                }
            }
        }

        public ValidatableBookingIdentitydocumentRequest BookingItyDocRequest { get => _identityDocumentRequest; set => Set(ref _identityDocumentRequest, value); }
        #endregion

        #region VisibleFields
        private bool _reasonVisible;
        private bool _bookingVisible;
        private bool _chooseHourVisible;
        private bool _contactInformationVisible;
        private bool _chooseInstitutionVisible;
        private bool _choosePersonNumberVisible;
        private bool _chooseDateVisible;
        private bool _submitButtonVisible;
        private bool _requestReasonErrorIsVisible;
        private bool _civilityErrorIsVisible;
        private bool _confirmVisible;
        private bool _scheduleUnavaibleVisible;
        public bool ReasonVisible { get => _reasonVisible; set => Set(ref _reasonVisible, value); }
        public bool BookingVisible { get => _bookingVisible; set => Set(ref _bookingVisible, value); }
        public bool ChooseHourVisible { get => _chooseHourVisible; set => Set(ref _chooseHourVisible, value); }
        public bool ChooseInstitutionVisible { get => _chooseInstitutionVisible; set => Set(ref _chooseInstitutionVisible, value); }
        public bool ContactInformationVisible { get => _contactInformationVisible; set => Set(ref _contactInformationVisible, value); }
        public bool ChoosePersonNumberVisible { get => _choosePersonNumberVisible; set => Set(ref _choosePersonNumberVisible, value); }
        public bool ChooseDateVisible { get => _chooseDateVisible; set => Set(ref _chooseDateVisible, value); }
        public bool SubmitButtonVisible { get => _submitButtonVisible; set => Set(ref _submitButtonVisible, value); }
        public bool RequestReasonErrorIsVisible { get => _requestReasonErrorIsVisible; set => Set(ref _requestReasonErrorIsVisible, value); }
        public bool CivilityErrorIsVisible { get => _civilityErrorIsVisible; set => Set(ref _civilityErrorIsVisible, value); }
        public bool ConfirmVisible { get => _confirmVisible; set => Set(ref _confirmVisible, value); }
        public bool ScheduleUnavaibleVisible { get => _scheduleUnavaibleVisible; set => Set(ref _scheduleUnavaibleVisible, value); }
        #endregion

        #region Commands
        public ICommand ValidIdentityDocumentCommand { get; set; }
        public ICommand GetSchedulesCommand { get; set; }
        public ICommand GoToUsefulContactCommand { get; set; }
        public ICommand SendBook { get; set; }
        public ICommand GoDashboardCommand { get; set; }
        public ICommand ReasonChoice { get; set; }

        #endregion

        public BookingViewModel(INavigationService navigationService,
                                ITranslationService translationService,
                                IPopupService popupService,
                                ISession session,
                                IBookingService bookingService, 
                                ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _BookingService = bookingService;
            Profile = _session.Profile;

            ValidIdentityDocumentCommand = new Command(IdentityDocumentListAction);
            GetSchedulesCommand = new Command(GetScheduleList);
            SendBook = new Command(SendBookCommand);
            ReasonChoice = new Command(ReasonChoiceCommand);
            GoToUsefulContactCommand = new DelegateCommand(async () => { await NavigationService.NavigateAsync(Locator.ContactMapPage); });
            GoDashboardCommand = new DelegateCommand(async () => await Close());

            SelectedPersonNumber = -1;
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            InitData();
        }

        public void InitData()
        {
            BookingItyDocRequest = new ValidatableBookingIdentitydocumentRequest();
            InitIdentityRequestFromProfile();

            ReasonVisible = true;
            ContactInformationVisible = false;
            ChooseInstitutionVisible = false;
            ChoosePersonNumberVisible = false;
            ChooseDateVisible = false;
            ChooseHourVisible = false;
            BookingVisible = false;
            RequestReasonErrorIsVisible = false;
            CivilityErrorIsVisible = false;
            SelectedPersonNumber = -1;
            Cities = RecipeUIConstants.CityChoices.ToList();
        }

        public async Task Close()
        {
            async Task goBackAsync() => await NavigationService.GoBackToPageKey(Locator.DashboardView);

            if (!ReasonVisible)
            {
                PopupService.Show(PopupEnum.PopupInfo, "Attention", "Vous allez perdre votre saisie actuelle.", "Quitter", async () =>
                {
                    await goBackAsync();
                }, "Annuler");
            }
            else
            {
                await goBackAsync();
            }

        }

        private void UpdateSelectedPersonNumber()
        {
            if (SelectedPersonNumber != -1)
            {
                GetSchedulesCommand.Execute(null);
            }
            else
            {
                ChooseDateVisible = false;
            }
        }

        private void UpdateSelectedInstitution()
        {
            ListNumberOfPerson = Enumerable.Range(1, SelectedInstitution.MaxNumberOfPerson).ToList();
            SelectedPersonNumber = ListNumberOfPerson.Count() == 1 ? ListNumberOfPerson.First() : -1;
            ChoosePersonNumberVisible = true;
            ChooseDateVisible = false;
            ChooseHourVisible = false;
            SubmitButtonVisible = false;
            ScheduleUnavaibleVisible = false;
        }

        private void UpdateSelectedDate()
        {
            if (SelectedDateSchedule != null)
            {
                Schedules = SelectedDateSchedule.ScheduleModels.ToList();
                ChooseHourVisible = true;
                SubmitButtonVisible = false;
            }
            else
            {
                ChooseHourVisible = false;
                SubmitButtonVisible = false;
                Schedules?.Clear();
            }
        }

        private void UpdateSelectedSchedule()
        {
            SubmitButtonVisible = true;
        }




        public void IdentityDocumentListAction()
        {
            BookingItyDocRequest.DocumentCivility.Value = new[] { Mr, Mrs }.FirstOrDefault(civility => !string.IsNullOrEmpty(civility));
            if (BookingItyDocRequest.Validate())
            {
                ContactInformationVisible = false;
                BookingVisible = true;
            }
            CivilityErrorIsVisible = string.IsNullOrEmpty(BookingItyDocRequest.DocumentCivility.Value);
        }

        private void GetInstitutionList()
        {
            ScheduleUnavaibleVisible = false;
            ChoosePersonNumberVisible = false;
            ChooseDateVisible = false;
            ChooseHourVisible = false;
            IsLoading = true;
            CallApi(async () =>
            {
                InstitutionListResponse response = await _BookingService.GetBookingInstitutionsList(SelectedCity, Site);
                ManageApiResponses(response, new DefaultCallbackManager<InstitutionListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.Institutions.Any())
                        {
                            Institutions = res.Institutions.ToList();
                            EditIdSession = res.SessionEditId;
                            ChooseInstitutionVisible = true;
                            IsLoading = false;
                        }
                    },
                });
            });
        }
        public void GetScheduleList()
        {
            IsLoading = true;
            CallApi(async () =>
            {
                ScheduleListResponse response = await _BookingService.GetScheduleList(SelectedCity, EditIdSession, SelectedInstitution.EditId, SelectedPersonNumber, DateTime.Now, DateTime.Now.AddDays(SelectedInstitution.OpeningTime));
                ManageApiResponses(response, new DefaultCallbackManager<ScheduleListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.Schedules.Any())
                        {
                            IEnumerable<IGrouping<DateTime, ScheduleModel>> groupSchedules = res.Schedules.GroupBy(s => s.StartDate.Date).ToArray();
                            IList<ViewScheduleModel> viewScheduleModels = groupSchedules.Select(grp =>
                            {
                                return new ViewScheduleModel()
                                {
                                    Day = grp.Key,
                                    ScheduleModels = grp.ToArray()
                                };
                            }).ToList();
                            SchedulesDate = viewScheduleModels;
                            ChooseDateVisible = true;
                            ScheduleUnavaibleVisible = false;
                        }
                        else
                        {
                            ChooseDateVisible = false;
                            ScheduleUnavaibleVisible = true;
                        }
                        IsLoading = false;
                    }
                });
            });
        }
        public void InitIdentityRequestFromProfile()
        {
            BookingItyDocRequest.BookingRequest.ApplicantCivility = Profile.Gender;
            BookingItyDocRequest.BookingRequest.ApplicantName = Profile.Name;
            BookingItyDocRequest.BookingRequest.ApplicantFirstName = Profile.FirstName;
            BookingItyDocRequest.BookingRequest.ApplicantEmail = Profile.Mail;
            BookingItyDocRequest.BookingRequest.ApplicantAddressNumber = Profile.StreetNumber;
            BookingItyDocRequest.BookingRequest.ApplicantAddressStreet = Profile.Street;
            BookingItyDocRequest.BookingRequest.ApplicantAddressPostalCode = Profile.PostalCode;
            BookingItyDocRequest.BookingRequest.ApplicantAddressCity = Profile.City;

            BookingItyDocRequest.DocumentCivility.Value = Profile.Gender;
            Mr = Profile.Gender == RecipeUIConstants.GenderMale ? RecipeUIConstants.GenderMale : null;
            Mrs = Profile.Gender == RecipeUIConstants.GenderFemale ? RecipeUIConstants.GenderFemale : null;
            BookingItyDocRequest.DocumentName.Value = Profile.Name;
            BookingItyDocRequest.DocumentFirstName.Value = Profile.FirstName;
            BookingItyDocRequest.DocumentBirthDate.Value = Profile.Birthday;
            BookingItyDocRequest.ApplicantPhone.Value = Profile.PhoneNumber;
        }
        public void SendBookCommand()
        {
            IsLoading = true;
            CallApi(async () =>
            {
                BookingItyDocRequest.BookingRequest.DocumentCivility = new[] { Mr, Mrs }.FirstOrDefault(civility => !string.IsNullOrEmpty(civility));
                BookingItyDocRequest.BookingRequest.ScheduleEditId = SelectedSchedule.EditId;
                BookingItyDocRequest.BookingRequest.UserId = Profile.Guid;
                BookingItyDocRequest.BookingRequest.SessionEditId = EditIdSession;
                BookingItyDocRequest.BookingRequest.ConfigurationSite = Site;
                BookingItyDocRequest.BookingRequest.NbOfPerson = SelectedPersonNumber;

                Response response = await _BookingService.SendBook(BookingItyDocRequest.BookingRequest, SelectedCity);
                ManageApiResponses(response, new DefaultCallbackManager<Response>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        ConfirmVisible = true;
                        BookingVisible = false;
                        IsLoading = false;
                    },
                    OnError = (res) =>
                    {
                        IsLoading = false;
                        PopupService.Show(PopupEnum.PopupError, "Une erreur est survenue", "Votre demande n'a pas pu aboutir, veuillez réessayer ultérieurement.", "Retour", async () =>
                        {
                            await NavigationService.GoBackToPageKey(Locator.DashboardView);
                        });
                    }
                });
            });

        }

        public void ReasonChoiceCommand()
        {
            BookingItyDocRequest.RequestReason.Value = new[] { CNI, Passeport }.Where(val => !string.IsNullOrEmpty(val)).ToArray();
            if (BookingItyDocRequest.ValidateRequestReason())
            {
                RequestReasonErrorIsVisible = false;
                ReasonVisible = false;
                ContactInformationVisible = true;
            }
            else
            {
                RequestReasonErrorIsVisible = true;
            }
        }
    }
}
