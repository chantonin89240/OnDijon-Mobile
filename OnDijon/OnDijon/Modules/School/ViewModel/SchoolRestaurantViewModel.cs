
using System;
using System.Collections.ObjectModel;
using System.Linq;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Entities;
using OnDijon.Modules.School.Entities.Response;
using OnDijon.Modules.School.Models;
using OnDijon.Modules.School.Services.Interfaces;
using Prism.Navigation;

namespace OnDijon.Modules.School.ViewModels
{
    public class SchoolRestaurantViewModel : BaseViewModel
    {
        public Action<bool> PropagteLoading = null;

     
        public override bool IsLoading
        {
            //get { return _isLoading; }
            set { if (PropagteLoading != null) PropagteLoading(value); }
        }

        readonly IRSCalendar _schoolRestaurantCalendarService;

        private ObservableCollection<SchoolRestaurantCalendar> _schoolRestaurantData;

        private DateTime _minDate;
        public DateTime MinDate
        {
            get { return _minDate; }
            set
            {
                Set(ref _minDate, value);
            }
        }
        private DateTime _maxDate;
        public DateTime MaxDate
        {
            get { return _maxDate; }
            set
            {
                Set(ref _maxDate, value);
            }
        }


        private SchoolRestaurantCalendar _calendar;
        public SchoolRestaurantCalendar Calendar
        {
            get { return _calendar; }
            set
            {
                Set(ref _calendar, value);
            }
        }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                Set(ref _selectedDate, value);
                UpdateCalendar();
            }
        }




        // public ICommand LoadItemsCommand { get; set; }

        public SchoolRestaurantViewModel(INavigationService navigationService, ITranslationService translationService, IPopupService popupService, IRSCalendar schoolRestaurantCalendarService, ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _schoolRestaurantCalendarService = schoolRestaurantCalendarService;
            _schoolRestaurantData = new ObservableCollection<SchoolRestaurantCalendar>();
            //LoadItemsCommand = new Command(InitializeSchoolRestaurantCalendarList);
            SelectedDate = DateTime.Today;
            MinDate = DateTime.Today;
            MaxDate = DateTime.Today.AddMonths(1);
        }

        public void InitializeSchoolRestaurantCalendarList()
        {
            CallApi(async () =>
            {

                SchoolRestaurantCalendarListResponse response = await _schoolRestaurantCalendarService.GetMenusList();
                ManageApiResponses(response, new DefaultCallbackManager<SchoolRestaurantCalendarListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        _schoolRestaurantData.Clear();
                        foreach (var item in response.SchoolRestaurantCalendarList)
                        {
                            _schoolRestaurantData.Add(item);
                        }
                        InitDate();
                        UpdateCalendar();
                    }
                });

            });
        }

        private void InitDate()
        {
            var dates = _schoolRestaurantData.Select(calendar => calendar.Date);
            if (dates.Any())
            {
                MinDate = dates.Min();
                MaxDate = dates.Max();
            }

        }

        private void UpdateCalendar()
        {
            if (_schoolRestaurantData != null && _schoolRestaurantData.Any())
            {
                Calendar = _schoolRestaurantData.FirstOrDefault(item => item.Date.Date == SelectedDate.Date);
            }

        }


    }
}


