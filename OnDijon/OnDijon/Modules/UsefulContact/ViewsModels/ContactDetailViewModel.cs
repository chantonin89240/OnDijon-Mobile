using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.UsefulContact.Entities.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OnDijon.Modules.UsefulContact.ViewsModels
{
    public class ContactDetailViewModel : BaseViewModel
    {

        #region variables et commandes

        private ContactModel _Contact { get; set; }
        public ContactModel Contact
        {
            get
            {
                return _Contact;
            }
            set
            {
                _Contact = value;
                RaisePropertyChanged(nameof(Contact));
                LoadData();
            }
        }

        private IList<OpeningTimeDay> _openingTimeDetail;
        public IList<OpeningTimeDay> OpeningTimeDetail
        {
            get
            {
                return _openingTimeDetail;
            }
            set
            {
                Set(ref _openingTimeDetail, value); ;
                //LoadData();
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { Set(ref _isRefreshing, value); }
        }

        private bool _hasOpenInfo = false;
        public bool HasOpenInfo
        {
            get { return _hasOpenInfo; }
            set { Set(ref _hasOpenInfo, value); }
        }

        public Command LoadItemsCommand { get; set; }


        public ICommand CloseCommand { get; }
        public ICommand CloseViewCommand { get; }
        public ICommand PhoneCommand { get; }
        public ICommand AddressCommand { get; }
        public ICommand MailCommand { get; }

        #endregion

        public ContactDetailViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            CloseViewCommand = new Command(() => CloseContactDetailDisplayAction?.Invoke());
            PhoneCommand = new Command(() => Launcher.OpenAsync(new Uri("tel:" + Contact?.ContactInfos?.PhoneNumber)));
            AddressCommand = new Command(() => Clipboard.SetTextAsync(Contact?.Address));
            MailCommand = new Command(() => Clipboard.SetTextAsync(Contact?.ContactInfos?.Mail));
            OpeningTimeDetail = new List<OpeningTimeDay>();
        }
        
        public Action CloseContactDetailDisplayAction { get; set; }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            
        }

        public void LoadData()
        {
            HasOpenInfo = Contact?.HasOpenInfo ?? false;
            OpeningTimeDetail = HasOpenInfo ? GetOpeningTimeDetail() : null;
        }

        public List<OpeningTimeDay> GetOpeningTimeDetail()
        {
            CultureInfo provider = new CultureInfo("fr-FR");
            List<OpeningTimeDay> response = new List<OpeningTimeDay>();
            for (double i = 0; i < 7; i++)
            {
                OpeningTimeDay newDay = new OpeningTimeDay();
                newDay.DayName = String.Format("{0,-10}", provider.DateTimeFormat.GetDayName(DateTime.Today.AddDays(i).DayOfWeek)); 
                if (Contact.ContactInfos.OpeningTime.Where(op => DateTime.Compare(op.Day, DateTime.Today.AddDays(i)) == 0).Any())
                {
                    string timeList = "";
                    foreach (ContactOpeningPeriodModel dayOpeningTime in Contact.ContactInfos.OpeningTime.Where(op => DateTime.Compare(op.Day, DateTime.Today.AddDays(i)) == 0).ToList())
                    {
                        timeList += "  " + (dayOpeningTime.BeginPeriod / 60).ToString("00") + ":" + (dayOpeningTime.BeginPeriod % 60).ToString("00");
                        timeList += "-" + (dayOpeningTime.EndPeriod / 60).ToString("00") + ":" + (dayOpeningTime.EndPeriod % 60).ToString("00");
                    }
                    newDay.DayDetail = timeList;
                }
                else
                {
                    newDay.DayDetail = "Fermé";
                }
                response.Add(newDay);
            }
            return response;
        }

        public class OpeningTimeDay  {
            public string DayName { get; set; }
            public string DayDetail { get; set; }
        }
    }
}
