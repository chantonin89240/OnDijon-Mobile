using Newtonsoft.Json;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.UsefulContact.Entities.Models;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OnDijon.Modules.UsefulContact.ViewsModels
{
    public class ServiceDetailViewModel : BaseViewModel
    {

        private ServiceModel _Service { get; set; }
        public ServiceModel Service
        {
            get
            {
                return _Service;
            }
            set
            {
                _Service = value;
                HasGooglePlay = !string.IsNullOrEmpty(_Service.UrlGoogle);
                HasAppStore = !string.IsNullOrEmpty(_Service.UrlApple);
                RaisePropertyChanged(nameof(Service));
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { Set(ref _isRefreshing, value); }
        }


        private bool _hasGooglePlay = false;
        public bool HasGooglePlay
        {
            get { return _hasGooglePlay && Device.RuntimePlatform == Device.Android; }
            set
            {
                Set(ref _hasGooglePlay, value);
                RaisePropertyChanged(nameof(HasGooglePlay));
            }
        }

        private bool _hasAppStore = false;
        public bool HasAppStore
        {
            get { return _hasAppStore && Device.RuntimePlatform == Device.iOS; }
            set
            {
                Set(ref _hasAppStore, value);
                RaisePropertyChanged(nameof(HasAppStore));
            }
        }

        public Command LoadItemsCommand { get; set; }

        public ICommand CloseCommand { get; }
        public ICommand GooglePlayCommand { get; }
        public ICommand AppStoreCommand { get; }
        public ICommand UrlSiteCommand { get; }
        public ICommand PhoneCommand { get; }

        public ServiceDetailViewModel(INavigationService navigationService,
                                      ITranslationService translationService,
                                      IPopupService popupService,
                                      ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            GooglePlayCommand = new Command(() => Launcher.OpenAsync(new Uri(Service.UrlGoogle)));
            AppStoreCommand = new Command(() => Launcher.OpenAsync(new Uri(Service.UrlApple)));
            UrlSiteCommand = new Command(() => Launcher.OpenAsync(new Uri(Service.UrlSite)));
            PhoneCommand = new Command(() => Launcher.OpenAsync(new Uri("tel:" + Service.PhoneNumber)));
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            if (parameters.TryGetValue(Constants.ServiceModelNavigationKey, out string service))
            {
                var serviceModel = JsonConvert.DeserializeObject<ServiceModel>(service);
                Service = serviceModel;
            }
        }
    }
}
