using System.Collections.Generic;

namespace OnDijon.Common.Utils
{
    public static class Constants
    {
        #region API Keys
#if DEBUG
        public const string API_URL = @"https://ws-test.metropole-dijon.fr/";

        public const string API_DIIAGE = @"https://apiprodg2.azurewebsites.net/api/"; 
#endif
        #endregion

        #region AppCenter Secret Keys
#if DEBUG
        //public const string APP_CENTER_SECRET_ANDROID = "5c4670e8-03f0-4282-b3c4-660b690c565e";
        //public const string APP_CENTER_SECRET_IOS = "6ce5e54c-f2b6-4002-81e8-4b8235afc663";

        public const string APP_CENTER_SECRET_ANDROID = "2c6ce51a-2c07-4592-99dc-8804f0c19e46";
        public const string APP_CENTER_SECRET_IOS = "2c6ce51a-2c07-4592-99dc-8804f0c19e46";
        
        public const string BUILD_CONFIGURATION = "staging";
        public const string ONDIJON_KEY = "EB3B7DAB-0D5E-4A74-A4AE-E7EDFDE474F9";
#endif
        #endregion

        #region Firebase Keys
        //Topic Firebase pour les notifications générales
#if DEBUG
        public const string FIREBASE_TOPIC = "OnDijonGeneralDev";
#elif STAGING
        public const string FIREBASE_TOPIC = "OnDijonGeneralStaging";
#else
        public const string FIREBASE_TOPIC = "OnDijonGeneral";
#endif
        #endregion

        #region Cache constants
        //Default timeout (s)
        public const int TIMEOUT = 30000;

        //Url flux géocodage
        public const string GEOCODAGE_URL = "https://api-carto.dijon.fr/arcgis/rest/services/Service_Geocodage/Geocodeur_OnDijon/GeocodeServer/";

        //Url BM
        public const string INTERNAL_BM_URL = @"https://internal-bm.dijon.fr/";
        public const string INTERNAL_BM_LOGOFF_URL = @"https://internal-bm.dijon.fr/logoff.aspx";
        public const string ICON_BM_URL = "https://eservices.dijon.fr/_layouts/StylesAndScripts/Dijon/BM/images/";

        //Support contact
        public const string CONTACT_EMAIL = "contact@metropole-dijon.fr";

        // service list cache validity duration (in minutes)
        public const int CacheServiceDuration = 10;

        public const string CguAccepted = "CguAccepted";
        public const string ServicesListKey = "services_list";
        public const string FavoriteServicesListKey = "favorite_services_list";
        public const string FavoriteScopesListKey = "favorite_scopes_list";
        public const string FavoriteScopesAlertIdentityKey = "favorite_alert_identity";

        public const string LastVersionLaunched = "last_version_Launched";


        #endregion


        #region Cities/CityContext
        public const string DIJON_CITYCONTEXT = "Dijon";
        public const string QUETIGNY_CITYCONTEXT = "Quetigny";
        #endregion

        #region SVC EndPoint

        //Common
        public const string COMMON_DIRECTORY = "Common/";
        public const string ACCOUNT_SERVICES = COMMON_DIRECTORY + "ProfileService.svc/";
        public const string DE_SERVICES = COMMON_DIRECTORY + "DemandService.svc/";
        public const string BM_SERVICES = COMMON_DIRECTORY + DIJON_CITYCONTEXT + "/BMService.svc/";
        public const string RS_SERVICES = COMMON_DIRECTORY + "RSService.svc/";
        public const string SR_SERVICES = COMMON_DIRECTORY + "RateSimulatorService.svc/";
        public const string POP_SERVICES = COMMON_DIRECTORY + "PopService.svc/";
        public const string GRE_SERVICES = COMMON_DIRECTORY + DIJON_CITYCONTEXT + "/StrikesService.svc/";
        public const string JO_SERVICES = COMMON_DIRECTORY + "JobOfferService.svc/";
        public const string BO_SERVICES = "/BookingService.svc/"; //Pas de common_directory car passage de la ville dans l'url directement dans le Service
        public const string BILL_SERVICES = COMMON_DIRECTORY + DIJON_CITYCONTEXT + "/BillService.svc/";
        public const string ALSH_SERVICES = COMMON_DIRECTORY + DIJON_CITYCONTEXT + "/WedAlshService.svc/";

        //OnDijon
        public const string ONDIJON_DIRECTORY = "OnDijon/";
        public const string UC_SERVICES = ONDIJON_DIRECTORY + "UsefulContactService.svc/";
        public const string REPORT_SERVICES = ONDIJON_DIRECTORY + "ReportService.svc/";
        public const string ONDIJON_SERVICES = ONDIJON_DIRECTORY + "DashboardService.svc/";
        public const string DIARY_SERVICES = ONDIJON_DIRECTORY + "DiaryService.svc/";
        public const string ALERT_SERVICES = ONDIJON_DIRECTORY + "AlertService.svc/";
        public const string CGU_SERVICES = ONDIJON_DIRECTORY + "CguService.svc/";
        public const string NOTIF_SERVICES = ONDIJON_DIRECTORY + "NotificationService.svc/";
        public const string WORK_SERVICES = ONDIJON_DIRECTORY + "WorkInfoService.svc/";
        public const string RATING_SERVICES = ONDIJON_DIRECTORY + "RatingService.svc/";

        #endregion
        
        #region End Point
        /*----------------------------------\
        |                                   |
        |            End Point              |
        |                                   |
        \----------------------------------*/

        public const string ACCOUNT_AUTHENTICATION = "CheckAuthentication";
        public const string ACCOUNT_CREATE = "CreateProfile";
        public const string ACCOUNT_UPDATE = "UpdateProfile";
        public const string ACCOUNT_UPDATE_MOBILE = "UpdateMobileProfile";
        public const string ACCOUNT_GET = "GetProfile";
        public const string ACCOUNT_RESET_PASSWORD = "ResetPassword";
        public const string ACCOUNT_CHANGE_PASSWORD = "ChangePassword";
        public const string ACCOUNT_DELETE = "DeleteProfile";
        public const string ACCOUNT_DISCONNECT = "LogOut";

        //Report
        public const string API_REPORT_TYPES = "GetReportTypes";
        public const string API_REPORT_CREATE = "SendReport";
        public const string API_REPORT_GET = "GetReport";
        public const string API_REPORT_ICONS = "GetIconByKey";
        public const string API_REPORTS_USER = "GetReports";
        public const string API_REPORTS_AREA = "GetReportsByCoord";
        public const string API_REPORTS_SUBSCRIBE = "SubscribeToReport";

        //BM
        public const string BM_GET_LOANS = "GetLoans";
        public const string BM_GET_RESERVATIONS = "GetReservations";
        public const string BM_GET_CANCELRESERVATION = "CancelReservation";
        public const string BM_GET_ASSOCIATEREADERACCOUNT = "AssociateReaderAccount";
        public const string BM_GET_DISSOCIATEREADERACCOUNT = "DissociateCompteLecteur";
        public const string BM_GET_ACCOUNTBYPROFIL = "GetAccountByProfil";
        public const string BM_GET_AUTOCONNECTURL = "AutoConnectUrl";
        public const string BM_GET_IDENTITYACCOUNTBYPROFIL = "GetIdentityAccountByProfil";
        public const string BM_GET_BorrowerInformation = "GetUserAccount";
        public const string BM_UPDATE_BorrowerPassword = "UpdateUserAccountPassword";
        public const string BM_GET_RENEWLOAN = "RenewLoan";
        public const string BM_GET_Image = "GetImage";
        public const string BM_GET_SUGGESTIONS = "GetSuggestions";
        public const string BM_GET_SEARCH = "Search";
        public const string BM_GET_HOLDINGS = "GetHoldings";
        public const string BM_PLACE_RESERVATION = "PlaceReservation";

        //DSD
        public const string DSD_GET_ACTIVITIES = "GetActivitiesByCatalogue";
        public const string DSD_GET_SESSIONS = "GetSessionsByActivity";
        public const string DSD_GET_CANDIDATES = "GetCandidatesBySession";
        public const string DSD_ADD_REGISTRATION = "AddRegistrationToCart";
        public const string DSD_GET_CART = "GetCart";
        public const string DSD_GET_HISTORY_CART = "GetReservationHistory";
        public const string DSD_VALIDATE_CART = "ValidateCart";
        public const string DSD_DELETE_REGISTRATION = "RemoveRegistrationFromCart";

        public const string IG_GETDATA_CREC = "GetStrikesCreche";
        public const string IG_GETDATA_ELEM = "GetStrikesSchool";

        public const string OE_GETDATA = "GetExternalJobOffer";

        public const string RS_GETDATA = "GetDataRestaurationScolaire";

        //usefulContact
        public const string UC_GET_SERVICESLIST = "GetUsefulServicesList";
        public const string UC_GET_DOMAINSLIST = "GetDomainsList";
        public const string UC_GET_SEARCHCONTACTSLIST = "SearchContactsListWithLocalisation";
        public const string UC_GET_OPENINGTIME = "GetOpeningTimeContact";

        //demands
        public const string DE_GET_DEMANDSLIST = "GetDemandsList";

        //booking
        public const string BO_GET_BOOKINGINSTITUTIONS = "GetBookingInstitutions";
        public const string BO_GET_SCHEDULES = "GetSchedules";
        public const string BO_SEND_BOOK = "BookIdentityDocument";
        public const string BO_GET_BOOKING_INFORMATIONS = "GetBookingInformations";
        public const string BO_CANCEL_BOOKING = "CancelBooking";

        //SimulatorRate
        public const string SR_GET_SIMULATORRATE = "GetSimulatorRate";
        public const string SR_GET_ALLCITYCONTEXT = "GetAllCityContext";

        //Pop
        public const string POP_GET_CHILDSBYCITYCONTEXT = "GetChildsByCityContext";
        public const string POP_GET_CHILDAPPOINTMENTSCHEDULESBYCITYCONTEXT = "GetChildAppointmentSchedulesByCityContext";
        public const string POP_GET_ACTIVESESSIONBYCITYCONTEXT = "GetActiveSessionByCityContext";
        public const string POP_UPDATE_SCHEDULEDAPPOINTMENTBYCITYCONTEXT = "SendBookingByCityContext";
        public const string POP_GET_PARENTSCHEDULEBYCITYCONTEXT = "GetParentScheduleByCityContext";
        public const string POP_UPDATE_PARENTSCHEDULEBYCITYCONTEXT = "UpdateParentScheduleByCityContext";
        public const string POP_GET_CHILD_DIET_BYCITYCONTEXT = "GetChildDietByCityContext";
        public const string POP_UPDATE_CHILD_DIET_BYCITYCONTEXT = "UpdateChildDietByCityContext";


        //Bill
        public const string BILL_GET_BILLS = "GetBills";

        //OnDijon
        public const string ONDIJON_GET_ALL_CARDS = "GetAllCards";
        public const string ONDIJON_GET_APP_VERSION_STATE = "GetAppVersionState";
        public const string ONDIJON_GET_APP_VERSION_STATE_WITH_LAST = "GetAppVersionStateWithLastVersionDescription";
        public const string ONDIJON_GET_CUSTOM_CONTENT = "GetCustomContent";

        //Diary
        public const string DIARY_GET_EVENTS_BY_DATE = "GetEventsByDate";
        public const string DIARY_GET_EVENTS_BY_DIARY = "GetEventsByDiary";
        public const string DIARY_GET_EVENTS_BY_REQUEST = "GetEventsByRequest";
        public const string DIARY_GET_EVENT_DETAILS = "GetEventDetails";
        public const string DIARY_GET_SUGGESTIONS = "GetSuggestions";

        //Services
        public const string SERVICE_GET_ALL = "GetAllMenu";
        public const string SERVICE_GET_FAVOURITE = "GetFavoritesByUser";
        public const string SERVICE_UPDATE_USER_FAVORITE = "UpdateUserFavorites";

        //Alert
        public const string ALERT_GET_ALERTS = "GetAlertsByProfil";
        public const string ALERT_UPDATE_READ_STATUS = "UpdateAlertReadStatusByProfil";
        public const string ALERT_GET_ALERT_BY = "GetAlertSenderByNotification";

        //Work
        public const string WORK_GET_ALL = "GetAllWorkInfos";
        public const string WORK_GET_DATA = "GetWorkData";

        //CGU
        public const string CGU_GET = "GetCurrentCGU";

        //Strike      
        public const string GRE_GET_SCHOOL_STRIKESLIST = "GetStrikesSchool";
        public const string GRE_GET_NURSE_STRIKESLIST = "GetStrikesCreche";

        //Notifications
        public const string NOTIF_GET_ALL = "GetNotifications";
        public const string NOTIF_MARK_AS_READ = "MarkAsRead";
        public const string NOTIF_GET_COUNT = "GetNotificationCount";

        //JobOffer
        public const string JOB_OFFER_GET_LIST = "GetExternalJobOffer";
        public const string SEND_JOB_OFFER_APPLICATION = "SendApplicationToJobOffer";
        public const string JOB_OFFER_GET_TYPE = "GetOfferTypes";

        //Address
        public const string ADDRESS_GET_CITIES = "GetCitiesFromQuery";

        //Alsh
        public const string ALSH_GET_REGISTRATIONS = "GetRegistrations";
        public const string ALSH_UPDATE_REGISTRATION = "UpdateRegistration";

        //Rating
        public const string RATING_GET_ACTUAL_SESSION = "GetActualRatingSession";
        public const string RATING_SEND_RATING = "SendRating";

        #endregion

        #region Services Codes
        public const string SERVICE_VISIBLITY_MAINTENANCE = "maintenance";
        public const string SERVICE_VISIBLITY_VISIBILE = "visible";
        public const string SERVICE_VISIBLITY_HIDDEN = "hidden";

        public const string LibraryCode = "BMD";
        public const string SchoolCode = "PER";
        public const string ContactMapCode = "POI";
        public const string ReportCode = "SIGNALEMENT";
        public const string ServiceListCode = "SRV";
        public const string SimulatorRateCode = "SIM";
        public const string BookingCode = "TID";
        public const string WebCode = "WEB";
        public const string CustomContentCode = "CPERSO";
        public const string AlertCode = "ALERT";
        public const string AlertListCode = "ALERTLIST";
        public const string DiaryCode = "DIARY";
        public const string SchoolStrikeCode = "GRE";
        public const string NurseryStrikeCode = "GRENURS";
        public const string JobOfferCode = "JOBOFFER";
        public const string BillCode = "FACT";
        public const string WedAlshCode = "WEDALSH";
        public const string RoadworkInformationCode = "RWI";
        public const string FavorisCode = "FAV";
        public const string ItineraireCode = "ITINERAIRE";
        public const string AbrisCode = "ABRIS";

        //Requests
        public const string byGuid = "byGuid/";
        public const string Favoris = "Favoris/";
        public const string Abri = "Abri/";
        public const string ShelterState = "ShelterState/All";
        public const string Profil = "Profil/";



        public static readonly IDictionary<string, string> PageKeyByCodeService = new Dictionary<string, string>
        {   
            { LibraryCode, Locator.LibraryMainPage },
            { SchoolCode, Locator.SchoolHomePage },
            { ContactMapCode, Locator.ContactMapPage },
            { ReportCode, Locator.ReportsUserView },
            { ServiceListCode, Locator.ServiceListPage },
            { SimulatorRateCode, Locator.SimulatorRateFormPage },
            { BookingCode, Locator.BookingPage },
            { WebCode, Locator.WebPage },
            { CustomContentCode, Locator.CustomContentView },
            { AlertCode, Locator.AlertDetailPage},
            { AlertListCode, Locator.AlertPage},
            { DiaryCode, Locator.EventListPage},
            { SchoolStrikeCode, Locator.StrikeDetailPage},
            { NurseryStrikeCode, Locator.NurseryStrikeDetailPage},
            { JobOfferCode, Locator.ListJobOfferPage },
            { BillCode, Locator.BillListPage },
            { WedAlshCode, Locator.WedAlshMainPage },
            { RoadworkInformationCode, Locator.RoadworkInformationPage},
            { FavorisCode, Locator.FavoritesPage },
            { ItineraireCode, Locator.ItinerairePage },
            { AbrisCode, Locator.AbrisPage }
        };
        #endregion

        #region AppCenter Analytics TrackEvent
        /*----------------------------------\
        |                                   |
        |   AppCenter Analytics TrackEvent  |
        |                                   |
        \----------------------------------*/

        public const string Navigation = "Nav - Menu : ";
        public const string CardNavigation = "Nav - Card";
        public const string DashboardNavigation = "Nav - Dashboard";

        public static readonly IDictionary<string, string> NavigationMenuEvents = new Dictionary<string, string>
        {
            { LibraryCode, Navigation + LibraryCode},
            { SchoolCode, Navigation + SchoolCode},
            { ContactMapCode, Navigation + ContactMapCode},
            { ReportCode, Navigation + ReportCode},
            { ServiceListCode, Navigation + ServiceListCode},
            { SimulatorRateCode, Navigation + SimulatorRateCode},
            { BookingCode, Navigation + BookingCode},
            { WebCode, Navigation + WebCode},
            { CustomContentCode, Navigation + CustomContentCode},
            { AlertCode, Navigation + AlertCode},
            { AlertListCode, DashboardNavigation + AlertListCode},
            { DiaryCode, DashboardNavigation + DiaryCode},
            { SchoolStrikeCode, DashboardNavigation + SchoolStrikeCode},
            { NurseryStrikeCode, DashboardNavigation + NurseryStrikeCode },
            { JobOfferCode, Navigation + JobOfferCode},
            { BillCode, Navigation + BillCode},
            { WedAlshCode, Navigation + WedAlshCode },
            { RoadworkInformationCode, DashboardNavigation + RoadworkInformationCode},
            { FavorisCode, DashboardNavigation + FavorisCode},
            { ItineraireCode, Navigation + ItineraireCode},
            { AbrisCode, Navigation + AbrisCode},
        };
        #endregion

        #region Navigation Parameters Keys
        public const string NotificationItemNavigationKey = "notificationItemId";
        public const string ServiceNavigationKey = "serviceNavigationId";
        public const string ServiceModelNavigationKey = "serviceNavigationModel";
        public const string ReportDetailNavigationKey = "serviceNavigationId";
        public const string AlertNavigationParameterKey = "alertNavigationModel";
        public const string EventNavigationParameterKey = "eventNavigationModel";
        public const string JobOfferNavigationParameterKey = "jobOfferNavigationModel";
        public const string IsFormNavigationParameterKey = "isFormNavigationBool";
        public const string IsBreadcrumbVisibleNavigationParameterKey = "isBreadcrumbVisibleNavigationBool";
        public const string QueryNavigationParameterKey = "QueryNavigationParam";
        public const string SimulatorRateNavigationParameterKey = "SimulatorRateNavigationParameter";
        public const string SimulatorTitleNavigationParameterKey = "SimulatorTitleNavigationParameter";
        public const string SimulatorWarningNavigationParameterKey = "SimulatorWarningNavigationParameter";
        public const string ReaderListNavigationParameterKey = "ReaderListNavigationParameter";
        public const string CatalogDetailResourceNavigationParameterKey = "CatalogDetailResourceNavigationParameter";
        public const string CatalogDetailAccountReaderListNavigationParameterKey = "CatalogDetailAccountReaderListNavigationParameter";
        public const string StreamingOnlineUrlNavigationParameterKey = "StreamingOnlineUrlNavigationParameter";
        public const string CancelBookingActionNavigationParameterKey = "CancelBookingActionNavigationParameter";
        public const string ApplicationFormFirstPartNavigationParameterKey = "ApplicationFormFirstPartNavigationParameter";
        public const string ReportDetailNotificationItemIdKey = "notificationItemId";

        #endregion
    }
}
