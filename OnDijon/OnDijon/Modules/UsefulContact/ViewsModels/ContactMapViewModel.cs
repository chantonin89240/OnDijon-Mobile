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

namespace OnDijon.Modules.UsefulContact.ViewsModels
{
    public class ContactMapViewModel : BaseViewModel
    {
        readonly IContactDomainService _ContactDomainService;
        readonly IContactService _ContactService;
        private readonly IGeolocationService _geolocationService;
        

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
            private set { Set(ref _map, value); }
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

        public Command LoadItemsCommand { get; set; }
        public Command SearchCommand { get; set; }
        public Command ShowDomainOnMapCommand { get; set; }
        public Command ViewContactCommand { get; set; }

        #endregion

        public ContactMapViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IPopupService popupService,
                                   IContactDomainService ContactDomainService,
                                   IContactService ContactService,
                                   IGeolocationService geolocationService,
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _ContactDomainService = ContactDomainService;
            _ContactService = ContactService;
            _geolocationService = geolocationService;
            GetCurrentLocationCommand = new Command(async () => CurrentPosition = await GetCurrentLocation());
            DomainList = new ObservableCollection<ContactDomainModel>();
            ContactList = new ObservableCollection<ContactModel>();
            SearchCommand = new Command(GetContactList);
            ViewContactCommand = new Command(ViewContact);
            ContactDetail = App.Locator.GetInstance<ContactDetailViewModel>();
            ContactDetail.CloseContactDetailDisplayAction = () => IsContactDetailDisplay = false;
            WorkInfosDetail = App.Locator.GetInstance<WorkInfosViewModel>();
            WorkInfosDetail.CloseWorkInfosDetailDisplayAction = () => IsWorkInfosDetailDisplay = false;

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
            if(DomainSelected != null || (!string.IsNullOrEmpty(Recherche) && Recherche.Length > 2))
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

        public void ViewContactPopup()
        {
            if (ContactSelected.ElementType == "infosTravaux")
            {
                PopupService.Show(new PopupInfoContactView(this));
            }
            else
            {
                CallApi(async () =>
                {
                    OpeningTimeResponse response = await _ContactService.GetOpeningTime(ContactSelected.EditId);
                    ManageApiResponses(response, new DefaultCallbackManager<OpeningTimeResponse>(PopupService)
                    {
                        OnSuccess = (res) => {
                            ContactSelected.ContactInfos.OpeningTime = res.OpeningTime;
                            PopupService.Show(new PopupInfoContactView(this));
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
    }
}
