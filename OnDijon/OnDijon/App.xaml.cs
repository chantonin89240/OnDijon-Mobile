using Esri.ArcGISRuntime;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Extensions;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Report.Pages;
using OnDijon.Modules.Report.Pages.Detail;
using OnDijon.Modules.Account.Pages;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Account.Views;
using OnDijon.Modules.Notifications.Services.Interfaces;
using OnDijon.Modules.Notifications.Views;
using OnDijon.Common.Permissions;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.Views.Effects;
using OnDijon.Modules.Alert.Pages;
using OnDijon.Modules.Booking.Pages;
using OnDijon.Modules.CustomContent.Views;
using OnDijon.Modules.Demands.Pages;
using OnDijon.Modules.Diary.Pages;
using OnDijon.Modules.JobOffer.Pages;
using OnDijon.Modules.Library.Pages;
using OnDijon.Modules.Library.Views;
using OnDijon.Modules.OnBoarding.Pages;
using OnDijon.Modules.RoadworkInformation.Pages;
using OnDijon.Modules.School.Pages;
using OnDijon.Modules.Simulator.Pages;
using OnDijon.Modules.Strike.Pages;
using OnDijon.Modules.UsefulContact.Pages;
using OnDijon.Modules.Web.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using OnDijon.Modules.Bill.Pages;
using OnDijon.Modules.WedAlsh.Pages;
using OnDijon.Modules.Rating.Views;
using OnDijon.Modules.Dashboard.Pages;
using OnDijon.Modules.Service.Pages;
using OnDijon.Common.Views;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace OnDijon
{
    // Point d'entrée de l'app
    public partial class App 
    {
        /// <summary>
        /// Contains all IOC registrations
        /// </summary>
        public static Locator Locator { get; private set; }


        public App(IPlatformInitializer initializer)
	        : base(initializer)
        {
            

         
        }
        
        /// <summary>
        /// Current NavigationService (raffraichi dans BaseViewModel OnNavigatedTo)
        /// L'instance du NavigationService est potentiellement raffraichie à chaque fois qu'on navigue vers une nouvelle page
        /// Pour pouvoir accéder au bon NavigationService dans une classe qui n'est pas un ViewModel, nous avons besoin de garder une référence à l'instance courante
        /// </summary>
        public static INavigationService CurrentNavigationService { get; set; }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            //show warning popup if internet connection is lost on any page except dashboard
            if (e.NetworkAccess != NetworkAccess.Internet && App.Current.MainPage.GetType().Name != Locator.DashboardView)
            {
                var popupService = Locator.GetInstance<IPopupService>();
                popupService.Show(PopupEnum.PopupError,
                    "Votre connexion a été perdue, vous allez être redirigé(e) vers la page d'accueil",
                    "OK",
                    () => { CurrentNavigationService.NavigateTo(Locator.DashboardView); });
            }
        }

      

        protected override async void OnStart()
        {
            //AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start
            (
                $"android={Constants.APP_CENTER_SECRET_ANDROID};" +
                $"ios={Constants.APP_CENTER_SECRET_IOS}",
                typeof(Analytics),
                typeof(Crashes)
            );

            //push notifications
            PermissionStatus notificationPermission = await Permissions.CheckStatusAsync<NotificationPermission>();
            if (notificationPermission == PermissionStatus.Granted)
            {
                INotificationService notificationService = Locator.GetInstance<INotificationService>();
                notificationService.InitFirebase();
            }

            ArcGISRuntimeEnvironment.SetLicense("runtimelite,1000,rud8378955595,none,HC5X0H4AH42DPGXX3098");

            // Init user cache
            Akavache.Registrations.Start("OnDijon");
        }

        protected override void OnSleep()
        {
            Current.PageAppearing -= App_PageAppearing;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
	        Locator = new Locator(containerRegistry);
	        containerRegistry.RegisterForNavigation<NavigationPage>();
        }

        protected override async void OnInitialized()
        {
	        InitializeComponent();
	        VersionTracking.Track();
			//enable network monitoring
	        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

	        var navResult = await NavigationService.NavigateAsync("NavigationPage/OnBoardingPage");
	       
        }

        protected override void OnResume()
        {
            Current.PageAppearing += App_PageAppearing;
        }

        private void App_PageAppearing(object sender, Page e)
        {
            //Remove tooltip from previous page if present
            TooltipEffect.Remove();
        }
    }
}
