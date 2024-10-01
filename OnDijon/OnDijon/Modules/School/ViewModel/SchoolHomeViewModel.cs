using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities;
using OnDijon.Modules.School.Entities.Models;
using OnDijon.Modules.School.Entities.Response;
using OnDijon.Modules.School.Services.Interfaces;
using OnDijon.Modules.School.ViewModels;
using OnDijon.Modules.School.Views;
using OnDijon.Modules.SchoolServices.Interfaces;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;
using AsyncAwaitBestPractices.MVVM;
using Prism.Commands;
using System.Collections.Generic;

namespace OnDijon.Modules.School.ViewModel
{
    public class SchoolHomeViewModel : BaseViewModel
    {

        readonly ISession _session;
        readonly ISchoolCardConfigurationService _schoolCardConfigurationService;
        readonly ISchoolRestaurantBookingConfigurationService _schoolRestaurantBookingConfigurationService;

        #region visibility var
        private bool _SchoolRestaurantIsVisible;
        public bool SchoolRestaurantIsVisible
        {
            get
            {
                return _SchoolRestaurantIsVisible;
            }
            set
            {
                _SchoolRestaurantIsVisible = value;
                RaisePropertyChanged(nameof(SchoolRestaurantIsVisible));
            }
        }
        private bool _SchoolDayIsVisibleIsVisible;
        public bool SchoolDayIsVisible
        {
            get
            {
                return _SchoolDayIsVisibleIsVisible;
            }
            set
            {
                _SchoolDayIsVisibleIsVisible = value;
                RaisePropertyChanged(nameof(SchoolDayIsVisible));
            }
        }
        private bool _moreFunctionIsVisible;
        public bool MoreFunctionIsVisible
        {
            get
            {
                return _moreFunctionIsVisible;
            }
            set
            {
                _moreFunctionIsVisible = value;
                RaisePropertyChanged(nameof(MoreFunctionIsVisible));
            }
        }
        private bool _SchoolScheduledIsVisible;
        public bool SchoolScheduledIsVisible
        {
            get
            {
                return _SchoolScheduledIsVisible;
            }
            set
            {
                Set(ref _SchoolScheduledIsVisible, value);
            }
        }
        #endregion

        #region Properties
        private bool _isLeftArrowVisible = false;
        public bool IsLeftArrowVisible
        {
            get { return _isLeftArrowVisible; }
            set
            {
                Set(ref _isLeftArrowVisible, value);
            }
        }

        private bool _isRightArrowVisible = true;
        public bool IsRightArrowVisible
        {
            get { return _isRightArrowVisible; }
            set
            {
                Set(ref _isRightArrowVisible, value);
            }
        }

        private string _pageCounter;
        public string PageCounter
        {
            get { return _pageCounter; }
            set
            {
                Set(ref _pageCounter, value);
            }
        }

        ObservableCollection<ChildCardModel> _schoolCardList;
        public ObservableCollection<ChildCardModel> SchoolCardList
        {
            get { return _schoolCardList; }
            set
            {
                Set(ref _schoolCardList, value);
            }
        }

        private ChildCardModel _selectedSchoolCard;
        public ChildCardModel SelectedSchoolCard
        {
            get
            {
                return _selectedSchoolCard;
            }
            set
            {
                _selectedSchoolCard = value;
                PageCounter = (SchoolCardList.IndexOf(value) + 1) + "/" + SchoolCardList.Count;
                IsLeftArrowVisible = SchoolCardList.IndexOf(value) != 0;
                IsRightArrowVisible = SchoolCardList.IndexOf(value) + 1 != SchoolCardList.Count;
                if (_selectedSchoolCard != null)
                {
                    if (SelectedSchoolCard.Type == SchoolCardType.Child)
                        UpdateDayScheduledByChild();
                    if (SelectedSchoolCard.Type == SchoolCardType.Restaurant)
                        UpdatSchoolRestaurant();
                    UpdateVisibility();
                }
                RaisePropertyChanged(nameof(SelectedSchoolCard));
            }
        }

        private SchoolRestaurantViewModel _SchoolRestaurantModel;
        public SchoolRestaurantViewModel SchoolRestaurantModel
        {
            get => _SchoolRestaurantModel;
            set => Set(ref _SchoolRestaurantModel, value);
        }

        private SchoolDayViewModel _dayScheduled;
        public SchoolDayViewModel DayScheduled
        {
            get => _dayScheduled;
            set => Set(ref _dayScheduled, value);
        }

        private WeekSchedulingViewModel _weekScheduling;
        public WeekSchedulingViewModel WeekScheduling
        {
            get => _weekScheduling;
            set => Set(ref _weekScheduling, value);
        }

        private DietViewModel _diet;
        public DietViewModel Diet
        {
            get => _diet;
            set => Set(ref _diet, value);
        }

        private bool _isModify;
        public bool IsModify
        {
            get => _isModify;
            set => Set(ref _isModify, value);
        }

        private IDictionary<string, string> SessionEditIdByCityContext;

        public string sessionScheduledHelper { get; set; }

        #endregion

        #region Commands
        public ICommand LoadItemsCommand { get; set; }
        public ICommand OpenHelp { get; set; }
        public ICommand WeekButtonCommand { get; set; }
        public ICommand DayButtonCommand { get; set; }
        #endregion

        public SchoolHomeViewModel(INavigationService navigationService,
            ITranslationService translationService,
            IPopupService popupService,
            ISchoolCardConfigurationService schoolCardConfigurationService,
            ISchoolRestaurantBookingConfigurationService service,
            ISession session,
            ILoggerService loggerService) 
            : base(navigationService, translationService, popupService, loggerService)
        {
            _schoolRestaurantBookingConfigurationService = service;
            SchoolCardList = new ObservableCollection<ChildCardModel>();
            _schoolCardConfigurationService = schoolCardConfigurationService;
            _session = session;
            SchoolDayIsVisible = false;
            SchoolRestaurantIsVisible = false;
            SchoolScheduledIsVisible = false;
        
            SchoolRestaurantModel = App.Locator.GetInstance<SchoolRestaurantViewModel>();
            SchoolRestaurantModel.PropagteLoading = b => IsLoading = b;
            DayScheduled = App.Locator.GetInstance<SchoolDayViewModel>();
            DayScheduled.PropagteLoading = b => IsLoading = b;
            DayScheduled.PropagteModify = b => IsModify = b;
            WeekScheduling = App.Locator.GetInstance<WeekSchedulingViewModel>();
            WeekButtonCommand = new DelegateCommand(OnWeekButtonCommand);
            DayButtonCommand = new DelegateCommand(OnDayButtonCommand);
            Diet = App.Locator.GetInstance<DietViewModel>();

            LoadItemsCommand = new Command(async () => await Initialize());
            OpenHelp = new Command(OpenHelpCommand);
            IsModify = true;
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
	        await Initialize();
        }

        public async Task Initialize()
        {
            SchoolDayIsVisible = false;
            SchoolRestaurantIsVisible = false;
            SchoolScheduledIsVisible = false;
            RequestActiveSessions();
        }

        private void OnWeekButtonCommand()
        {
            if(!IsModify)
            {
                SchoolScheduledIsVisible = true;
                SchoolDayIsVisible = false;
            }
        }

        private void OnDayButtonCommand()
        {
            if (!IsModify)
            {
                SchoolScheduledIsVisible = false;
                SchoolDayIsVisible = true;
            }
        }

        private void OpenHelpCommand()
        {
            if (SchoolRestaurantIsVisible)
            {
                PopupService.Show(new PopupMenuView());
            }
            else
            {
                PopupService.Show(new PopupRestaurantView(sessionScheduledHelper));
            }
        }

        private void UpdatSchoolRestaurant()
        {
            SchoolRestaurantModel.InitializeSchoolRestaurantCalendarList();
        }

        private void UpdateDayScheduledByChild() 
        {
            IsLoading = true;
            DayScheduled.Child = SelectedSchoolCard.ChildModel;
            DayScheduled.LoaData();
            WeekScheduling.Child = SelectedSchoolCard.ChildModel;
            WeekScheduling.LoadData();
            Diet.Child = SelectedSchoolCard.ChildModel;
            Diet.LoadData();
        }


        private void UpdateVisibility() {
            SchoolRestaurantIsVisible = SelectedSchoolCard.Type == SchoolCardType.Restaurant;
            SchoolDayIsVisible = SelectedSchoolCard.Type == SchoolCardType.Child;
            SchoolScheduledIsVisible = false;
        }

        private void RequestActiveSessions()
        {
            CallApi(async () =>
            {
                string ediId = _session.Profile.Guid;
                SessionResponse response = await _schoolRestaurantBookingConfigurationService.GetActiveSessionByCityContext();
                ManageApiResponses(response, new DefaultCallbackManager<SessionResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        //TODO glisser le grp citycontext/EditIdSession partout 
                        SessionEditIdByCityContext = res.SessionsEditIdByCityContext;
                        InitializeCardsList();
                    }
                });
            });
        }

        private void InitializeCardsList()
        {
            CallApi(async () =>
            {
                string ediId = _session.Profile.Guid;
                ChildResponse response = await _schoolCardConfigurationService.GetChilds(ediId, SessionEditIdByCityContext);
                ManageApiResponses(response, new DefaultCallbackManager<ChildResponse>(PopupService)
                {
                    OnSuccess = (res) => {
                        sessionScheduledHelper = res.SessionScheduledHelper;
                        SchoolCardList.Clear();
                        foreach (ChildCardModel schoolRestaurantChild in res.SchoolCardList)
                        {
                            SchoolCardList.Add(schoolRestaurantChild);
                        }
                    }
                });
            });
        }


    }
}
