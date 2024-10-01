using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Diary.Entities.Model;
using OnDijon.Modules.Diary.Entities.Response;
using OnDijon.Modules.Diary.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace OnDijon.Modules.Diary.ViewModels
{
    public class EventDiaryListDashboardViewModel : BaseViewModel
    {
        private int EventNumber = 6;

        private IDiaryService DiaryService;

        private bool _eventListIsEmpty;
        public bool EventListIsEmpty { get => _eventListIsEmpty; set => Set(ref _eventListIsEmpty, value); }

        private IList<EventModel> _eventList;
        public IList<EventModel> EventList
        {
            get => _eventList;
            set
            {
                Set(ref _eventList, value);
            }
        }

        public ICommand LoadEventViewCommand { get; set; }
        public ICommand GotoEvenetDiaryListViewCommand { get; set; }

        public EventDiaryListDashboardViewModel(INavigationService navigationService,
                                                ITranslationService translationService,
                                                IPopupService popupService,
                                                IDiaryService diaryService, ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            DiaryService = diaryService;
            EventList = new ObservableCollection<EventModel>();
            LoadNews();
            LoadEventViewCommand = new DelegateCommand<EventModel>(LoadEventView);
            GotoEvenetDiaryListViewCommand = new DelegateCommand(GotoEvenetDiaryListView);
        }

        internal void UpdateEvents(List<EventModel> events)
        {
            EventList = events;
            EventListIsEmpty = !EventList.Any();
        }

        public override void Cleanup()
        {
            EventList = new ObservableCollection<EventModel>();
            EventListIsEmpty = true;
        }

        public void LoadNews()
        {
            CallApi(async () =>
            {
                EventListResponse response = await DiaryService.GetEventsByDate(DateTime.Today, DateTime.Today.AddDays(14), 0, EventNumber);
                ManageApiResponses(response, new DefaultCallbackManager<EventListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (response.Events.Any())
                        {
                            UpdateEvents(response.Events.ToList());
                        }
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
	                    // DO Refacto : use navigation parameters
                        var eventModel = new EventModel()
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
                            Tags = response.Tags != null && !string.IsNullOrEmpty(response.Tags.First()) ? response.Tags : null,
                            X = response.X,
                            Y = response.Y,
                            DiaryName = response.DiaryName,
                            Scope = response.Scope
                        };
                        INavigationParameters param = new NavigationParameters
                        {
                            { Constants.EventNavigationParameterKey, eventModel}
                        };
                        NavigationService.NavigateAsync(Locator.EventDetailsPage,param);
                    }
                });
            });
        }

        private void GotoEvenetDiaryListView()
        {
            Analytics.TrackEvent(Constants.NavigationMenuEvents[Constants.DiaryCode]);
            NavigateTo(Locator.EventListPage);
        }
    }
}
