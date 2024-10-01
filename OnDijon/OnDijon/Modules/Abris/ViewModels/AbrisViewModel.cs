using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Tasks.NetworkAnalysis;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnDijon.Common.Entities;
using OnDijon.Common.Extensions;
using OnDijon.Common.Services;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.Utils.UI;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Abris.Entities.Models;
using OnDijon.Modules.Abris.Entities.Response;
using OnDijon.Modules.Abris.Pages;
using OnDijon.Modules.Abris.Serv.Interfaces;
using OnDijon.Modules.Abris.Views;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Favorites.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace OnDijon.Modules.Abris.ViewModels
{
    public class AbrisViewModel : BaseViewModel
    {

        #region variables

        public bool MapInitialized = false;
        public bool routeExist = false;
        public int countRouteExistMultiAdress = 0;
        readonly INavigationService _NavigationService;


        private Map _map;
        public Map Map
        {
            get { return _map; }
            private set { Set(ref _map, value); }
        }
        public MapPoint CurrentPosition { get; set; }
        public Location SelectedDestination { get; set; }

        private GraphicsOverlay _localisationAbriOverlay;
        private GraphicsOverlay _localisationPositionOverlay;

        private MapView _mapview;

        private List<AbrisModel> _AbrisList { get; set; }
        public List<AbrisModel> AbrisList { 
            get
            {
                return _AbrisList;
            }
            set
            {
                _AbrisList = value;
            }
        }

        #region services
        private readonly IGeolocationService _geolocationService;
        private readonly IAbrisService _abrisService;
        private readonly IFavoriteService _favoriteService;
        readonly ISession _session;
        #endregion

        #region binding
        public AbrisModel SelectedAbris { get; set; }
        public Placemark SelectedPlacemark { get; set; }
        public double DistanceToShelter { get; set; }
        
        public bool IsShelter { get; set; }
        public bool IsConnected { get; set; }

        private string _Recherche;
        public string Recherche { get => _Recherche; set => Set(ref _Recherche, value); }

        private bool _IsDepartSearchBarVisible;
        public bool IsDepartSearchBarVisible { get => _IsDepartSearchBarVisible; set => Set(ref _IsDepartSearchBarVisible, value); }

        private bool _IsDestinationSearchBarVisible;
        public bool IsDestinationSearchBarVisible { get => _IsDestinationSearchBarVisible; set => Set(ref _IsDestinationSearchBarVisible, value); }

        private string _DepartAddress;
        public string DepartAddress { get => _DepartAddress; set => Set(ref _DepartAddress, value); }

        private string _DestinationAddress;
        public string DestinationAddress { get => _DestinationAddress; set => Set(ref _DestinationAddress, value); }


        private bool _IsSingleSearchBarVisible;
        public bool IsSingleSearchBarVisible { get => _IsSingleSearchBarVisible; set => Set(ref _IsSingleSearchBarVisible, value); }
        #endregion

        #region commands

        public DelegateCommand RechercheCommand { get; set; }
        public DelegateCommand SearchAddressCommand { get; set; }
        public DelegateCommand AddFavCommand { get; set; }
        public DelegateCommand ToggleSearchModeCommand { get; set; }
        public ICommand GetCurrentLocationCommand { get; }
        public ICommand LocateMeCommand { get; }
        public ICommand LocateMeDepartAdressCommand { get; }
        public ICommand LocateMeDestinationAdressCommand { get; }
        #endregion

        #endregion

        public AbrisViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IPopupService popupService,
                                   IGeolocationService geolocationService,
                                   IAbrisService abrisService, 
                                   IFavoriteService favoriteService,
                                   ISession session,
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
                                   
        {
            _mapview = AbrisPage.mapView;
            _geolocationService = geolocationService;
            _NavigationService = navigationService;
            _abrisService = abrisService;
            _favoriteService = favoriteService;
            _session = session;
            IsSingleSearchBarVisible = true;
            IsDepartSearchBarVisible = false;
            IsDestinationSearchBarVisible = false;
            if(session.IsConnected()) { Console.WriteLine("connected: guid"+session.Profile.Guid); IsConnected = true; }
            else { Console.WriteLine("not connected"); IsConnected = false; }
            GetCurrentLocationCommand = new Command(async () => { CurrentPosition = await GetCurrentLocation();
                if (CurrentPosition != null)
                {
                    await _mapview.SetViewpointCenterAsync(CurrentPosition, MapUtils.DEFAULT_MAP_SCALE);
                };
            });
            LocateMeCommand = new Command(async () => Recherche = await GetCurrentAddress());
            LocateMeDestinationAdressCommand = new Command(async () => DestinationAddress = await GetCurrentAddress());
            LocateMeDepartAdressCommand = new Command(async () => DepartAddress = await GetCurrentAddress());
            RechercheCommand = new DelegateCommand(async () => await DoRechercheCommand());
            SearchAddressCommand = new DelegateCommand(async () => await DoRechercheAddressCommand());
            ToggleSearchModeCommand = new DelegateCommand(async () => await DoToggleSearchModeCommand());
        }

   
        public async Task DoRechercheCommand()
        {
           
            if ((Recherche != null && Recherche != "") && MapInitialized)
            {
                var locations = await Geocoding.GetLocationsAsync(Recherche + " 21" + " Dijon");
                if (locations != null && locations.Count() > 0)
                {
                    SelectedDestination = locations.FirstOrDefault();
                    MapPoint mapPoint = SelectedDestination?.ToMapPoint();

                    // Zoom sur la position sélectionnée
                    Viewpoint viewpoint = new Viewpoint(mapPoint, MapUtils.DEFAULT_MAP_SCALE);
                    await _mapview.SetViewpointAsync(viewpoint);

                    // Pin sur la map

                    if (SelectedDestination != null && AbrisList != null)
                    {
                        // Convertir la liste de AbrisModel en une liste de MapPoint
                        IEnumerable<MapPoint> abrisMapPoints = AbrisList.Select(abri => AbriToMapPoint(abri));

                        // Trouver l'abri le plus proche
                        MapPoint abriPlusProche = TrouverAbriPlusProche(abrisMapPoints, SelectedDestination.ToMapPoint());
                        
                        // Tracer l'itinéraire entre le point et l'abri le plus proche
                        await TracerItineraire(SelectedDestination.ToMapPoint(), abriPlusProche, false);
                        routeExist = true;
                        await ShowPinLocationSelected(_localisationPositionOverlay, SelectedDestination, false);

                    }


                }
            }
            else
            {
                PopupService.Show(PopupEnum.PopupError, "Action impossible", "Le champ est vide, veuillez le remplir.", "OK");

            };
        }
        public async Task DoRechercheAddressCommand()
        {


            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPK843f7e3ddcad496989254897b2e1cd6dFFjJdmwcQWa_2MP87-gXrkx8ML_nvMMPhcQWRxPv9QZcUk7AzLAAf_KNmsJ6iaFL";
            string adresseDepart = DepartAddress;
            string adresseArrivee = DestinationAddress;
            //Convertir la liste de AbrisModel en une liste de MapPoint
                 IEnumerable<MapPoint> abrisMapPoints = AbrisList.Select(abri => AbriToMapPoint(abri));
                 var Destination = await Geocoding.GetLocationsAsync(DestinationAddress + " 21" + " Dijon");
                 Location locationDestination = Destination.FirstOrDefault();

                 var Depart = await Geocoding.GetLocationsAsync(DepartAddress + " 21" + " Dijon");
                 Location locationDepart = Depart.FirstOrDefault();
                 // Trouver l'abri le plus proche
                 MapPoint abriPlusProche = TrouverAbriPlusProche(abrisMapPoints, locationDestination.ToMapPoint());

            // Tracer l'itinéraire entre le point et l'abri le plus proche
            await TracerItineraire(locationDepart.ToMapPoint(), abriPlusProche, false);

            await TracerItineraire(abriPlusProche, locationDestination.ToMapPoint(), true);
            countRouteExistMultiAdress = 2;


            await ShowPinLocationSelected(_localisationPositionOverlay, locationDepart, false);
                 await ShowPinLocationSelected(_localisationPositionOverlay, locationDestination, true);
      



        }

        public async Task DoToggleSearchModeCommand()
        {
            IsSingleSearchBarVisible = !IsSingleSearchBarVisible;

            if (IsSingleSearchBarVisible)
            {
                IsDepartSearchBarVisible = false;
                IsDestinationSearchBarVisible = false;
            }
            else
            {
                IsDepartSearchBarVisible = true;
                IsDestinationSearchBarVisible = true;
            };
          
        }

        public MapPoint AbriToMapPoint(AbrisModel abri)
        {
            return new MapPoint(abri.GeoPointLon, abri.GeoPointLat, SpatialReferences.Wgs84);
        }
        private MapPoint TrouverAbriPlusProche(IEnumerable<MapPoint> abris, MapPoint point)
        {
            double distanceMin = double.MaxValue;
            MapPoint abriPlusProche = null;

            foreach (var abri in abris)
            {
                double distance = CalculerDistance(point, abri);
                if (distance < distanceMin)
                {
                    distanceMin = distance;
                    abriPlusProche = abri;
                }
            }
            return abriPlusProche;
        }

        private double CalculerDistance(MapPoint point1, MapPoint point2)
        {
            // Convertir les points en une ligne (polyline) pour le calcul de distance
            var polylineBuilder = new PolylineBuilder(SpatialReferences.Wgs84);
            polylineBuilder.AddPoint(point1);
            polylineBuilder.AddPoint(point2);
            var polyline = polylineBuilder.ToGeometry();

            // Calculer la distance entre les deux points en mètres
            double distanceEnMetres = GeometryEngine.LengthGeodetic(polyline, LinearUnits.Meters, GeodeticCurveType.Geodesic);

            // Convertir la distance en kilomètres
            double distanceEnKilometres = distanceEnMetres / 1000;

            return distanceEnKilometres;
        }
        private async Task TracerItineraire(MapPoint depart, MapPoint arrivee, bool isForMultipleAddress)
        {
            
            if(routeExist || countRouteExistMultiAdress == 2)
            {
                ResetMapView();
                InitMapView();
                GetAbrisList();
                countRouteExistMultiAdress = 0;
                routeExist = false;
            }
            

            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPK843f7e3ddcad496989254897b2e1cd6dFFjJdmwcQWa_2MP87-gXrkx8ML_nvMMPhcQWRxPv9QZcUk7AzLAAf_KNmsJ6iaFL";

            try
            {
                // Créer une instance de RouteTask
                var routeServiceUri = new Uri("https://route-api.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World");
                var routeTask = await RouteTask.CreateAsync(routeServiceUri, Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey);

                // Créer les points de départ et d'arrivée
                var departStop = new Stop(depart);
                var arriveeStop = new Stop(arrivee);
                var stops = new List<Stop> { departStop, arriveeStop };

                // Créer les paramètres de l'itinéraire
                var routeParameters = await routeTask.CreateDefaultParametersAsync();
                routeParameters.SetStops(stops);

                // Résoudre l'itinéraire
                var routeResult = await routeTask.SolveRouteAsync(routeParameters);

                if (routeResult.Routes.Count > 0)
                {
                    var route = routeResult.Routes[0];
            

                    // Obtenir la distance totale en mètres
                    double distanceInMeters = route.TotalLength;
               
                    // Convertir la distance en kilomètres
                    double distanceInKilometers = distanceInMeters / 1000;

                    DistanceToShelter = distanceInKilometers;
                    var routeSymbol = new SimpleLineSymbol();
                    if (isForMultipleAddress)
                    {
                        // Afficher l'itinéraire sur la carte
                        routeSymbol = new SimpleLineSymbol()
                        {
                            Style = SimpleLineSymbolStyle.Solid,
                            Color = System.Drawing.Color.FromArgb(255, 0, 0),
                            Width = 5 // Augmenter la largeur de la ligne pour un aspect plus visible
                        };
                    }
                    else
                    {
                        // Afficher l'itinéraire sur la carte
                        routeSymbol = new SimpleLineSymbol()
                        {
                            Style = SimpleLineSymbolStyle.Solid,
                            Color = System.Drawing.Color.FromArgb(0, 120, 215),
                            Width = 5 // Augmenter la largeur de la ligne pour un aspect plus visible
                        };
                    }
             

                    var routeGraphic = new Graphic(route.RouteGeometry, routeSymbol);
                    var routeGraphicsOverlay = new GraphicsOverlay();
                    routeGraphicsOverlay.Graphics.Add(routeGraphic);
                    _mapview.GraphicsOverlays.Add(routeGraphicsOverlay);
                 

                    // Zoom sur l'itinéraire
                    await _mapview.SetViewpointGeometryAsync(route.RouteGeometry, 100);
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs lors de la résolution de l'itinéraire
                Console.WriteLine($"Erreur lors de la résolution de l'itinéraire : {ex.Message}");
            }
        }
        #region recup abris

        public void GetAbrisList()
        {
            CallApi(async () =>
            {
                AbrisListResponse response = await _abrisService.GetAbris();
                ManageApiResponses(response, new DefaultCallbackManager<AbrisListResponse>(PopupService)
                {

                    OnSuccess = (res) =>
                    {
                        if (AbrisList != null && AbrisList.Any())
                        {
                            AbrisList.Clear();
                        }
                        
                        if (res.AbrisList.Any())
                        {

                            OnAbrisLoaded(res.AbrisList);
                        }
                    }
                }) ;
            });

        }

        private async void OnAbrisLoaded(List<AbrisModel> abrisList)
        {
            AbrisList= abrisList;
            await ShowAbrisOnMapAsync(_localisationAbriOverlay, abrisList);
            
        }

        private async Task ShowAbrisOnMapAsync(GraphicsOverlay positionOverlay, List<AbrisModel> abrisList)
        {
            positionOverlay.Graphics.Clear();

            if (abrisList.Count > 0)
            {
                double x, y;
                foreach (var abri in abrisList.OrderByDescending(a => a.GeoPointLat))
                {

                    x = abri.GeoPointLon;
                    y = abri.GeoPointLat;
                    await ShowPinGraphicAbrisOnMap(positionOverlay, new MapPoint(x, y, new SpatialReference(4326)), abri);
                }
                if (abrisList.Count == 1)
                {
                    // Zoom sur la position du pin
                    Viewpoint viewpoint = new Viewpoint(new MapPoint(abrisList.First().GeoPointLon, abrisList.First().GeoPointLat, new SpatialReference(4326)), MapUtils.DEFAULT_MAP_SCALE);
                    await _mapview.SetViewpointAsync(viewpoint);
                }

            }
        }

        #endregion

        #region handling map
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
            InitMapView();
            MapInitialized = true;   
        }

        private void InitMapView()
        {
            _mapview.DismissCallout();
            _mapview.LayerViewStateChanged += MapView_LayerViewStateChanged;
            _mapview.GeoViewTapped += MapView_GeoViewTapped;

            _localisationAbriOverlay = new GraphicsOverlay();
            _mapview.GraphicsOverlays.Add(_localisationAbriOverlay);
            _localisationPositionOverlay = new GraphicsOverlay();
            _mapview.GraphicsOverlays.Add(_localisationPositionOverlay);

        }

        private void ResetMapView()
        {
            _mapview.LayerViewStateChanged -= MapView_LayerViewStateChanged;
            _mapview.GeoViewTapped -= MapView_GeoViewTapped;
            _mapview.GraphicsOverlays.Clear();
        }

        private async void MapView_LayerViewStateChanged(object sender, LayerViewStateChangedEventArgs e)
        {
            if (e.LayerViewState.Status == LayerViewStatus.Active)
            {
                await _mapview.SetViewpointAsync(MapUtils.DEFAULT_MAP_VIEWPOINT);
            }
        }

        private async void MapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            await UnselectAllPinsAbris();

            //Si clic sur un abri
            var resultAbri = await _mapview.IdentifyGraphicsOverlayAsync(_localisationAbriOverlay, e.Position, 20, false, 1);
            if (resultAbri.Graphics.Any())
            {
                Graphic SelectedGraphicContact = resultAbri.Graphics[0];
                if (SelectedGraphicContact != null)
                {
                    await SelectPinAbris(SelectedGraphicContact);
                }
            }

            //Si clic sur l'adresse de destination épinglée
            var resultPosition = await _mapview.IdentifyGraphicsOverlayAsync(_localisationPositionOverlay, e.Position, 20, false, 1);
            if (resultPosition.Graphics.Any())
            {
                Graphic SelectedGraphicContact = resultPosition.Graphics[0];
                string selectedLocationJson = (string)SelectedGraphicContact.Attributes["selected_address"];

                SelectedDestination = JsonConvert.DeserializeObject<Location>(selectedLocationJson);
                try
                {
                    SelectedPlacemark = await GetPlacemark(SelectedDestination);
                    if (SelectedPlacemark != null) { PopupService.Show(new AbrisDetailView(_favoriteService, _session, this)); }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
              
            }
        }



        #endregion

        #region utils handling location

        public async Task<string> GetCurrentAddress()
        {
           
            string currentAddress = "";
            try { 
                CurrentPosition= await GetCurrentLocation(); 
                if (CurrentPosition != null)
                {
                    await _mapview.SetViewpointCenterAsync(CurrentPosition, MapUtils.DEFAULT_MAP_SCALE);

                }
            } catch (Exception ex) { Console.WriteLine(ex.Message);}
            
            if (CurrentPosition != null)
            {
                try
                {
                    Placemark placemark = await GetPlacemark(new Location(CurrentPosition.Y, CurrentPosition.X));
                    currentAddress = SetAddress(placemark);
               
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return currentAddress;;
        }

        private string SetAddress(Placemark placemark)
        {
            string address = "";

            if (placemark != null)
            {
                if (!string.IsNullOrEmpty(placemark.FeatureName))
                {
                    address = placemark.FeatureName+" ";
                }
                if (!string.IsNullOrEmpty(placemark.Thoroughfare))
                {
                    address += placemark.Thoroughfare+" ";
                }

                if (!string.IsNullOrEmpty(placemark.Locality))
                {
                    address += placemark.Locality+ " ";
                }

                if (!string.IsNullOrEmpty(placemark.PostalCode))
                {
                    address += placemark.PostalCode;
                }
            }
            return address;
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
                PopupService.Show(PopupEnum.PopupError, "Action impossible", "Impossible d'activer la géolocalisation", "OK");
            }

            return currentLocation?.ToMapPoint();
        }

        private async Task<Placemark> GetPlacemark(Location selectedLocation)
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(selectedLocation);

            if (placemarks != null && placemarks.Any())
            {
                return placemarks.FirstOrDefault();
            }
            return null;
        }

        #endregion

        #region handle pins
        private async Task ShowPinLocationSelected(GraphicsOverlay positionOverlay, Location location, bool isForMultipleAdress)
        {
            if (isForMultipleAdress == false)
            {
                positionOverlay.Graphics.Clear();

            }

            MapPoint mapPoint = location?.ToMapPoint();
            PictureMarkerSymbol pinSymbol = null;
            
            if (isForMultipleAdress)
            {
                pinSymbol = await MapUtils.CreatePictureMarkerSymbolFromResources("OnDijon.Assets.terminer.png");
                pinSymbol.Width = 37;
                pinSymbol.Height = 49;
            }
            else
            {
                pinSymbol = await MapUtils.CreatePictureMarkerSymbolFromResources("OnDijon.Assets.pinMyPosition.png");
                pinSymbol.Width = 37;
                pinSymbol.Height = 49;
            }
            //création de la pin

            var pinGraphic = new Graphic(mapPoint, pinSymbol);


            //Ajout sur la carte de la pin avec l'adresse


            string jsonLocation = JsonConvert.SerializeObject(location);
            pinGraphic.Attributes.Add("selected_address", jsonLocation);
            positionOverlay.Graphics.Add(pinGraphic);
        }

        private async Task ShowPinGraphicAbrisOnMap(GraphicsOverlay positionOverlay, MapPoint mapPoint, AbrisModel abri)
        {
            //création de la pin
            var pinSymbol = await MapUtils.CreatePictureMarkerSymbolFromResources("OnDijon.Assets.pin.png");
            pinSymbol.Width = 37;
            pinSymbol.Height = 49;
            pinSymbol.OffsetY = pinSymbol.Height / 2;

            //Ajout sur la carte de la pin avec l'adresse

            var pinGraphic2 = new Graphic(mapPoint, pinSymbol);

            pinGraphic2.Attributes.Add("abri", abri.RecordId);
            
            positionOverlay.Graphics.Add(pinGraphic2);

        }

        private async Task UnselectAllPinsAbris()
        {
            DistanceToShelter = 0.0;
            IsShelter = false;
            foreach (Graphic graph in _localisationAbriOverlay.Graphics)
            {
                if (graph != null)
                {
                    // Modifier les propriétés du pin
                    var pinSymbol = await MapUtils.CreatePictureMarkerSymbolFromResources("OnDijon.Assets.pin.png");
                    pinSymbol.Width = 37;
                    pinSymbol.Height = 49;
                    pinSymbol.OffsetY = pinSymbol.Height / 2;

                    // Mettre à jour le symbole du pin
                    graph.Symbol = pinSymbol;
                }
            }
        }

        private async Task SelectPinAbris(Graphic SelectedGraphicContact)
        {
            // Modifier les propriétés du pin
            var pinSymbol = await MapUtils.CreatePictureMarkerSymbolFromResources("OnDijon.Assets.pinSelected.png");
            pinSymbol.Width = 37;
            pinSymbol.Height = 49;
            pinSymbol.OffsetY = pinSymbol.Height / 2;

            // Mettre à jour le symbole du pin
            SelectedGraphicContact.Symbol = pinSymbol;

            // Récupération de l'abri
            string idAbri = (string)SelectedGraphicContact.Attributes["abri"];
            SelectedAbris = AbrisList.FirstOrDefault(abri => abri.RecordId == idAbri);

            if (SelectedAbris != null)
            {
                IsShelter = true;
                try
                {
                    SelectedPlacemark = await GetPlacemark(new Location(SelectedAbris.GeoPointLat, SelectedAbris.GeoPointLon));
                  
        
                    // Actualiser l'itinéraire existant
                    if (SelectedDestination != null && countRouteExistMultiAdress == 0)
                    {
                        var SelectedAbrisAsMapPoint = AbriToMapPoint(SelectedAbris);
                        var SelectedDestinationAsMapPoint = SelectedDestination.ToMapPoint();
                        await TracerItineraire(SelectedDestinationAsMapPoint, SelectedAbrisAsMapPoint, false);
                        routeExist = true;

                        await ShowPinLocationSelected(_localisationPositionOverlay, SelectedDestination, false);

                    }

                    PopupService.Show(new AbrisDetailView(_favoriteService, _session, this));
  
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("no abri found for abri_id");
            }
        }
        #endregion

        #region navigation
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
                {
                    InitMap();
                    GetAbrisList();
                    await base.OnNavigatedToAsync(parameters);
                }

        public override async Task OnNavigatedFromAsync(INavigationParameters parameters)
                {
                    await base.OnNavigatedToAsync(parameters);
                    ResetMapView();
                    Cleanup();
                }
        #endregion
    }
}
