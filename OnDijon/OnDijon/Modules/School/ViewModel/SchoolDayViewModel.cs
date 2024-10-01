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
using OnDijon.Modules.SchoolServices.Interfaces;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace OnDijon.Modules.School.ViewModels
{
    public class SchoolDayViewModel : BaseViewModel
    {
        public Action<bool> PropagteLoading = null;
        public Action<bool> PropagteModify = null;
        public override bool IsLoading
        {
            //get { return _isLoading; }
            set { if (PropagteLoading != null) PropagteLoading(value); }
        }

        readonly ISchoolRestaurantBookingConfigurationService MainService;

        private IList<AppointmentModel> _allAppointments;


        ObservableCollection<AppointmentViewModel> _schoolRestaurantBooking;
        public ObservableCollection<AppointmentViewModel> SchoolRestaurantBooking
        {
            get { return _schoolRestaurantBooking; }
            set
            {
                Set(ref _schoolRestaurantBooking, value);
                RaisePropertyChanged(nameof(SchoolRestaurantBooking));
            }
        }


        private bool _SchoolRestaurantBookingIsEmpty;
        public bool SchoolRestaurantBookingIsEmpty { get => _SchoolRestaurantBookingIsEmpty; set => Set(ref _SchoolRestaurantBookingIsEmpty, value); }

        public ChildByCityModel _child;
        public ChildByCityModel Child
        {
            get { return _child; }
            set
            {
                Set(ref _child, value);
            }
        }


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




        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                Set(ref _selectedDate, value);
                UpdateAppointements();
            }
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
                //PropagteModify(value);
            }
        }



        public ICommand LoadItemsCommand { get; set; }

        public ICommand SendReservations { get; set; }

        public ICommand ResetReservationCommand { get; set; }

        public SchoolDayViewModel(
            INavigationService navigationService, 
            ITranslationService translationService, 
            IPopupService popupService, 
            ISchoolRestaurantBookingConfigurationService service, 
            ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            MainService = service;
            SchoolRestaurantBooking = new ObservableCollection<AppointmentViewModel>();
            _allAppointments = new List<AppointmentModel>();
            LoadItemsCommand = new Command(LoaData);
            SendReservations = new Command(SendData);
            ResetReservationCommand = new Command(ResetReservation);
            SelectedDate = DateTime.Today;
            MinDate = DateTime.Today.AddDays(-7);
            MaxDate = DateTime.Today.AddMonths(1).Date;
            IsModify = false;
        }


        public void SendData()
        {
            if (IsModify)
            {
                CallApi(async () =>
                {
                    var toSend = SchoolRestaurantBooking.Where(el => el.IsModified).Select(el => el.Source).ToList();
                    var response = await MainService.SendBookingByCityContext(toSend, Child.CityContext);

                    ManageApiResponses(response, new DefaultCallbackManager<Response>(PopupService)
                    {
                        OnSuccess = (res) =>
                        {
                            PopupService.Show(PopupEnum.PopupSuccess, "Opération réalisée avec succès", "Les reservations ont bien été mises à jours.", "Continuer");
                            UpdateAppointements();
                        }
                    });
                });
            }
        }

        public void LoaData()
        {
            CallApi(async () =>
            {
                AppointmentListResponse response = await MainService.GetBookingList(Child.EditId, Child.SessionEditId, Child.CityContext);

                ManageApiResponses(response, new DefaultCallbackManager<AppointmentListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.childEditId == Child.EditId)
                        {
                            _allAppointments = res.AppointmentList;
                            UpdateAppointements();
                            InitDate();
                            IsLoading = false;
                        }
                    }
                });

            });
        }

        public void InitDate()
        {
            var dates = _allAppointments.Select(item => item.Date);
            // A remplacer coté WS
            var showdate = DateTime.Today.AddDays(1).DayOfWeek == DayOfWeek.Saturday ? DateTime.Today.AddDays(3) :
                            DateTime.Today.AddDays(1).DayOfWeek == DayOfWeek.Sunday ? DateTime.Today.AddDays(2) : DateTime.Today.AddDays(1);
            if (dates.Any())
            {
                //Init MaxDate avant MinDate sinon erreur quand MinDate > MaxDate avant init
                MaxDate = dates.Max();
                MinDate = dates.Min();
                SelectedDate = MinDate > showdate ? MinDate : showdate;
            }

        }

        internal void UpdateAppointementStatut()
        {
            if (onUpdating)
                return;
            SchoolRestaurantBooking.ForEach(s => s.IsOpen = !s.Source.IsClosed);
            foreach (AppointmentRuleModel appointmentRule in SchoolTool.GetAppointmentRules())
            {
                ConditionAppointementStatutConfiguration(appointmentRule.AppointmentId, appointmentRule.OppositeAppointmentid, appointmentRule.OppositeValue, appointmentRule.ClosingReason);
            }

        }

        private void ConditionAppointementStatutConfiguration(string appointmentId, string oppositeAppointmentId, bool oppositeValue, string closingReason)
        {

            var appointment = SchoolRestaurantBooking.FirstOrDefault(el => el.Source.ActivityCode == appointmentId);
            var oppositeAppointment = SchoolRestaurantBooking.FirstOrDefault(el => el.Source.ActivityCode == oppositeAppointmentId);

            if (appointment != null && oppositeAppointment != null)
            {
                bool closing = !appointment.Source.IsClosed && (oppositeAppointment.Scheduled == oppositeValue);
                appointment.IsOpen = appointment.IsOpen && closing;
                if (appointment.Source.IsClosed)
                {
                    appointment.ClosingReason = string.IsNullOrEmpty(appointment.ClosingReason) ? "Inscription clôturée" : appointment.ClosingReason;
                }
                else if (!closing)
                {
                    appointment.ClosingReason = closingReason;
                }
            }
        }


        private bool onUpdating = false;
        private void UpdateAppointements()
        {
            if (_allAppointments != null)
            {
                onUpdating = true;
                SchoolRestaurantBooking.Clear();
                foreach (var item in _allAppointments.Where(item => item.Date == SelectedDate))
                {
                    var vw = new AppointmentViewModel()
                    {
                        Parent = this,
                        Source = item,
                        Scheduled = item.Scheduled,
                        IsOpen = !item.IsClosed,
                        ClosingReason = item.SpecialDayLabel
                    };
                    //if (!vw.IsOpen)
                    //    vw.BackgroundColor = Color.DimGray;
                    SchoolRestaurantBooking.Add(vw);
                }
                SchoolRestaurantBookingIsEmpty = SchoolRestaurantBooking.Count == 0;
                onUpdating = false;
                UpdateModifyStatus();
                UpdateAppointementStatut();
            }
        }

        public void ResetReservation()
        {
            foreach (var item in SchoolRestaurantBooking)
            {
                if (item.IsModified)
                {
                    item.Scheduled = !item.Scheduled;
                }
            }
        }

        internal void UpdateModifyStatus()
        {
            if (onUpdating)
                return;
            IsModify = SchoolRestaurantBooking.Any(item => item.IsModified);
            //UpdateAppointementStatut();
        }
    }

    public class AppointmentViewModel : BindableObjectBase
    {
        public SchoolDayViewModel Parent { get; set; }
        public AppointmentModel Source { get; set; }

        private bool _CurrentScheduledValue;

        private bool? _LastScheduledValue = null;

        public bool Scheduled
        {
            get
            {
                return _CurrentScheduledValue;
            }
            set
            {
                _CurrentScheduledValue = value;

                if (_LastScheduledValue == null)
                    _LastScheduledValue = value;
                else
                {
                    Source.Scheduled = _CurrentScheduledValue;
                    Parent.UpdateModifyStatus();
                    Parent.UpdateAppointementStatut();
                }
                RaisePropertyChanged(nameof(Scheduled));
            }
        }


        bool _isOpen;
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

        public bool IsModified { get { return _LastScheduledValue.HasValue && (_LastScheduledValue != _CurrentScheduledValue); } }
    }

}



