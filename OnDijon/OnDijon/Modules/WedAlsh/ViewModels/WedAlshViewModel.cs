using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Entities;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.WedAlsh.Entities.Models;
using OnDijon.Modules.WedAlsh.Entities.Request;
using OnDijon.Modules.WedAlsh.Entities.Response;
using OnDijon.Modules.WedAlsh.Services.Interfaces;
using OnDijon.Modules.WedAlsh.Views;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace OnDijon.Modules.WedAlsh.ViewModels
{
    public class WedAlshViewModel : BaseViewModel
    {

        readonly ISession _session;
        readonly IWedAlshService WedAlshService;

        #region Properties
        private RegistrationWedAlshViewModel _registrationSelected;
        public RegistrationWedAlshViewModel RegistrationSelected
        {
            get { return _registrationSelected; }
            set
            {
                Set(ref _registrationSelected, value);
            }
        }

        private List<RegistrationWedAlshViewModel> _registrationsOfChildSelected;
        public List<RegistrationWedAlshViewModel> RegistrationsOfChildSelected
        {
            get { return _registrationsOfChildSelected; }
            set
            {
                Set(ref _registrationsOfChildSelected, value);
            }
        }

        private bool _multipleSelectionRegistrationIsVisible = false;
        public bool MultipleSelectionRegistrationIsVisible
        {
            get
            {
                return _multipleSelectionRegistrationIsVisible;
            }
            set
            {
                Set(ref _multipleSelectionRegistrationIsVisible, value);
            }
        }


        private WedAlshChildModel _childSelected;
        public WedAlshChildModel ChildSelected
        {
            get { return _childSelected; }
            set
            {
                PageCounter = (Childs.IndexOf(value) + 1) + "/" + Childs.Count();
                IsLeftArrowVisible = Childs.IndexOf(value) != 0;
                IsRightArrowVisible = Childs.IndexOf(value) + 1 != Childs.Count;
                Set(ref _childSelected, value);
                LoadSelection();
            }
        }

        private List<WedAlshChildModel> _childs;
        public List<WedAlshChildModel> Childs
        {
            get { return _childs; }
            set
            {
                Set(ref _childs, value);
            }
        }
        private bool _isModify = false;
        public bool IsModify
        {
            get { return _isModify; }
            set
            {
                Set(ref _isModify, value);
                RaisePropertyChanged(nameof(IsModify));
            }
        }

        private bool _isLeftArrowVisible = false;
        public bool IsLeftArrowVisible
        {
            get { return _isLeftArrowVisible; }
            set
            {
                Set(ref _isLeftArrowVisible, value);
                RaisePropertyChanged(nameof(IsLeftArrowVisible));
            }
        }

        private bool _isRightArrowVisible = true;
        public bool IsRightArrowVisible
        {
            get { return _isRightArrowVisible; }
            set
            {
                Set(ref _isRightArrowVisible, value);
                RaisePropertyChanged(nameof(IsRightArrowVisible));
            }
        }

        private string _pageCounter;
        public string PageCounter
        {
            get { return _pageCounter; }
            set
            {
                Set(ref _pageCounter, value);
                RaisePropertyChanged(nameof(PageCounter));
            }
        }

        public ICommand OpenHelp { get; set; }
        public ICommand Validate { get; set; }
        public ICommand Annulate { get; set; }
        public ICommand CarouselChangeCommand { get; set; }
        #endregion

        public WedAlshViewModel(INavigationService navigationService,
                                ITranslationService translationService,
                                IPopupService popupService,
                                ISession session,
                                IWedAlshService wedAlshService,
                                ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            WedAlshService = wedAlshService;
            OpenHelp = new Command(OpenHelpCommand);
            Validate = new Command(ValidateCommand);
            Annulate = new Command(AnnulateCommand);
            CarouselChangeCommand = new Command<WedAlshChildModel>(item => ChildSelected = item != null ? item : ChildSelected);
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            LoadData();
        }

        #region Appels WS
        public void LoadData()
        {
            CallApi(async () =>
            {
                WedAlshRegistrationsResponse response = await WedAlshService.GetRegistrations(_session.Profile.Guid);

                ManageApiResponses(response, new DefaultCallbackManager<WedAlshRegistrationsResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        Childs = res.Childs;
                        ChildSelected = Childs.FirstOrDefault();

                    }
                });
            });
        }

        public void UpdateData()
        {
            CallApi(async () =>
            {
                if (RegistrationSelected.IsModify)
                {

                    //création de la liste des changement
                    List<ScheduleAction> updatedList = new List<ScheduleAction>();
                    RegistrationSelected.Months.ForEach(month => month.Days.ForEach(d =>
                    {
                        if (d.NoonSchedule.IsModified)
                        {
                            updatedList.Add(new ScheduleAction()
                            {
                                Checked = d.NoonSchedule.IsCheck,
                                EditId = d.NoonSchedule.Source.EditId
                            });
                        }
                        if (d.AfternoonSchedule.IsModified)
                        {
                            updatedList.Add(new ScheduleAction()
                            {
                                Checked = d.AfternoonSchedule.IsCheck,
                                EditId = d.AfternoonSchedule.Source.EditId
                            });
                        }
                    }));

                    WedAlshSchedulesResponse response = await WedAlshService.UpdateRegistrations(RegistrationSelected.EditId, updatedList);

                    ManageApiResponses(response, new DefaultCallbackManager<WedAlshSchedulesResponse>(PopupService)
                    {
                        OnSuccess = (res) =>
                        {
                            PopupService.Show(PopupEnum.PopupSuccess, res.Message, "OK");
                            ValidateChange(res.Schedules);
                        },
                        OnError = (res) =>
                        {
                            PopupService.Show(PopupEnum.PopupError, res.Message, "OK");
                        }
                    });
                }

            });
        }
        #endregion

        #region Fonctions de traitement et de création
        public void LoadSelection()
        {
            if (ChildSelected != null)
            {
                var newRegistrationsOfChildSelected = new List<RegistrationWedAlshViewModel>();
                ChildSelected.Registrations.ForEach((r) =>
                {
                    var newRegistration = new RegistrationWedAlshViewModel()
                    {
                        Name = r.CentreAccueil.Title,
                        EditId = r.EditId,
                        Parent = this,
                        Months = new List<MonthWedAlshViewModel>()
                    };

                    //Ajout par mois
                    MonthWedAlshViewModel actualMonth = new MonthWedAlshViewModel(this)
                    {
                        Month = DateTime.Now.Month,
                        Name = char.ToUpper(DateTime.Now.ToString("MMMM")[0]) + DateTime.Now.ToString("MMMM").Substring(1),
                        Days = new List<DayWedAlshViewModel>()
                    };
                    DayWedAlshViewModel day = new DayWedAlshViewModel();
                    r.Schedules.ForEach(s =>
                    {
                        if (day.DayOfYear != s.StartDate.Value.DayOfYear)
                        {
                            if (day.Name != null)
                            {
                                day.VerifyOpenCondition();
                                actualMonth.Days.Add(day);
                            }
                            day = new DayWedAlshViewModel()
                            {
                                DayOfYear = s.StartDate.Value.DayOfYear,
                                Name = char.ToUpper(s.StartDate.Value.ToString("dddd d")[0]) + s.StartDate.Value.ToString("dddd d").Substring(1),
                            };
                        }
                        if (s.StartDate.Value.Month != actualMonth.Month)
                        {
                            if (actualMonth.Days.Any())
                            {
                                newRegistration.Months.Add(actualMonth);
                            }
                            actualMonth = new MonthWedAlshViewModel(this)
                            {
                                Month = s.StartDate.Value.Month,
                                Name = char.ToUpper(s.StartDate.Value.ToString("MMMM")[0]) + s.StartDate.Value.ToString("MMMM").Substring(1),
                                Days = new List<DayWedAlshViewModel>()
                            };
                        }
                        if (s.ScheduleType == "Repas")
                        {
                            day.NoonSchedule = new WedAlshScheduleViewModel()
                            {
                                Source = s,
                                Month = actualMonth,
                                IsOpen = !s.IsClosed,
                                IsCheck = s.IsBooked,
                                ClosingReason = s.IsClosedLabel
                            };
                        }
                        else
                        {
                            day.AfternoonSchedule = new WedAlshScheduleViewModel()
                            {
                                Source = s,
                                Month = actualMonth,
                                IsOpen = !s.IsClosed,
                                IsCheck = s.IsBooked,
                                ClosingReason = s.IsClosedLabel
                            };
                        }
                    });
                    day.VerifyOpenCondition();
                    actualMonth.Days.Add(day);
                    newRegistration.Months.Add(actualMonth);

                    //ajout d'un élément registration complet
                    newRegistrationsOfChildSelected.Add(newRegistration);
                });
                RegistrationsOfChildSelected = new List<RegistrationWedAlshViewModel>(newRegistrationsOfChildSelected);
                RegistrationSelected = RegistrationsOfChildSelected.FirstOrDefault();
                MultipleSelectionRegistrationIsVisible = RegistrationsOfChildSelected != null && RegistrationsOfChildSelected.Count > 1;
            }
        }

        private void CleanChange()
        {
            RegistrationSelected.Months.ForEach(m =>
           {
               m.Days.ForEach(d =>
               {
                   if (d.NoonSchedule.IsModified)
                   {
                       d.NoonSchedule.IsCheck = d.NoonSchedule.LastCheckValue.Value;
                   }
                   if (d.AfternoonSchedule.IsModified)
                   {
                       d.AfternoonSchedule.IsCheck = d.AfternoonSchedule.LastCheckValue.Value;
                   }
               });
           });
        }

        private void ValidateChange(List<WedAlshScheduleModel> schedulesReponse)
        {
            RegistrationSelected.Months.ForEach(m =>
            {
                m.Days.ForEach(s =>
                {
                    if (s.NoonSchedule.IsModified && schedulesReponse.Any(sr => sr.EditId == s.NoonSchedule.Source.EditId))
                    {
                        s.NoonSchedule.LastCheckValue = schedulesReponse.FirstOrDefault(sr => sr.EditId == s.NoonSchedule.Source.EditId).IsBooked;
                        s.NoonSchedule.IsCheck = (bool)s.NoonSchedule.LastCheckValue;
                    }
                    if (s.AfternoonSchedule.IsModified && schedulesReponse.Any(sr => sr.EditId == s.AfternoonSchedule.Source.EditId))
                    {
                        s.AfternoonSchedule.LastCheckValue = schedulesReponse.FirstOrDefault(sr => sr.EditId == s.AfternoonSchedule.Source.EditId).IsBooked;
                        s.AfternoonSchedule.IsCheck = (bool)s.AfternoonSchedule.LastCheckValue;
                    }
                });
            });
        }
        #endregion

        #region Commands
        private void OpenHelpCommand()
        {
            PopupService.Show(new PopupInfoWedAlshView());
        }

        private void ValidateCommand()
        {
            UpdateData();
        }

        private void AnnulateCommand()
        {
            CleanChange();
        }
        #endregion

        #region ViewModelBases
        public class RegistrationWedAlshViewModel : BindableObjectBase
        {
            public string Name { get; set; }
            public string EditId { get; set; }
            public List<MonthWedAlshViewModel> Months { get; set; }
            public WedAlshViewModel Parent { get; set; }
            private bool _isModify = false;
            public bool IsModify
            {
                get { return _isModify; }
                set
                {
                    Set(ref _isModify, value);
                    Parent.IsModify = IsModify;
                }
            }
            public void UpdateModifyStatus()
            {
                IsModify = Months.Any(month => month.IsModified);
            }
        }
        public class MonthWedAlshViewModel : BindableObjectBase
        {
            private readonly WedAlshViewModel _wedAlshViewModel;

            public MonthWedAlshViewModel(WedAlshViewModel wedAlshViewModel)
            {
                _wedAlshViewModel = wedAlshViewModel;
            }

            public string Name { get; set; }
            public int Month { get; set; }
            public List<DayWedAlshViewModel> Days { get; set; }
            public bool IsModified { get; set; }
            internal void UpdateModifyStatus()
            {
                IsModified = Days.Any(ca => ca.IsModified);
                _wedAlshViewModel.RegistrationSelected.UpdateModifyStatus();
            }
        }
        public class DayWedAlshViewModel : BindableObjectBase
        {
            public string Name { get; set; }
            public int DayOfYear { get; set; }
            public bool IsModified
            {
                get
                {
                    VerifyOpenCondition();
                    return NoonSchedule.IsModified || AfternoonSchedule.IsModified;
                }
            }
            public WedAlshScheduleViewModel NoonSchedule { get; set; }
            public WedAlshScheduleViewModel AfternoonSchedule { get; set; }
            public void VerifyOpenCondition()
            {
                if (AfternoonSchedule.IsOpen || NoonSchedule.IsOpen)
                {
                    if (!AfternoonSchedule.IsCheck && AfternoonSchedule.IsOpen)
                    {
                        NoonSchedule.IsOpen = false;
                        NoonSchedule.ClosingReason = "Il faut être inscrit l'après-midi pour s'incrire le midi.";
                    }
                    else if (AfternoonSchedule.IsCheck && AfternoonSchedule.IsOpen)
                    {
                        NoonSchedule.IsOpen = true;
                        NoonSchedule.ClosingReason = "";
                    }
                    if (NoonSchedule.IsCheck && AfternoonSchedule.IsOpen)
                    {
                        AfternoonSchedule.IsOpen = false;
                        AfternoonSchedule.ClosingReason = "Il faut être inscrit l'après-midi pour s'incrire le midi.";
                    }
                    else if (!NoonSchedule.IsCheck && !AfternoonSchedule.IsOpen)
                    {
                        AfternoonSchedule.IsOpen = true;
                        AfternoonSchedule.ClosingReason = "";
                    }
                }
            }
        }
        public class WedAlshScheduleViewModel : BindableObjectBase
        {
            public MonthWedAlshViewModel Month { get; set; }
            public WedAlshScheduleModel Source { get; set; }

            private bool CurrentCheckValue;
            public bool? LastCheckValue = null;

            public bool IsCheck
            {
                get
                {
                    return CurrentCheckValue;
                }
                set
                {
                    CurrentCheckValue = value;

                    if (LastCheckValue == null)
                        LastCheckValue = value;
                    else
                    {
                        Source.IsBooked = CurrentCheckValue;
                        Month.UpdateModifyStatus();
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

            public bool IsModified { get { return LastCheckValue.HasValue && (LastCheckValue != CurrentCheckValue); } }
        }

    }
    #endregion
}



