using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Response;
using OnDijon.Modules.School.Entities.Models;
using OnDijon.Modules.School.Entities.Response;
using OnDijon.Modules.School.Tools;
using OnDijon.Modules.School.ViewModel;
using OnDijon.Modules.SchoolServices.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace OnDijon.Modules.School.ViewModels
{
    public class WeekSchedulingViewModel : BaseViewModel
    {
        public Action<bool> PropagteLoading = null;
        public Action<bool> PropagteModify = null;

        readonly ISchoolRestaurantBookingConfigurationService MainService;

        #region SchoolHomeViewModel => SchoolHomeViewModel

        private SchoolHomeViewModel _schoolHomeViewModel;

        public SchoolHomeViewModel SchoolHomeViewModel { get => _schoolHomeViewModel; set => SetProperty(ref _schoolHomeViewModel, value); }

        #endregion

        private WeeklyScheduleModel _weeklySchedule;
        public WeeklyScheduleModel WeeklySchedule
        {
            get { return _weeklySchedule; }
            set { Set(ref _weeklySchedule, value); }
        }

        private ChildByCityModel _child;
        public ChildByCityModel Child
        {
            get { return _child; }
            set { Set(ref _child, value); }
        }

        private List<string> DayList = new List<string>() { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi" };

        private ObservableCollection<DayCalendarViewModel> _calendarActivityDays;
        public ObservableCollection<DayCalendarViewModel> CalendarActivityDays
        {
            get { return _calendarActivityDays; }
            set { Set(ref _calendarActivityDays, value); }
        }

        private string _stepName;
        public string StepName
        {
            get { return _stepName; }
            set { Set(ref _stepName, value); }
        }

        private bool _isModify;
        public bool IsModify
        {
            get { return _isModify; }
            set
            {
                Set(ref _isModify, value);
                RaisePropertyChanged(nameof(IsModify));
                if (PropagteModify != null) PropagteModify(value);
            }
        }

        private bool _registered;
        public bool Registered
        {
            get { return _registered; }
            set { Set(ref _registered, value); }
        }

        public ICommand GoNextStep { get; set; }
        public ICommand GoPreviousStep { get; set; }
        public ICommand GoUpdate { get; set; }
        public ICommand ResetReservationCommand { get; set; }

        public WeekSchedulingViewModel(INavigationService navigationService,
                                       ITranslationService translationService,
                                       IPopupService popupService,
                                       ISchoolRestaurantBookingConfigurationService service,
                                       ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            MainService = service;
            CalendarActivityDays = new ObservableCollection<DayCalendarViewModel>();
            GoUpdate = new Command(Update);
            ResetReservationCommand = new Command(ResetReservation);
            IsModify = false;

            //SchoolHomeViewModel = App.Locator.School;
        }

        public void LoadData()
        {
            CallApi(async () =>
            {
                WeeklyScheduleResponse response = await MainService.GetParentSchedule(Child.EditId, Child.SessionEditId, Child.CityContext);

                ManageApiResponses(response, new DefaultCallbackManager<WeeklyScheduleResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        Registered = true;
                        if (res.ChildEditId == Child.EditId)
                        {
                            WeeklySchedule = new WeeklyScheduleModel()
                            {
                                EditId = res.EditId,
                                StartDate = res.StartDate,
                                EndDate = res.EndDate,
                                Schedule = res.Schedule
                            };
                            ActualizeCurrentCalendarActivities();
                        }
                        IsLoading = false;
                    },
                    OnInvalidInformations = (res) =>
                    {
                        //enfant non inscrit à l'école
                        CalendarActivityDays.Clear();
                        Registered = false;
                        IsLoading = false;
                    }
                });
            });
        }

        private void ActualizeCurrentCalendarActivities()
        {
            //StepName = DayList[ActualDayNumber];
            CalendarActivityDays = new ObservableCollection<DayCalendarViewModel>();
            foreach (string day in DayList)
            {
                DayCalendarViewModel dayCalendar = new DayCalendarViewModel()
                {
                    ActualDay = day,
                    Childs = new ObservableCollection<CalendarActivitiesViewModel>()
                };
                WeeklySchedule.Schedule.Where(item => string.Equals(item.ActivityDay, day))
                    .OrderBy((item) => item.Order).ToList()
                    .ForEach(i => dayCalendar.Childs.Add(new CalendarActivitiesViewModel()
                    {
                        Source = i,
                        Parent = this,
                        IsCheck = i.IsCheck,
                        Day = dayCalendar
                    }
                ));
                CalendarActivityDays.Add(dayCalendar);
                UpdateCalendarActivityStatut(dayCalendar.Childs);
            }
        }
        private void Update()
        {
            UpdateDataParentSchedule();
        }

        private void RefreshViewAndNavigateToOnDayScheduled()
        {
            SchoolHomeViewModel.Initialize();
        }

        public void UpdateDataParentSchedule()
        {
            CallApi(async () =>
            {
                Response response = await MainService.UpdateParentScheduleByCityContext(WeeklySchedule, Child.CityContext);
                ManageApiResponses(response, new DefaultCallbackManager<Response>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        PopupService.Show(PopupEnum.PopupSuccess, res.Message, "OK", RefreshViewAndNavigateToOnDayScheduled);
                    }
                });
            });
        }

        private void UpdateCalendarActivityStatut(IList<CalendarActivitiesViewModel> activities)
        {
            activities.ForEach(s => s.IsOpen = true);
            foreach (AppointmentRuleModel appointmentRule in SchoolTool.GetAppointmentRules())
            {
                ConditionCalendarActivityStatutConfiguration(activities, appointmentRule.AppointmentId, appointmentRule.OppositeAppointmentid, appointmentRule.OppositeValue, appointmentRule.ClosingReason);
            }

        }

        private void ConditionCalendarActivityStatutConfiguration(IList<CalendarActivitiesViewModel> activities, string appointmentId, string oppositeAppointmentId, bool oppositeValue, string closingReason)
        {
            CalendarActivitiesViewModel appointment = activities.FirstOrDefault(ca => ca.Source.ActivityCode == appointmentId);
            CalendarActivitiesViewModel oppositeAppointment = activities.FirstOrDefault(ca => ca.Source.ActivityCode == oppositeAppointmentId);
            if (appointment != null && oppositeAppointment != null)
            {
                bool closing = oppositeAppointment.IsCheck == oppositeValue;
                appointment.IsOpen = appointment.IsOpen && closing;
                if (!closing)
                {
                    appointment.ClosingReason = closingReason;
                }
            }
        }

        public void ResetReservation()
        {
            foreach (var day in CalendarActivityDays)
            {
                foreach (var ca in day.Childs)
                {
                    if (ca.IsModified)
                    {
                        ca.IsCheck = !ca.IsCheck;
                    }
                }
            }
        }

        internal void UpdateModifyStatus()
        {
            IsModify = CalendarActivityDays.Any(day => day.Childs.Any(ca => ca.IsModified));
        }

        public class DayCalendarViewModel : BindableObjectBase
        {
            public string ActualDay { get; set; }
            public IList<CalendarActivitiesViewModel> Childs { get; set; }
        }

        public class CalendarActivitiesViewModel : BindableObjectBase
        {
            public WeekSchedulingViewModel Parent { get; set; }
            public DayCalendarViewModel Day { get; set; }
            public CalendarActivityModel Source { get; set; }

            private bool _CurrentCheckValue;
            private bool? _LastCheckValue = null;

            public bool IsCheck
            {
                get
                {
                    return _CurrentCheckValue;
                }
                set
                {
                    _CurrentCheckValue = value;

                    if (_LastCheckValue == null)
                        _LastCheckValue = value;
                    else
                    {
                        Source.IsCheck = _CurrentCheckValue;
                        Parent.UpdateModifyStatus();
                        Parent.UpdateCalendarActivityStatut(Day.Childs);
                    }
                    RaisePropertyChanged(nameof(IsCheck));
                }
            }

            bool _isOpen = true;
            public bool IsOpen
            {
                get { return _isOpen; }
                set { Set(ref _isOpen, value); }
            }

            string _closingReason;
            public string ClosingReason
            {
                get { return _closingReason; }
                set { Set(ref _closingReason, value); }
            }
            public bool IsModified { get { return _LastCheckValue.HasValue && (_LastCheckValue != _CurrentCheckValue); } }
        }
    }

}



