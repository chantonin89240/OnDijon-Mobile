using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DryIoc;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Entities;
using OnDijon.Modules.Report.Entities.Dto;
using OnDijon.Modules.Report.Entities.Response;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils.UI;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Response;
using OnDijon.Common.Extensions;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using OnDijon.Modules.Report.Services.Interfaces;
using Prism.Navigation;
using Newtonsoft.Json;
using OnDijon.Common.Utils;
using AsyncAwaitBestPractices.MVVM;

namespace OnDijon.Modules.Report.ViewModels
{
    public class ReportLocalisationViewModel : ReportBaseViewModel
    {
        private readonly ISession _session;
        private readonly IReportService _reportService;
        private readonly IGeolocationService _geolocationService;
        private readonly IUserIdService _userIdService;

        public ICommand GetCurrentLocationCommand { get; }

        public ICommand GetReportsCommand { get; }

        public ICommand GoToReportCommand { get; }

        public ICommand GoToNextPageCommand { get; }

        public IList<string> Suggestions { get; private set; }

        public string Address
        {
            get { return _session.ReportRequest.ReportContent.Address; }
            set
            {
                if (!string.Equals(value, _session.ReportRequest.ReportContent.Address))
                {
                    _session.ReportRequest.ReportContent.Address = value;
                    RaisePropertyChanged(nameof(Address));
                }
            }
        }

        public MapPoint CurrentPosition
        {
            get
            {
                return _session.ReportRequest.ReportContent.Position?.ToMapPoint();
            }
            set
            {
                _session.ReportRequest.ReportContent.Position = value?.ToCoordinatesRequest();
                RaisePropertyChanged(nameof(CurrentPosition));
            }
        }

        public IList<ReportDto> Reports { get; private set; }

        private Map _map;
        public Map Map
        {
            get { return _map; }
            private set { Set(ref _map, value); }
        }

        public ReportLocalisationViewModel(INavigationService navigationService,
                                           ITranslationService translationService,
                                           IPopupService popupService,
                                           ISession session,
                                           IReportService reportService,
                                           IGeolocationService geolocationService,
                                           IUserIdService userIdService,
                                           ILoggerService loggerService) : base(navigationService, translationService, popupService, session, loggerService)
        {
            _session = session;
            _reportService = reportService;
            _geolocationService = geolocationService;
            _userIdService = userIdService;

            GetCurrentLocationCommand = new Command(async () => CurrentPosition = await GetCurrentLocation());

            GetReportsCommand = new Command<MapPoint>(GetReports);

            GoToReportCommand = new AsyncCommand<ReportDto>(async (report) => await NavigationService.NavigateAsync(Locator.ReportDetailView,NavigationParametersFactory(report,Constants.ReportDetailNavigationKey)));

            GoToNextPageCommand = new AsyncCommand(async () => await NavigationService.NavigateAsync(Locator.ReportDescriptionView));
        }

        public void OnPageInit()
        {
            Suggestions = null;
            _session.ReportRequest.ReportContent.Address = null;
            _session.ReportRequest.ReportContent.Position = null;
        }

        public void InitMap()
        {
            try
            {
                Map = new Map(Basemap.CreateOpenStreetMap())
                {
                    MinScale = MapUtils.MIN_MAP_SCALE
                };
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors de l'initialisation de la carte", ex);
            }
        }

        public void GetCoordinatesFromAddress(string address)
        {
            Address = address;

            //géocodage
            CallApi(async () =>
            {
                CoordinatesResponse coordinatesResponse = await _reportService.GetCoordinatesFromAddress(address);
                ManageApiResponses(coordinatesResponse, new CallbackManager<CoordinatesResponse>
                {
                    OnSuccess = (res) => CurrentPosition = res.ToMapPoint(),
                    OnInvalidInformations = (res) =>
                    {
                        CurrentPosition = null;
                        ShowError(res.Message);
                    }
                });
            });
        }

        public void GetSuggestions(string address)
        {
            CallApi(async () =>
            {
                AddressesListResponse addressesListResponse = await _reportService.GetSuggestions(address,5);
                ManageApiResponses(addressesListResponse, new CallbackManager<AddressesListResponse>
                {
                    OnSuccess = (res) =>
                    {
                        Suggestions = res.Suggestions;
                        RaisePropertyChanged(nameof(Suggestions));
                    }
                });
            });
        }

        public void GetAddressFromCoordinates(MapPoint coordinates)
        {
            //check if coordinates are valid
            if (GeometryEngine.Contains(MapUtils.VALID_AREA, coordinates))
            {
                CallApi(async () =>
                {
                    Entities.Response.AddressResponse addressResponse = await _reportService.GetAddressFromCoordinates(coordinates);
                    ManageApiResponses(addressResponse, new CallbackManager<Entities.Response.AddressResponse>
                    {
                        OnSuccess = (res) => Address = res.Address,
                        OnInvalidInformations = (res) => Address = "adresse inconnue"
                    });
                });
            }
            else
            {
                CurrentPosition = null;
            }
        }

        private async Task<MapPoint> GetCurrentLocation()
        {
            Location currentLocation = null;
            IsLoading = true;

            try
            {
                currentLocation = await _geolocationService.GetCurrentLocation();
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                IsLoading = false;
                PopupService.Show(PopupEnum.PopupError, "Action impossible", "Votre appareil ne dispose pas d'un GPS", "OK");
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
                IsLoading = false;
                PopupService.Show(PopupEnum.PopupInfo, "Veuillez activer la géolocalisation pour effectuer cette action", "OK");
            }
            catch (PermissionException)
            {
                // Handle permission exception
                IsLoading = false;
                PopupService.Show(PopupEnum.PopupError, "Action impossible", "L'application ne peut pas utiliser votre position", "OK");
            }
            catch (Exception ex)
            {
                // Unable to get location
                IsLoading = false;
                System.Diagnostics.Debug.Fail(ex.ToString());
                Crashes.TrackError(ex);
                PopupService.Show(PopupEnum.PopupError, "Action impossible", "Impossible d'activer la géolocalisation", "OK");
            }

            return currentLocation?.ToMapPoint();
        }

        private void GetReports(MapPoint coordinates)
        {
            if (coordinates == null) { return; }

            CallApi(async () =>
            {
                var response = await _reportService.GetReports(coordinates, await _userIdService.GetUserId());
                ManageApiResponses(response, new CallbackManager<DtoListResponse<ReportDto>>
                {
                    OnSuccess = (res) =>
                    {
                        Reports = res.Data;
                        GetReportsIcons();
                    }
                });
            });
        }

        private void GetReportsIcons()
        {
            CallApi(async () =>
            {
                ReportTypesListResponse response = await _reportService.GetReportTypes();
                ManageApiResponses(response, new CallbackManager<ReportTypesListResponse>
                {
                    OnSuccess = (res) =>
                    {
                        foreach (var report in Reports)
                        {
                            var type = res.ReportTypes.FirstOrDefault(t => t.Code.Equals(report.TypeCode));
                            report.TypeIconUrl = type?.ImageUrl;
                            report.TypeName = type?.Name ?? string.Empty;
                        }

                        RaisePropertyChanged(nameof(Reports));
                    }
                });
            });
        }

        public override void Cleanup()
        {
            Map = null;
            base.Cleanup();
        }
    }
}
