using OnDijon.Common.Services;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Report.ViewModels;
using OnDijon.Modules.Services.ViewModels;
using OnDijon.Modules.Account.Services;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Account.ViewModels;
using OnDijon.Modules.Notifications.Services;
using OnDijon.Modules.Notifications.Services.Interfaces;
using OnDijon.Modules.Notifications.ViewModels;
using OnDijon.Common.Utils.Http;
using OnDijon.Common.Utils.Services;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Alert.Services;
using OnDijon.Modules.Alert.Services.Interfaces;
using OnDijon.Modules.Alert.ViewModels;
using OnDijon.Modules.Booking.Services;
using OnDijon.Modules.Booking.Services.Interfaces;
using OnDijon.Modules.Booking.ViewModels;
using OnDijon.Modules.CustomContent.Services;
using OnDijon.Modules.CustomContent.Services.Interfaces;
using OnDijon.Modules.CustomContent.ViewModel;
using OnDijon.Modules.Demands.Services;
using OnDijon.Modules.Demands.Services.Interfaces;
using OnDijon.Modules.Demands.ViewsModels;
using OnDijon.Modules.Diary.Services;
using OnDijon.Modules.Diary.Services.Interfaces;
using OnDijon.Modules.Diary.ViewModels;
using OnDijon.Modules.JobOffer.Services;
using OnDijon.Modules.JobOffer.Services.Interfaces;
using OnDijon.Modules.JobOffer.ViewModels;
using OnDijon.Modules.Library.Services;
using OnDijon.Modules.Library.Services.Impl;
using OnDijon.Modules.Library.ViewModels;
using OnDijon.Modules.RoadworkInformation.Services;
using OnDijon.Modules.RoadworkInformation.Services.Interfaces;
using OnDijon.Modules.RoadworkInformation.ViewModels;
using OnDijon.Modules.OnBoarding.ViewModels;
using OnDijon.Modules.School.Services;
using OnDijon.Modules.School.Services.Interfaces;
using OnDijon.Modules.School.ViewModel;
using OnDijon.Modules.School.ViewModels;
using OnDijon.Modules.SchoolServices.Interfaces;
using OnDijon.Modules.Simulator.Services;
using OnDijon.Modules.Simulator.Services.Interfaces;
using OnDijon.Modules.Simulator.ViewsModels;
using OnDijon.Modules.Strike.Services;
using OnDijon.Modules.Strike.Services.Interfaces;
using OnDijon.Modules.Strike.ViewModels;
using OnDijon.Modules.UsefulContact.Services;
using OnDijon.Modules.UsefulContact.Services.Interfaces;
using OnDijon.Modules.UsefulContact.ViewsModels;
using OnDijon.Modules.Web.ViewModels;
using System;
using System.Net.Http;
using OnDijon.Modules.Bill.ViewModels;
using OnDijon.Modules.Bill.Services.Interfaces;
using OnDijon.Modules.Bill.Services;
using FFImageLoading;
using FFImageLoading.Config;
using OnDijon.Common.Views;
using OnDijon.Modules.Account.Pages;
using OnDijon.Modules.Alert.Pages;
using OnDijon.Modules.Bill.Pages;
using OnDijon.Modules.Booking.Pages;
using OnDijon.Modules.CustomContent.Views;
using OnDijon.Modules.Dashboard.Pages;
using OnDijon.Modules.WedAlsh.ViewModels;
using OnDijon.Modules.WedAlsh.Services;
using OnDijon.Modules.WedAlsh.Services.Interfaces;
using OnDijon.Modules.Rating.ViewModels;
using OnDijon.Modules.Rating.Services;
using OnDijon.Modules.Rating.Services.Interfaces;
using OnDijon.Modules.Report.Services;
using OnDijon.Modules.Report.Services.Interfaces;
using OnDijon.Modules.Dashboard.ViewModels;
using OnDijon.Modules.Dashboard.Services.Interfaces;
using OnDijon.Modules.Services.Services.Interfaces;
using OnDijon.Modules.Services.Services;
using OnDijon.Modules.Dashboard.Services;
using OnDijon.Modules.Demands.Pages;
using OnDijon.Modules.Diary.Pages;
using OnDijon.Modules.JobOffer.Pages;
using OnDijon.Modules.Library.Pages;
using OnDijon.Modules.Library.Views;
using OnDijon.Modules.Notifications.Views;
using OnDijon.Modules.OnBoarding.Pages;
using OnDijon.Modules.Report.Pages;
using OnDijon.Modules.Report.Pages.Detail;
using OnDijon.Modules.RoadworkInformation.Pages;
using OnDijon.Modules.School.Pages;
using OnDijon.Modules.School.Views;
using OnDijon.Modules.Service.Pages;
using OnDijon.Modules.Simulator.Pages;
using OnDijon.Modules.Strike.Pages;
using OnDijon.Modules.UsefulContact.Pages;
using OnDijon.Modules.Web.Views;
using OnDijon.Modules.WedAlsh.Pages;
using Prism.Ioc;
using Xamarin.Forms;
using OnDijon.Modules.Favorites.ViewModels;
using OnDijon.Modules.Favorites.Services;
using OnDijon.Modules.Favorites.Services.Interfaces;
using OnDijon.Modules.Favorites.Pages;

using OnDijon.Modules.Abris.ViewModels;
using OnDijon.Modules.Abris.Pages;
using OnDijon.Modules.Itineraire.ViewModels;
using OnDijon.Modules.Itineraire.Services;
using OnDijon.Modules.Itineraire.Services.Interfaces;
using OnDijon.Modules.Itineraire.Pages;
using OnDijon.Modules.Abris.Serv.Interfaces;
using OnDijon.Modules.Abris.Serv;

namespace OnDijon
{
    public class  Locator
    {
	    private readonly IContainerRegistry _containerRegistry;

	    public Locator(IContainerRegistry containerRegistry)
        {
	        _containerRegistry = containerRegistry;

	        //register http client
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = AuthenticatedHttpClientHandler.ValidateServerCertificate;
            HttpClient httpClient = new HttpClient(handler);

            RegisterFactory(() => httpClient);

            RegisterAllViewModels();
            RegisterAllServices();

            // Cette partie est à retirer quand on aura résolue les problèmes de certificat d'image (venant de Chenove au moment de l'écriture)
            var handlerImage = new HttpClientHandler();
            handlerImage.ClientCertificateOptions = ClientCertificateOption.Manual;
            handlerImage.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) => true;
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Dijon-Metropole");
            ImageService.Instance.Initialize(new Configuration
            {
                HttpClient = client
            });
        }

        private void RegisterAllViewModels()
        {
            RegisterViewModel<LoginViewModel, LoginPage>(LoginPage);
            RegisterViewModel<DashboardViewModel, DashboardView>(DashboardView);
            RegisterViewModel<OnBoardingViewModel, OnBoardingPage>(OnBoardingPage);

            RegisterViewModel<MenuViewModel, MenuView>(MenuView);

            RegisterViewModel<SignUpViewModel, SignUpView>(SignUpView);
            RegisterViewModel<ResetPasswordViewModel, ResetPasswordView>(ResetPasswordView);
            RegisterViewModel<ProfileViewModel, ProfileView>(ProfileView);
            RegisterViewModel<ChangeProfileInfoViewModel, ChangeProfileInfoView>(ChangeProfileInfoView);
            RegisterType<CguPopupViewModel>();
            RegisterViewModel<NotificationsHistoryViewModel, NotificationsHistoryView>(NotificationsHistoryView);
            RegisterViewModel<DeleteAccountViewModel, DeleteAccountPage>(DeleteAccountPage);

            RegisterViewModel<ReportTypeViewModel, ReportTypeView>(ReportTypeView);
            RegisterViewModel<ReportLocalisationViewModel, ReportLocalisationView>(ReportLocalisationView);
            RegisterViewModel<ReportDescriptionViewModel, ReportDescriptionView>(ReportDescriptionView);
            RegisterViewModel<ReportSummaryViewModel, ReportSummaryView>(ReportSummaryView);
            RegisterViewModel<ReportDetailViewModel, ReportDetailView>(ReportDetailView);
            RegisterViewModel<ReportsUserViewModel, ReportsUserView>(ReportsUserView);
            RegisterViewModel<ReportSuccessViewModel, ReportSuccessView>(ReportSuccessView);

            RegisterViewModel<SchoolHomeViewModel, SchoolHomePage>(SchoolHomePage);

            RegisterType<TopBarViewModel>();
            RegisterType<SchoolRestaurantViewModel>();
            RegisterType<SchoolDayViewModel>();
            RegisterType<WeekSchedulingViewModel>();

            RegisterType<AssociateReaderAccountViewModel>();
            RegisterType<LoanListViewModel>();
            RegisterType<ReservationListViewModel>();
            RegisterType<LibraryCardViewModel>();
            RegisterViewModel<LibraryMainViewModel, LibraryMainPage>(LibraryMainPage);
            RegisterViewModel<SearchLibraryViewModel, SearchLibraryPage>(SearchLibraryPage);
            RegisterType<ProfilCardsViewModel>();
            RegisterViewModel<CatalogSearchViewModel, CatalogSearchPage>(CatalogSearchPage);
            RegisterViewModel<StreamingOnlineViewModel, StreamingOnlinePage>(StreamingOnlinePage);
            RegisterViewModel<CatalogDetailViewModel, CatalogDetailPage>(CatalogDetailPage);

            // RegisterType<EventViewModel>();
            // RegisterViewModel<EventListViewModel, NewsPageDetail>(NewsPageDetail);
            // RegisterType<CategoryViewModel>();
            // RegisterViewModel<CategoryListViewModel, NewsPageMaster>(NewsPageMaster);

            RegisterViewModel<ContactMapViewModel, ContactMapPage>(ContactMapPage);
            RegisterViewModel<ContactDetailViewModel, ContactDetailPage>(ContactDetailPage);
            RegisterViewModel<WorkInfosViewModel, WorkInfosPage>(WorkInfosPage);
            RegisterViewModel<ServiceListViewModel, ServiceListPage>(ServiceListPage);
            RegisterViewModel<ServiceDetailViewModel, ServiceDetailPage>(ServiceDetailPage);

            RegisterViewModel<DemandListViewModel, DemandListPage>(DemandListPage);

            RegisterViewModel<BookingViewModel, BookingPage>(BookingPage);
            RegisterViewModel<CancelBookingViewModel, CancelBookingPage>(CancelBookingPage);

            RegisterViewModel<SimulatorRateViewModel, SimulatorRatePage>(SimulatorRatePage);
            RegisterViewModel<SimulatorRateFormViewModel, SimulatorRateFormPage>(SimulatorRateFormPage);

            RegisterViewModel<ServicesViewModel, ServicesView>(ServicesView);
            RegisterViewModel<ServicesViewModel, ScopesView>(ScopesView);

            RegisterViewModel<WebViewModel, WebPage>(WebPage);
            RegisterViewModel<CustomContentViewModel, CustomContentView>(CustomContentView);

            RegisterViewModel<EventDetailDiaryViewModel, EventDetailsPage>(EventDetailsPage);
            RegisterViewModel<EventDiaryListViewModel, EventListPage>(EventListPage);
            RegisterType<EventDiaryListDashboardViewModel>();

            RegisterViewModel<AlertDetailViewModel, AlertDetailPage>(AlertDetailPage);
            RegisterViewModel<AlertRepositoryViewModel, AlertRepositoryPage>(AlertPage);
            RegisterType<AlertListViewModel>();

            RegisterViewModel<StrikeDetailViewModel, StrikeDetailPage>(StrikeDetailPage);
            RegisterViewModel<NurseryStrikeDetailViewModel, NurseryStrikeDetailPage>(NurseryStrikeDetailPage);
        
            RegisterViewModel<DetailJobOfferViewModel, DetailJobOfferPage>(DetailJobOfferPage);
            RegisterViewModel<ListJobOfferViewModel, ListJobOfferPage>(ListJobOfferPage);
            RegisterViewModel<ApplicationFormViewModel, ApplicationFormPage>(ApplicationFormPage);
            RegisterViewModel<SelectCityViewModel, SelectCityPage>(SelectCityPage);

            RegisterViewModel<BillListViewModel, BillListPage>(BillListPage);
            
            RegisterViewModel<WedAlshViewModel, WedAlshMainPage>(WedAlshMainPage);

            RegisterViewModel<RoadworkInformationViewModel, RoadworkInformationPage>(RoadworkInformationPage);
            RegisterType<RoadworkDetailViewModel>();

            RegisterType<RatingViewModel>();

            RegisterViewModel<FavoritesViewModel, FavoritesPage>(FavoritesPage);
            RegisterViewModel<ModifyFavoritesViewModel, ModifyFavoritesPage>(ModifyFavoritesPage);

            RegisterViewModel<ItineraireViewModel, ItinerairePage>(ItinerairePage);
            RegisterViewModel<AbrisViewModel, AbrisPage>(AbrisPage);
        }
        
        private void RegisterType<TType>()
        {
	        _containerRegistry.Register<TType>();
        }
        
        private void RegisterType<TType, TImplementation>()
        where TImplementation : TType
		{
	        _containerRegistry.Register<TType, TImplementation>();
		}

        private void RegisterAllServices()
        {
	        RegisterService<ILoggerService, DebuggerConsoleLoggerService>();
            RegisterService<IHttpService, HttpService>();
            RegisterService<ISession, Session>();
            RegisterService<ITranslationService, TranslationService>();
            RegisterService<IAccountService, AccountService>();
            RegisterService<IReportService, ReportService>();
            RegisterService<IPopupService, PopupService>();
            RegisterService<IPhotoService, PhotoService>();
            RegisterService<ICguService, CguService>();
            RegisterService<INotificationService, NotificationService>();
            RegisterService<ICacheService, CacheService>();
            RegisterService<IGeolocationService, GeolocationService>();
            RegisterService<IUserIdService, UserIdService>();
            RegisterService<IVersionService, VersionService>();

            RegisterService<ISchoolRestaurantBookingConfigurationService, SchoolRestaurantBookingConfigurationService>();
            RegisterService<IRSCalendar, RSCalendarService>();

            RegisterService<ILoanService, LoanService>();
            RegisterService<IReservationService, ReservationService>();
            RegisterService<IAccountReaderService, AccountReaderService>();
            RegisterService<IDocumentService, DocumentService>();

            // RegisterService<IEventConfigurationService, EventConfigurationService>();
            // RegisterService<ICategoryConfigurationService, CategoryConfigurationService>();
            // RegisterService<IEventService, EventService>();
            // RegisterService<ICategoryService, CategoryService>();

            RegisterService<IContactDomainService, ContactDomainService>();
            RegisterService<IContactService, ContactService>();
            RegisterService<IServiceService, ServiceService>();

            RegisterService<IDemandService, DemandService>();

            RegisterService<IBookingService, BookingService>();

            RegisterService<ISimulatorRateService, SimulatorRateService>();
            RegisterService<IDashboardService, DashboardService>();
            RegisterService<IServicesService, ServicesService>();
            RegisterService<ISchoolCardConfigurationService, SchoolCardConfigurationService>();
            RegisterService<ICustomContentService, CustomContentService>();

            RegisterService<IDiaryService, DiaryService>();

            RegisterService<IAlertService, AlertService>();

            RegisterService<IListStrikeService, ListStrikeService>();

            RegisterService<IJobOfferService, JobOfferService>();

            RegisterService<IAddressServices, AddressServices>();
            
            RegisterService<IBillService, BillService>();

            RegisterService<IWedAlshService, WedAlshService>();

            RegisterService<IRoadworkInfoService, RoadworkInfoService>();

            RegisterService<IRatingService, RatingService>();
            
            RegisterService<IOAuthService, OAuthService>();

            RegisterService<IFavoriteService, FavoriteService>();

            RegisterService<IAbrisService, AbrisService>();
            //RegisterService<IItineraireService, ItineraireService>();
        }

        /// <summary>
        /// Register a ViewModel type in the IOC container.
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel type</typeparam>
        public void RegisterViewModel<TViewModel, TPage>(string pageKey = null) 
	        where TViewModel : BaseViewModel
	        where TPage : ContentPage
        {
	        if (pageKey is  null)
				_containerRegistry.RegisterForNavigation<TPage, TViewModel>();
	        else
		        _containerRegistry.RegisterForNavigation<TPage, TViewModel>(pageKey);
        }

        /// <summary>
        /// Register a service interface and implementation types in the IOC container.
        /// </summary>
        /// <typeparam name="TInterface">interface type</typeparam>
        /// <typeparam name="TService">implementation type</typeparam>
        public void RegisterService<TInterface, TService>()
            where TInterface : class
            where TService : class, TInterface
        {
	        _containerRegistry.RegisterSingleton<TInterface, TService>();
        }

        /// <summary>
        /// Register an instance factory in the IOC container.
        /// </summary>
        /// <typeparam name="T">instance type</typeparam>
        /// <param name="factory">instance factory</param>
        public void RegisterFactory<T>(Func<T> factory) where T : class
        {
	        _containerRegistry.RegisterSingleton<T>(factory);
        }

        /// <summary>
        /// Get the instance of class T previously registered in the IOC container.
        /// </summary>
        /// <typeparam name="T">instance type</typeparam>
        public T GetInstance<T>()
        {
            try
            {
	            return Prism.PrismApplicationBase.Current.Container.Resolve<T>();
            }
            catch (Exception e)
            {
	            Prism.PrismApplicationBase.Current.Container.Resolve<ILoggerService>().Error("Error while resolving " + typeof(T).Name, e);
	            return default(T);
            }
        }

        #region Pages keys

        public const string LoginPage = "LoginPage";
        //public const string LoginView = "LoginView";
        public const string DashboardView = "DashboardView";
        public const string OnBoardingPage = "OnBoardingPage";

        public const string MenuView = "MenuView";

        public const string SignUpView = "SignUpView";
        public const string ResetPasswordView = "ResetPasswordView";
        public const string ProfileView = "ProfileView";
        public const string ChangeProfileInfoView = "ChangeProfileInfoView";
        //public const string CguPopupView = "CguPopupView";
        public const string NotificationsHistoryView = "NotificationsHistoryView";
        public const string DeleteAccountPage = "DeleteAccountPage";

        public const string ReportsHomeView = "ReportsHomeView";
        public const string ReportTypeView = "ReportTypeView";
        public const string ReportLocalisationView = "ReportLocalisationView";
        public const string ReportDescriptionView = "ReportDescriptionView";
        public const string ReportSummaryView = "ReportSummaryView";
        public const string ReportDetailView = "ReportDetailView";
        public const string ReportsUserView = "ReportsUserView";
        public const string ReportSuccessView = "ReportSuccessView";


        public const string SchoolHomePage = "SchoolHomePage";

        public const string SchoolRestaurantMenuPage = "SchoolRestaurantMenuPage";
        public const string SchoolRestaurantDayCalendarPage = "SchoolRestaurantDayCalendarPage";
        public const string SchoolRestaurantCalendarPage = "SchoolRestaurantCalendarPage";
        public const string SchoolRestaurantBookingWeekView = "SchoolRestaurantBookingWeekView";
        public const string SchoolRestaurantBookingPage = "SchoolRestaurantBookingPage";
        public const string SchoolRestaurantChildPage = "SchoolRestaurantChildPage";
        public const string SchoolRestaurantDisplayPage = "SchoolRestaurantDisplayPage";
        public const string StrikesSchoolPage = "StrikesSchoolPage";
        public const string startStrikesPrimarySchoolPage = "startStrikesPrimarySchoolPage";
        public const string MenuCommand = "MenuCommand";
        public const string StrikesCrechePage = "StrikesCrechePage";
        public const string JobOfferPage = "JobOfferPage";
        public const string JobOfferDetailPage = "JobOfferDetailPage";
        public const string JobOfferSpontaneousPage = "JobOfferSpontaneousPage";
        public const string JobOfferDisplayImageUrlPage = "JobOfferDisplayImageUrlPage";

        public const string LibraryMainPage = "LibraryMainPage";
        public const string SearchLibraryPage = "SearchLibraryPage";
        public const string CatalogSearchPage = "CatalogSearchPage";
        public const string StreamingOnlinePage = "StremingOnlinePage";
        public const string CatalogDetailPage = "CatalogDetailPage";

        public const string NewsPage = "NewsPage";
        public const string NewsPageMaster = "NewsPageMaster";
        public const string NewsPageDetail = "NewsPageDetail";
        public const string NewsWebViewPage = "NewsWebViewPage";

        public const string ActivityPage = "ActivityPage";
        public const string ActivitySearchPage = "ActivitySearchPage";
        public const string CartPage = "CartPage";
        public const string SessionPage = "SessionPage";
        public const string SessionDetailPage = "SessionDetailPage";
        public const string RegistrationPage = "RegistrationPage";
        public const string HistoryPage = "HistoryPage";
        public const string HistoryDetailPage = "HistoryDetailPage";

        public const string ContactMapPage = "ContactMapPage";
        public const string ContactDetailPage = "ContactDetailPage";
        public const string WorkInfosPage = "WorkInfosPage";
        public const string ServiceListPage = "ServiceListPage";
        public const string ServiceDetailPage = "ServiceDetailPage";

        public const string DemandListPage = "DemandListPage";

        public const string BookingPage = "BookingPage";
        public const string CancelBookingPage = "CancelBookingPage";

        public const string SimulatorRatePage = "SimulatorRatePage";
        public const string SimulatorRateFormPage = "SimulatorRateFormPage";

        public const string ServicesView = "ServicesView";
        public const string ScopesView = "ScopesView";

        public const string WebPage = "WebPage";

        public const string CustomContentView = "CustomContentView";

        public const string EventDetailsPage = "EventDetailsPage";
        public const string EventListPage = "EventListPage";

        public const string AlertPage = "AlertPage";
        public const string AlertDetailPage = "AlertDetailPage";

        public const string DetailJobOfferPage = "DetailJobOfferPage";
        public const string ListJobOfferPage = "ListJobOfferPage";
        public const string ApplicationFormPage = "ApplicationFormPage";
        public const string SelectCityPage = "SelectCityPage";

        public const string BillListPage = "BillListPage";

        public const string MetropolitanCounsilSubscriptionPage = "MetropolitanCounsilSubscriptionPage";

        public const string RoadworkInformationPage = "RoadworkInformationPage";

        //public const string RatingView = "RatingPopupView";

        public const string StrikeListPage = "StrikeListPage";
        public const string StrikeDetailPage = "StrikeDetailPage";
        public const string NurseryStrikeDetailPage = "NurseryStrikeDetailPage";

        public const string WedAlshMainPage = "WedAlshMainPage";

        public const string FavoritesPage = "FavoritesPage";

        public const string ModifyFavoritesPage = "ModifyFavoritesPage";

        public const string ItinerairePage = "ItinerairePage";
        public const string AbrisPage = "AbrisPage";
        #endregion

        #region ViewModels instances
        public TopBarViewModel TopBarViewModel => GetInstance<TopBarViewModel>();
        #endregion
    }
}
