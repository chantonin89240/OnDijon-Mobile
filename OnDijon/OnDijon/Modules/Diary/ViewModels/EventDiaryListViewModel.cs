using OnDijon.Common.Entities;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Views;
using OnDijon.Modules.Diary.Entities.Model;
using OnDijon.Modules.Diary.Entities.Response;
using OnDijon.Modules.Diary.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnDijon.Modules.Diary.ViewModels
{
    public class EventDiaryListViewModel : BaseViewModel
    {

        private int EventNumberPerPage = 20;
        private double MaxDaysResearch = 90;
        private DateTime StartDate = DateTime.Today;
        private DateTime EndDate;
        private bool SearchModeOn = false;
        private bool CurrentLoadActive = false;
        public bool CommeWithQuery = false;


        private DateTime Today { get; set; }
        private int PageIndex { get; set; }
        private int MaxPageIndex { get; set; }

        private bool _existingMore;
        public bool ExistingMore { get => _existingMore; set => Set(ref _existingMore, value); }

        private IDiaryService DiaryService;

        private bool _eventListIsEmpty;
        public bool EventListIsEmpty { get => _eventListIsEmpty; set => Set(ref _eventListIsEmpty, value); }

        private string _querySearch;
        public string QuerySearch
        {
            get => _querySearch;
            set
            {
                Set(ref _querySearch, value.ToLower());
                if (!CommeWithQuery)
                {
                    SuggestionListShow = true;
                    Suggest();
                }
            }
        }

        private IList<string> _suggestionList;
        public IList<string> SuggestionList { get => _suggestionList; set => Set(ref _suggestionList, value); }

        private bool _suggestionListShow;
        public bool SuggestionListShow { get => _suggestionListShow; set => Set(ref _suggestionListShow, value); }

        private string _selectedQuery;
        public string SelectedQuery
        {
            get => _selectedQuery;
            set
            {
                Set(ref _selectedQuery, value);
            }
        }

        private ObservableCollection<EventModel> _eventList;
        public ObservableCollection<EventModel> EventList
        {
            get => _eventList;
            set
            {
                Set(ref _eventList, value);
                EventListIsEmpty = !(_eventList.Count > 0);
            }
        }

        private bool _isEventDetailDisplay = false;
        public bool IsEventDetailDisplay
        {
            get { return _isEventDetailDisplay; }
            set { Set(ref _isEventDetailDisplay, value); }
        }

        private DateTime? _chosenDateStart;
        public DateTime? ChosenDateStart
        {
            get { return _chosenDateStart; }
            set { Set(ref _chosenDateStart, value); }
        }


        private string _weekendButtonText = DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? "Le week-end prochain" : "Ce week-end";
        public string WeekendButtonText
        {
            get { return _weekendButtonText; }
            set { Set(ref _weekendButtonText, value); }
        }


        public ICommand LoadEventViewCommand { get; set; }
        public ICommand SearchSelectCommand { get; set; }
        public ICommand ResetSearchCommand { get; set; }
        public ICommand TodaySearchCommand { get; set; }
        public ICommand TomorrowSearchCommand { get; set; }
        public ICommand WeekendSearchCommand { get; set; }
        public ICommand FromDateSearchCommand { get; set; }

        #region EventDiaryViewModel => EventDiaryViewModel

        private EventDetailDiaryViewModel _eventDetailDiaryViewModel;

        public EventDetailDiaryViewModel EventDetailDiaryViewModel
        {
            get => _eventDetailDiaryViewModel;
            set => SetProperty(ref _eventDetailDiaryViewModel, value);
        }

        #endregion
        
        

        public EventDiaryListViewModel(INavigationService navigationService,
                                       ITranslationService translationService,
                                       IPopupService popupService,
                                       IDiaryService diaryService, 
                                       ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            EndDate = DateTime.Today.AddDays(MaxDaysResearch);
            DiaryService = diaryService;
            LoadEventViewCommand = new DelegateCommand<EventModel>(LoadEventView);
            SearchSelectCommand = new DelegateCommand<string>(SearchEvents);
            ResetSearchCommand = new DelegateCommand(() => { NewResearchWithNewDates(DateTime.Today, DateTime.Today.AddDays(MaxDaysResearch)); });
            TodaySearchCommand = new DelegateCommand(() => { NewResearchWithNewDates(DateTime.Today, DateTime.Today.AddDays(1).AddMilliseconds(-1)); });
            TomorrowSearchCommand = new DelegateCommand(() => { NewResearchWithNewDates(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2).AddMilliseconds(-1)); });
            WeekendSearchCommand = new DelegateCommand(() => { NewResearchWithNewDates(GetNextWeekEnd(DateTime.Today), GetNextWeekEnd(DateTime.Today).AddDays(2).AddMilliseconds(-1)); });
            FromDateSearchCommand = new DelegateCommand(NewResearchWithSelectedStartDate);

            EventDetailDiaryViewModel = App.Locator.GetInstance<EventDetailDiaryViewModel>();
            EventDetailDiaryViewModel.ParentEventDiaryListViewModel = this;
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            Today = DateTime.Today;
            if(parameters.TryGetValue(Constants.QueryNavigationParameterKey, out string query))
            {
                CommeWithQuery = true;
                LaunchSearchFromOtherPage(query);
            }
            if (!CommeWithQuery)
            {
                CleanEvents();
                LoadNews();
            }

            if (!CommeWithQuery)
                QuerySearch = "";
            else
                CommeWithQuery = false;
        }

        public void NewResearchWithNewDates(DateTime startDay, DateTime endDate)
        {
            QuerySearch = "";
            StartDate = startDay;
            EndDate = endDate;
            CleanEvents();
            LoadNews();
        }

        public void NewResearchWithSelectedStartDate()
        {
            PopupNavigation.Instance.PushAsync(new DatePickerPopupView(OnDateSelected, ChosenDateStart),true);
        }

        private void OnDateSelected(DateTime newDate)
        {
            ChosenDateStart = newDate;
            NewResearchWithNewDates(newDate, newDate.AddDays(MaxDaysResearch));
        }


        internal void UpdateEvents(List<EventModel> events)
        {
            DateTime testDate;
            bool firtToday = true;
            events.OrderBy(i => i.StartDate).ToList().ForEach(e => {
                testDate = e.StartDate.Value;
                if (firtToday && testDate < DateTime.Today.AddDays(1))
                {
                    firtToday = false;
                    e.IsFirstOfDate = true;
                    e.FirstOfDateString = "Aujourd\'hui";
                }
                else if (testDate.Year > Today.Year || testDate.Month > Today.Month || testDate.Day > Today.Day)
                {
                    Today = testDate;
                    e.IsFirstOfDate = true;
                    e.FirstOfDateString = e.StartDate?.ToString("dddd d MMMM", CultureInfo.CreateSpecificCulture("fr-FR"));
                    e.FirstOfDateString = char.ToUpper(e.FirstOfDateString[0]) + e.FirstOfDateString.Substring(1);
                }
                EventList.Add(e);
            });
            EventListIsEmpty = !EventList.Any();
            CurrentLoadActive = false;
        }

        public override void Cleanup()
        {
            CleanEvents();
        }

        private void CleanEvents()
        {
            PageIndex = 0;
            EventList = new ObservableCollection<EventModel>();
            EventListIsEmpty = true;
        }

        public void LoadMoreEvents()
        {
            if (!CurrentLoadActive)
            {
                CurrentLoadActive = true;
                ExistingMore = false;
                PageIndex++;
                if (SearchModeOn)
                {
                    SearchQuery();
                }
                else
                {
                    LoadNews();
                }
            }
        }

        private void SearchEvents(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                QuerySearch = query;
                SuggestionListShow = false;
                if (!CurrentLoadActive)
                {
                    CurrentLoadActive = true;
                    CleanEvents();
                    SearchQuery();
                }
            }
        }

        public void LaunchSearchFromOtherPage(string query)
        {
            IsEventDetailDisplay = false;
            SearchEvents(query);
        }

        public bool GoBack()
        {
            if (IsEventDetailDisplay)
            {
                IsEventDetailDisplay = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        #region CallApi

        public void Suggest()
        {
            // if (!SearchLaunched)
            if (!string.IsNullOrEmpty(QuerySearch) && QuerySearch.Length > 2)
            {
                GetSuggestions(QuerySearch);
            }
            else
            {
                SuggestionListShow = false;
                if (SearchModeOn && !CurrentLoadActive)
                {
                    CurrentLoadActive = true;
                    CleanEvents();
                    LoadNews();
                }
            }
        }

        private void LoadNews()
        {
            CallApi(async () =>
            {
                EventListResponse response = await DiaryService.GetEventsByDate(StartDate, EndDate, PageIndex, EventNumberPerPage);
                ManageApiResponses(response, new DefaultCallbackManager<EventListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (response.Events.Any())
                        {
                            UpdateEvents(response.Events.ToList());
                        }
                        MaxPageIndex = res.PageMax;
                        ExistingMore = PageIndex < MaxPageIndex;
                        SearchModeOn = false;
                    }
                });
            });
        }

        private void LoadEventView(EventModel eventItem)
        {
            CallApi(async () =>
            {
                EventResponse response = await DiaryService.GetEventDetails(eventItem.EditId);
                ManageApiResponses(response, new DefaultCallbackManager<EventResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        EventDetailDiaryViewModel.EventDetail = new EventModel()
                                                                {
                                                                    Title = response.Title,
                                                                    EditId = response.EditId,
                                                                    Address = response.Address,
                                                                    City = response.City,
                                                                    Description = response.Description,
                                                                    DiaryEditId = response.DiaryEditId,
                                                                    District = response.District,
                                                                    EndDate = response.EndDate,
                                                                    Image = response.Image,
                                                                    ImageThumbnail = response.ImageThumbnail,
                                                                    InfoLink = response.InfoLink,
                                                                    Location = response.Location,
                                                                    PostalCode = response.PostalCode,
                                                                    PricingInfo = response.PricingInfo,
                                                                    StartDate = response.StartDate,
                                                                    Summary = response.Summary,
                                                                    Tags = response.Tags,
                                                                    X = response.X,
                                                                    Y = response.Y,
                                                                    DiaryName = response.DiaryName,
                                                                    Scope = response.Scope
                                                                };
                        IsEventDetailDisplay = true;
                    }
                });
            });
        }

        private void GetSuggestions(string query)
        {
            CallApi(async () =>
            {
                DiarySuggestionResponse response = await DiaryService.GetSuggestions(query, DateTime.Today);
                ManageApiResponses(response, new DefaultCallbackManager<DiarySuggestionResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        SuggestionList = res.Results.ToList();
                    }
                });
            });
        }

        public void SearchQuery()
        {
            IsLoading = true;
            CallApi(async () =>
            {
                EventListResponse response = await DiaryService.GetEventsByRequest(QuerySearch, DateTime.Today.AddDays(0), PageIndex, EventNumberPerPage);
                ManageApiResponses(response, new DefaultCallbackManager<EventListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (response.Events.Any())
                        {
                            UpdateEvents(response.Events.ToList());
                        }
                        else
                        {
                            CurrentLoadActive = false;
                        }
                        MaxPageIndex = res.PageMax;
                        ExistingMore = PageIndex < MaxPageIndex;
                        SearchModeOn = true;
                    }
                });
            });
        }
        #endregion

        public DateTime GetNextWeekEnd(DateTime dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
            if(diff > 5)
            {
                return dt.AddDays((double)((-1 * diff) + 12)).Date;
            }
            else
            {
                return dt.AddDays((double)((-1 * diff) + 5)).Date;
            }
        }
    }
}
