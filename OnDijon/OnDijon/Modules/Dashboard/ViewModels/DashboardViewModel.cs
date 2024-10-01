using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.AppCenter.Analytics;
using OnDijon.Common.Entities;
using OnDijon.Modules.Dashboard.Entities.Dto.Card;
using OnDijon.Modules.Dashboard.Entities.Models;
using OnDijon.Modules.Dashboard.Entities.Response;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Services.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Model;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Modules.Alert.Services.Interfaces;
using OnDijon.Modules.Alert.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OnDijon.Common.Extensions;
using OnDijon.Modules.Rating.ViewModels;
using OnDijon.Modules.Services.Entities.Dto;
using OnDijon.Modules.Services.Services.Interfaces;
using OnDijon.Modules.Dashboard.Services.Interfaces;
using OnDijon.Modules.Diary.ViewModels;
using OnDijon.Modules.Services.Entities.Models;
using OnDijon.Modules.Services.Entities.Response;
using OnDijon.Modules.Services.Helpers;
using Prism.Commands;
using Prism.Navigation;
using AsyncAwaitBestPractices.MVVM;

namespace OnDijon.Modules.Dashboard.ViewModels
{
    public class DashboardViewModel : ServiceBaseViewModel
    {
        private readonly ISession _session;
        private readonly IServicesService _servicesService;
        private readonly IDashboardService _dashboardService;
        private readonly IVersionService _versionService;

        public bool IsConnected => _session.IsConnected();

        public AppVersionStateResponse AppVersion;

        public ICommand GoToProfile { get; }

        public ICommand GoToService { get; }

        public ICommand GoToServiceList { get; }

        public ICommand ContactSupport { get; }

        public ICommand CarouselActionCommand { get; }

        private ObservableCollection<CardDto> cards;
        public ICommand GoToRoadworkInfoCommand { get; set; }

        public ObservableCollection<CardDto> Cards
        {
            get { return cards; }
            set { Set(ref cards, value);
                RaisePropertyChanged(nameof(Cards));
            }
        }

        private IList<ServiceDto> _favouriteServices;
        
        public IList<ServiceDto> FavouriteServices
        {
            get { return _favouriteServices; }
            set { Set(ref _favouriteServices, value); }
        }

        private List<WorkDataModel> _WorkDataList;
        public List<WorkDataModel> WorkDataList
        {
            get { return _WorkDataList; }
            set { Set(ref _WorkDataList, value); }
        }

        private AlertListViewModel _alertListViewModel;
        public AlertListViewModel AlertListViewModel { get => _alertListViewModel; set => Set(ref _alertListViewModel, value); }

        private RatingViewModel _ratingViewModel;
        public RatingViewModel RatingViewModel { get => _ratingViewModel; set => Set(ref _ratingViewModel, value); }

        #region EventDiaryListDashboardViewModel => EventDiaryListDashboardViewModel

        private EventDiaryListDashboardViewModel _eventDiaryListDashboardViewModel;

        public EventDiaryListDashboardViewModel EventDiaryListDashboardViewModel { get => _eventDiaryListDashboardViewModel; set => SetProperty(ref _eventDiaryListDashboardViewModel, value); }

        #endregion

        private FavoriteService _favoriteService;

        public FavoriteService FavoriteService
        {
            get { return _favoriteService; }
            set { Set(ref _favoriteService, value); }
        }

        private string _NewWorkDashboardString;
        public string NewWorkDashboardString
        {
            get { return _NewWorkDashboardString; }
            set { Set(ref _NewWorkDashboardString, value); }
        }

        private string _CurrentWorkDashboardString;
        public string CurrentWorkDashboardString
        {
            get { return _CurrentWorkDashboardString; }
            set { Set(ref _CurrentWorkDashboardString, value); }
        }

        private bool _alertVisible;
        public bool AlertVisible { get => _alertVisible; set => Set(ref _alertVisible, value); }

        private bool _diaryVisible = true;
        public bool DiaryVisible { get => _diaryVisible; set => Set(ref _diaryVisible, value); }

        public DashboardViewModel(INavigationService navigationService,
                                  ITranslationService translationService,
                                  ISession session,
                                  IPopupService popupService,
                                  IServicesService servicesService,
                                  IDashboardService dashboardService,
                                  IAlertService alertService,
                                  IVersionService versionService, 
                                  ILoggerService loggerService) : base(navigationService, translationService, popupService, session, loggerService)
        {


            _session = session;
            _servicesService = servicesService;
            _dashboardService = dashboardService;
            _versionService = versionService;
            cards = new ObservableCollection<CardDto>();

            GoToProfile = new DelegateCommand(() =>
            {
                if (_session.IsConnected())
                    NavigateTo(Locator.ProfileView);
                else
                    NavigateTo(Locator.LoginPage);
            });
            AlertListViewModel = App.Locator.GetInstance<AlertListViewModel>();
            RatingViewModel = App.Locator.GetInstance<RatingViewModel>();
            EventDiaryListDashboardViewModel = App.Locator.GetInstance<EventDiaryListDashboardViewModel>();

            CarouselActionCommand = new DelegateCommand<CardActionDto>(action => HandleAction(action));

            GoToService = new AsyncCommand<ServiceLayout>(async service => await NavigateToService(service, ""));

            GoToServiceList = new Command(() => { NavigateTo(Locator.ServicesView); });

            GoToRoadworkInfoCommand = new Command(() => { NavigateTo(Locator.RoadworkInformationPage); });


            ContactSupport = new DelegateCommand(async () =>
            {
                var mailBody = $@"Informations techniques (ne pas supprimer) :
                    OS : {DeviceInfo.Platform} {DeviceInfo.VersionString}
                    Modèle : {DeviceInfo.Manufacturer} {DeviceInfo.Model}
                    Version de l'application : {AppInfo.VersionString}
                    ";
                await Launcher.TryOpenAsync($"mailto:{Constants.CONTACT_EMAIL}?body={Uri.EscapeDataString(mailBody)}");
            });

           
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            VerifVersion();
            GetAllServices();
            GetCards();
            StartNetworkMonitoring();
            GetWorkData();
            if (_session.IsConnected())
            {
                AlertVisible = true;
                
                AlertListViewModel.Limit = 2;
                AlertListViewModel.SeeAllAlertsVisible = true;
                AlertListViewModel.GetAlerts();
                RatingViewModel.VerifyRatingSessionCommand.Execute(null);
            }
            EventDiaryListDashboardViewModel.LoadNews();
        }

        public override async Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedFromAsync(parameters);
            StopNetworkMonitoring();
        }

        private void HandleAction(CardActionDto action)
        {
            if (Constants.PageKeyByCodeService.ContainsKey(action.Target))
            {
                Analytics.TrackEvent(Constants.CardNavigation, new Dictionary<string, string> { { "Click", action.Title } });
                var service = FavoriteService.Services.FirstOrDefault(s => s.Code == action.Target);
                if (service != null)
                    NavigateToService(service, action.Parameter);
                else
                {
                    if (Constants.PageKeyByCodeService.ContainsKey(action.Target))
                    {
                        //fix du dimanche soir
                        var page = Constants.PageKeyByCodeService[action.Target];
                        if (string.IsNullOrEmpty(action.Parameter))
                            NavigationService.NavigateTo(page);
                        else
                            NavigationService.NavigateTo(page, navigationParameters:NavigationParametersFactory(action.Parameter));
                    }
                    else
                        Console.WriteLine($"No page for service code : {action.Target}");
                }
            }
            // Cas particulier pour re télécharger les cards du carousel en cas d'erreur
            else if (action.Target == "RETRY_DOWNLOAD")
            {
                GetCards();
            }
            else
            {
                Console.WriteLine($"No page for service code : {action.Target}");
            }
        }

        public void GetCards(bool? loadedIndicator = null)
        {
            CallApi(async () =>
            {
                var response = await _dashboardService.GetAllCards();
                ManageApiResponses(response, new CallbackManager<DtoListResponse<CardDto>>
                {
                    OnSuccess = (res) => {
                        OnSuccessGetCards(res);
                        if(loadedIndicator != null)
                        {
                            loadedIndicator = true;
                        }
                    },
                    OnError = OnError
                });
            });
        }

        private void OnError(DtoListResponse<CardDto> obj)
        {
            Cards = new ObservableCollection<CardDto> {
                new CardDto
                {
                    Title = "La connexion avec le serveur semble impossible, verifiez votre connexion internet.",
                    Type = "ERROR",
                    Actions = new System.Collections.ObjectModel.ObservableCollection<CardActionDto> { new CardActionDto { Title = "Réessayer", Target = "RETRY_DOWNLOAD" } }
                }
            };
        }

        private void OnSuccessGetCards(DtoListResponse<CardDto> response)
        {
            cards.Clear();
            response.Data.ToList().ForEach( item => cards.Add(item));
        }


        public void GetWorkData()
        {
            CallApi(async () =>
            {
                WorkDataResponse response = await _dashboardService.GetWorkData(_session.Profile.Guid);
                ManageApiResponses(response, new DefaultCallbackManager<WorkDataResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.WorkDataList.Any())
                        {
                            WorkDataList = res.WorkDataList;
                        }
                        SetCurrentWorkDashboardString();
                        SetNewWorkDashboardString();
                    }
                });
            });
        }

        private void SetNewWorkDashboardString()
        {
            var tempCount = 0;
            foreach(var item in WorkDataList)
            {
                if(item.State == "newest")
                {
                    tempCount += item.Count;
                }
            }
            if (tempCount > 1)
            {
                NewWorkDashboardString = tempCount.ToString() + " nouveaux travaux";
            }
            else
            {
                NewWorkDashboardString = tempCount.ToString() + " nouveau travail";
            }
        }

        private void SetCurrentWorkDashboardString()
        {
            var tempCount = 0;
            foreach (var item in WorkDataList)
            {
                if (item.State == "newest")
                {
                    tempCount += item.Count;
                }
                if (item.State == "current")
                {
                    tempCount += item.Count;
                }
            }
            if (tempCount > 1)
            {
                CurrentWorkDashboardString = tempCount.ToString() + " travaux en cours";
            }
            else
            {
                CurrentWorkDashboardString = tempCount.ToString() + " travail en cours";
            }
            
        }

        public void GetAllServices()
        {
            CallApi(async () =>
            {
                if (_session.IsConnected())
                {
                    FavoriteServiceListResponse response = await _servicesService.GetFavouriteServices(_session.Profile.Guid, true);


                    ManageApiResponses(response, new DefaultCallbackManager<FavoriteServiceListResponse>(PopupService)
                    {
                        OnSuccess = (res) =>
                        {
                            OnSuccessFavouriteServices(res.Services.ToList(), res.Scopes.ToList(), res.HasAlertIdentity);
                        }
                    });
                }
                else
                {
                    var response = await _servicesService.GetAllServices();
                    OnSuccessFavouriteServices(response.Data.ToList(), new List<CheckboxModel>(), false);
                }
            });
        }


        public void VerifVersion()
        {
#if !(DEBUG || STAGING)
            CallApi(async () =>
            {
                AppVersionStateResponse response = await _versionService.GetAppVersionState();

                ManageApiResponses(response, new CallbackManager<AppVersionStateResponse>
                {
                    OnSuccess = (res) =>
                    {
                        if (response.Versionning.State.Equals("Obsolète"))
                        {
                            PopupVersionObsoleteView popupObsolete = new PopupVersionObsoleteView()
                            {
                                Message = response.Versionning.Message
                            };
                            PopupService.Show(popupObsolete);
                        }
                        else if (response.Versionning.State.Equals("Suspendu"))
                        {
                            PopupVersionSuspendedView popupSuspended = new PopupVersionSuspendedView()
                            {
                                Message = response.Versionning.Message
                            };
                            PopupService.Show(popupSuspended);
                        }
                    }
                });
            });

#endif
        }

        private void OnSuccessFavouriteServices(List<ServiceDto> services, List<CheckboxModel> scopes, bool hasAlertIdentity)
        {
            List<ServiceLayout> serviceLayout;
            if (_session.IsConnected())
            {
                serviceLayout = ServicesViewModelHelper.TranslateToLayoutService(services.Where(s => s.Visibility != Constants.SERVICE_VISIBLITY_HIDDEN && s.IsFavourite).ToList());
                //AlertVisible = !hasAlertIdentity || (hasAlertIdentity && AlertListViewModel.AlertList != null && AlertListViewModel.AlertList.Any());
                AlertListViewModel.SeeAllAlertsVisible = hasAlertIdentity;
            }
            else
            {
                serviceLayout = ServicesViewModelHelper.TranslateToLayoutService(services.Where(s => s.Visibility != Constants.SERVICE_VISIBLITY_HIDDEN).ToList());
            }
            FavoriteService = new FavoriteService()
            {
                Services = serviceLayout,
                Scopes = scopes.Select(s => new Scope() { Title = s.Title, Checked = s.Checked }).ToList(),
                HasAlertIdentity = hasAlertIdentity
            };
        }
      
    }
}