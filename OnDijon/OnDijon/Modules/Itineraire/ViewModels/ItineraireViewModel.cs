using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.UsefulContact.Entities.Models;
using System;
using OnDijon.Common.Services.Interfaces.Front;
using Xamarin.Forms;
using OnDijon.Modules.UsefulContact.Services.Interfaces;
using OnDijon.Modules.UsefulContact.Entities.Responses;
using OnDijon.Common.Utils.Enums;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using OnDijon.Common.Utils.UI;
using System.Threading.Tasks;
using OnDijon.Common.Utils.Services.Interfaces;
using Xamarin.Essentials;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using OnDijon.Common.Entities;
using Xamarin.Forms.Internals;
using OnDijon.Modules.UsefulContact.Views;
using OnDijon.Modules.Account.Services.Interfaces;
using Prism.Navigation;
using OnDijon.Modules.UsefulContact.ViewsModels;
using OnDijon.Modules.Favorites.Entities.Models;
using Prism.Commands;
using OnDijon.Modules.Itineraire.Entities.Model;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.Tasks.NetworkAnalysis;
using System.Collections.Generic;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Portal;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Esri.ArcGISRuntime.Security;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Esri.ArcGISRuntime.Symbology;
using Mapsui.UI.Forms;
using System.Globalization;
using Esri.ArcGISRuntime.Data;
using OnDijon.Modules.Alert.Services;
using FFImageLoading;
using OnDijon.Common.Services;
using OnDijon.Common.Extensions;
using Mapsui.Providers.ArcGIS;

namespace OnDijon.Modules.Itineraire.ViewModels
{
    public class ItineraireViewModel : BaseViewModel
    {
        readonly IContactDomainService _ContactDomainService;
        readonly IContactService _ContactService;
        private readonly IGeolocationService _geolocationService;
        public bool routeExist = false;
        readonly INavigationService _NavigationService;

        #region variables
        public ICommand GetCurrentLocationCommand { get; }

        private ObservableCollection<ContactDomainModel> _DomainList { get; set; }
        public ObservableCollection<ContactDomainModel> DomainList
        {
            get
            {
                return _DomainList;
            }
            set
            {
                _DomainList = value;
                RaisePropertyChanged(nameof(DomainList));
            }
        }

        #region ContactDetailViewModel => ContactDetail

        private ContactDetailViewModel _contactDetail;

        public ContactDetailViewModel ContactDetail { get => _contactDetail; set => SetProperty(ref _contactDetail, value); }

        #endregion

        #region WorkInfosViewModel => WorkInfosDetail

        private WorkInfosViewModel _workInfosDetail;

        public WorkInfosViewModel WorkInfosDetail { get => _workInfosDetail; set => SetProperty(ref _workInfosDetail, value); }

        #endregion

        public ContactDomainModel DomainSelected { get; set; }

        private ObservableCollection<ContactModel> _ContactList { get; set; }
        public ObservableCollection<ContactModel> ContactList
        {
            get
            {
                return _ContactList;
            }
            set
            {
                _ContactList = value;
                RaisePropertyChanged(nameof(ContactList));
            }
        }

        private string _Recherche;
        public string Recherche { get => _Recherche; set => Set(ref _Recherche, value); }
        public ContactModel ContactSelected { get; set; }

        public MapPoint CurrentPosition { get; set; }

        private Map _map;
        public Map Map
        {
            get { return _map; }
            set { Set(ref _map, value); }
        }

        private Esri.ArcGISRuntime.Xamarin.Forms.MapView _mapView;
        public Esri.ArcGISRuntime.Xamarin.Forms.MapView MapView
        {
            get { return _mapView; }
            set { Set(ref _mapView, value); }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { Set(ref _isRefreshing, value); }
        }

        private bool _isContactDetailDisplay = false;
        public bool IsContactDetailDisplay
        {
            get { return _isContactDetailDisplay; }
            set { Set(ref _isContactDetailDisplay, value); }
        }

        private bool _isWorkInfosDetailDisplay = false;
        public bool IsWorkInfosDetailDisplay
        {
            get { return _isWorkInfosDetailDisplay; }
            set { Set(ref _isWorkInfosDetailDisplay, value); }
        }

        public DelegateCommand ValiderItineraireCommand { get; set; }


        public Command LoadItemsCommand { get; set; }
        public Command SearchCommand { get; set; }
        public Command ShowDomainOnMapCommand { get; set; }
        public Command ViewContactCommand { get; set; }
        public ICommand LocateMeCommandDepart { get; }
        public ICommand LocateMeCommandArrivee { get; }

        private string adresseDepart;

        private double distanceKm;

        public string AdresseDepart { get => adresseDepart; set => Set(ref adresseDepart, value); }
        public double DistanceKm { get => distanceKm; set => Set(ref distanceKm, value); }

        private string adresseArrivee;

        public string AdresseArrivee { get => adresseArrivee; set => Set(ref adresseArrivee, value); }

        #endregion

        public ItineraireViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IPopupService popupService,
                                   IContactDomainService ContactDomainService,
                                   IContactService ContactService,
                                   IGeolocationService geolocationService,
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _NavigationService = navigationService;
            _ContactDomainService = ContactDomainService;
            _ContactService = ContactService;
            _geolocationService = geolocationService;
            GetCurrentLocationCommand = new Command(async () =>
            {
                CurrentPosition = await GetCurrentLocation();
                if (CurrentPosition != null)
                {
                    await MapView.SetViewpointCenterAsync(CurrentPosition, MapUtils.DEFAULT_MAP_SCALE);
                };
            });
            LocateMeCommandDepart = new Command(async () => AdresseDepart = await GetCurrentAddress());
            LocateMeCommandArrivee = new Command(async () => AdresseArrivee = await GetCurrentAddress());

            DomainList = new ObservableCollection<ContactDomainModel>();
            ContactList = new ObservableCollection<ContactModel>();
            SearchCommand = new Command(GetContactList);
            ViewContactCommand = new Command(ViewContact);
            ContactDetail = App.Locator.GetInstance<ContactDetailViewModel>();
            ContactDetail.CloseContactDetailDisplayAction = () => IsContactDetailDisplay = false;
            WorkInfosDetail = App.Locator.GetInstance<WorkInfosViewModel>();
            WorkInfosDetail.CloseWorkInfosDetailDisplayAction = () => IsWorkInfosDetailDisplay = false;
            ValiderItineraireCommand = new DelegateCommand(async () => await DoValiderItineraireCommand());
           

        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            GetDomainList();
            InitMap();
            await base.OnNavigatedToAsync(parameters);
        }

        public override async Task OnNavigatedFromAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            Cleanup();
        }

        public void GetDomainList()
        {
            CallApi(async () =>
            {
                ContactDomainListResponse response = await _ContactDomainService.GetDomains();
                ManageApiResponses(response, new DefaultCallbackManager<ContactDomainListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        DomainList.Clear();
                        if (res.ContactDomainList.Any())
                        {
                            //res.ContactDomainList.ForEach(DomainList.Add);
                            DomainList = new ObservableCollection<ContactDomainModel>(res.ContactDomainList);
                        }
                    }
                });
            });
        }

        public void GetContactList()
        {
            if (DomainSelected != null || (!string.IsNullOrEmpty(Recherche) && Recherche.Length > 2))
            {
                CallApi(async () =>
                {
                    ContactListResponse response = await _ContactDomainService.SearchContact(string.IsNullOrEmpty(Recherche) ? " " : Recherche, DomainSelected != null ? DomainSelected.Id : "");

                    ManageApiResponses(response, new DefaultCallbackManager<ContactListResponse>(PopupService)
                    {
                        OnSuccess = (res) =>
                        {
                            ContactList.Clear();
                            if (res.ContactList.Any())
                            {
                                var contactList = new ObservableCollection<ContactModel>();
                                res.ContactList.ForEach(c =>
                                {
                                    contactList.Add(new ContactModel()
                                    {
                                        EditId = c.EditId,
                                        Name = c.Name,
                                        Address = c.Address,
                                        ElementType = c.ElementType,
                                        X = c.X,
                                        Y = c.Y,
                                        ContactInfos = c.ContactInfos == null ? null : new ContactInfosModel()
                                        {
                                            Mail = c.ContactInfos.Mail,
                                            PhoneNumber = c.ContactInfos.PhoneNumber,
                                            PictureUrl = c.ContactInfos.PictureUrl,
                                            OpeningTime = c.ContactInfos.OpeningTime,
                                            Description = c.ContactInfos.Description
                                        },
                                        WorkInfos = c.WorkInfos == null ? null : new WorkInfosModel()
                                        {
                                            Applicant = c.WorkInfos.Applicant,
                                            Description = c.WorkInfos.Description,
                                            Executant = c.WorkInfos.Executant,
                                            StartDate = c.WorkInfos.StartDate,
                                            EndDate = c.WorkInfos.EndDate
                                        }
                                    });
                                });
                                ContactList = contactList;
                            }
                            else
                            {
                                ContactList = new ObservableCollection<ContactModel>();
                            }
                        }
                    });
                });
            }
        }

        public void ViewContact()
        {

            if (ContactSelected.ElementType == "infosTravaux")
            {

                WorkInfosDetail.Contact = ContactSelected;
                IsWorkInfosDetailDisplay = true;
                //NavigateTo(Locator.WorkInfosPage);
            }
            else
            {
                ContactDetail.Contact = ContactSelected;
                ContactDetail.LoadData();
                IsContactDetailDisplay = true;
                //NavigateTo(Locator.ContactDetailPage);
            }
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


    public async Task DoValiderItineraireCommand()
    {

    if (routeExist)
    {
        routeExist = !routeExist;
        MapView.GraphicsOverlays.Clear();                

    }
    Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPK843f7e3ddcad496989254897b2e1cd6dFFjJdmwcQWa_2MP87-gXrkx8ML_nvMMPhcQWRxPv9QZcUk7AzLAAf_KNmsJ6iaFL";
        string adresseDepart = AdresseDepart;
        string adresseArrivee = AdresseArrivee;

        try
        {
            string geocodeUrl = "https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/findAddressCandidates?";
        string departUrl = $"{geocodeUrl}f=json&singleLine={Uri.EscapeDataString(adresseDepart + " 21" + " Dijon")}&outFields=Match_addr,Addr_type";
        string arriveeUrl = $"{geocodeUrl}f=json&singleLine={Uri.EscapeDataString(adresseArrivee + " 21" + " Dijon")}&outFields=Match_addr,Addr_type";
            HttpClient client = new HttpClient();
            HttpResponseMessage departResponse = await client.GetAsync(departUrl);
            HttpResponseMessage arriveeResponse = await client.GetAsync(arriveeUrl);

            if (!(departResponse.IsSuccessStatusCode && arriveeResponse.IsSuccessStatusCode))
            {
                throw new Exception("Erreur lors de la récupération des adresses.");
            }

            string departResult = await departResponse.Content.ReadAsStringAsync();
            string arriveeResult = await arriveeResponse.Content.ReadAsStringAsync();

            var departJson = JObject.Parse(departResult);
            var arriveeJson = JObject.Parse(arriveeResult);

            var candidatesDepart = departJson["candidates"];
            var candidatesArrivee = arriveeJson["candidates"];

            double departX = 0;
            double departY = 0;
            double arriveeX = 0;
            double arriveeY = 0;

            if (candidatesDepart.Any())
            {
                var firstCandidate = candidatesDepart[0];
                departX = firstCandidate["location"]["x"].Value<double>();
                departY = firstCandidate["location"]["y"].Value<double>();
            }

            if (candidatesArrivee.Any())
            {
                var firstCandidate = candidatesArrivee[0];
                arriveeX = firstCandidate["location"]["x"].Value<double>();
                arriveeY = firstCandidate["location"]["y"].Value<double>();
            }

            var routeServiceUri = new Uri("https://route-api.arcgis.com/arcgis/rest/services/World/Route/NAServer/Route_World");
            var routeTask = await RouteTask.CreateAsync(routeServiceUri, Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey);

            var pointDepart = new MapPoint(departX, departY, SpatialReferences.Wgs84);
            var pointArrivee = new MapPoint(arriveeX, arriveeY, SpatialReferences.Wgs84);

            var departStop = new Stop(pointDepart);
            var arriveeStop = new Stop(pointArrivee);
            var stops = new List<Stop> { departStop, arriveeStop };

            var routeParameters = await routeTask.CreateDefaultParametersAsync();
            routeParameters.SetStops(stops);

            var routeResult = await routeTask.SolveRouteAsync(routeParameters);
            double distanceItineraire = 0; // Variable pour stocker la distance

            if (routeResult.Routes.Count > 0)
            {
                var route = routeResult.Routes[0];
                distanceItineraire = route.TotalLength; // Stockage de la distance de l'itinéraire
                double distanceItineraireKm = distanceItineraire / 1000;
                DistanceKm = distanceItineraireKm;

                var routeSymbol = new SimpleLineSymbol()
                {
                    Style = SimpleLineSymbolStyle.Solid,
                    Color = System.Drawing.Color.FromArgb(0, 120, 215),
                    Width = 5 // Augmenter la largeur de la ligne pour un aspect plus visible
                };

                var routeGraphics = new GraphicsOverlay();

            var pinSymbolArrivee = await MapUtils.CreatePictureMarkerSymbolFromResources("OnDijon.Assets.terminer.png");
                    pinSymbolArrivee.Width = 40;
                    pinSymbolArrivee.Height = 40;
                    pinSymbolArrivee.OffsetY = pinSymbolArrivee.Height / 2;

            var pinSymbolDepart = await MapUtils.CreatePictureMarkerSymbolFromResources("OnDijon.Assets.debut.png");
                    pinSymbolDepart.Width = 40;
                    pinSymbolDepart.Height = 40;
                    pinSymbolDepart.OffsetY = pinSymbolArrivee.Height / 2;

                    var departGraphic = new Graphic(pointDepart, pinSymbolDepart);
                routeGraphics.Graphics.Add(departGraphic);

            var arriveeGraphic = new Graphic(pointArrivee, pinSymbolArrivee);

                routeGraphics.Graphics.Add(arriveeGraphic);

                var polyline = route.RouteGeometry as Esri.ArcGISRuntime.Geometry.Polyline;
                var routeGraphic = new Graphic(polyline, routeSymbol);
                routeGraphics.Graphics.Add(routeGraphic);

                MapView.GraphicsOverlays.Add(routeGraphics);
                await MapView.SetViewpointGeometryAsync(route.RouteGeometry, 100);
            routeExist = true;
        }
            else
            {
                throw new Exception("Aucun itinéraire trouvé.");
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("null"))
            {
                PopupService.Show(PopupEnum.PopupError, "Erreur", "Veuillez remplir les champs.", "OK");
            }
            else if (ex.Message.Contains("Unable"))
            {
                PopupService.Show(PopupEnum.PopupError, "Erreur", "Les adresses renseignées ne sont pas du bon format", "OK");
            }
            else
            {
                PopupService.Show(PopupEnum.PopupError, "Erreur", ex.Message, "OK");
            }
        }
    }

        public async Task<string> GetCurrentAddress()
        {
            string currentAddress = "";
            try
            {
                CurrentPosition = await GetCurrentLocation();
                if (CurrentPosition != null)
                {
                    await MapView.SetViewpointCenterAsync(CurrentPosition, MapUtils.DEFAULT_MAP_SCALE);

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            if (CurrentPosition != null)
            {
                try
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(new Location(CurrentPosition.Y, CurrentPosition.X));

                    if (placemarks != null && placemarks.Any())
                    {
                        currentAddress = SetAddress(placemarks.FirstOrDefault());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return currentAddress; ;
        }

        private string SetAddress(Placemark placemark)
        {
            string address = "";

            if (placemark != null)
            {
                if (!string.IsNullOrEmpty(placemark.FeatureName))
                {
                    address = placemark.FeatureName + " ";
                }
                if (!string.IsNullOrEmpty(placemark.Thoroughfare))
                {
                    address += placemark.Thoroughfare + " ";
                }

                if (!string.IsNullOrEmpty(placemark.Locality))
                {
                    address += placemark.Locality + " ";
                }

                if (!string.IsNullOrEmpty(placemark.PostalCode))
                {
                    address += placemark.PostalCode;
                }
            }
            return address;
        }

    }

    
}
