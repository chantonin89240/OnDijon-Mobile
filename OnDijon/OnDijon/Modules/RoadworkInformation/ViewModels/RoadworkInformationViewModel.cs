using Mapsui.UI.Forms;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Entities;
using OnDijon.Common.Entities.Model;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.RoadworkInformation.CustomComponent;
using OnDijon.Modules.RoadworkInformation.Entities;
using OnDijon.Modules.RoadworkInformation.Entities.Models;
using OnDijon.Modules.RoadworkInformation.Entities.Responses;
using OnDijon.Modules.RoadworkInformation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ElementTypeEnum = OnDijon.Modules.RoadworkInformation.CustomComponent.ElementTypeEnum;

namespace OnDijon.Modules.RoadworkInformation.ViewModels
{
    public class RoadworkInformationViewModel : BaseViewModel
    {
        readonly ISession _session;
        readonly IRoadworkInfoService _RoadworkInfoService;

        #region Commands
        public ICommand ViewDetailCommand { get; set; }
        public ICommand SelectRoadworkOnTapCommand { get; set; }
        public ICommand SelectNewestWorkCommand { get; set; }
        public ICommand SelectCurrentWorkCommand { get; set; }
        public ICommand SelectIncomingWorkCommand { get; set; }
        public ICommand OnCloseCommand { get; set; }
        #endregion

        #region Properties
        private bool _DisplayMap;
        public bool DisplayMap
        {
            get { return !DoDisplayList; }
        }

        private bool _DisplayDetail;
        public bool DisplayDetail
        {
            get { return DoDisplayList; }
        }

        private bool _doDisplayList;
        public bool DoDisplayList
        {
            get => _doDisplayList;
            set
            {
                Set(ref _doDisplayList, value);
                RaisePropertyChanged(nameof(DisplayMap));
                RaisePropertyChanged(nameof(DisplayDetail));
            }
        }

        private List<RoadworkInfoModel> _RoadworkList;
        public List<RoadworkInfoModel> RoadworkList
        {
            get { return _RoadworkList; }
            set { Set(ref _RoadworkList, value); }
        }

        private List<RoadworkInfoModel> _FilteredRoadworkList;
        public List<RoadworkInfoModel> FilteredRoadworkList
        {
            get { return _FilteredRoadworkList; }
            set { Set(ref _FilteredRoadworkList, value); }
        }

        private ObservableCollection<Pin> _RoadworkPinsList;
        public ObservableCollection<Pin> RoadworkPinsList
        {
            get { return _RoadworkPinsList; }
            set
            {
                Set(ref _RoadworkPinsList, value);
            }
        }

        private AddressModel _MyHomeLocation;
        public AddressModel MyHomeLocation
        {
            get { return _MyHomeLocation; }
            set { Set(ref _MyHomeLocation, value); }
        }

        private AddressModel _MyWorkLocation;
        public AddressModel MyWorkLocation
        {
            get { return _MyWorkLocation; }
            set { Set(ref _MyWorkLocation, value); }
        }

        private Pin _SelectedPin;
        public Pin SelectedPin
        {
            get { return _SelectedPin; }
            set { Set(ref _SelectedPin, value); }
        }

        private RoadworkInfoModel _SelectedRoadwork;
        public RoadworkInfoModel SelectedRoadwork
        {
            get { return _SelectedRoadwork; }
            set { Set(ref _SelectedRoadwork, value); }
        }

        private bool _DisplayRoadworkDetail;
        public bool DisplayRoadworkDetail
        {
            get { return _DisplayRoadworkDetail; }
            set { Set(ref _DisplayRoadworkDetail, value); }
        }

        private string _RoadworkCount;
        public string RoadworkCount
        {
            get { return _RoadworkCount; }
            set { Set(ref _RoadworkCount, value); }
        }

        private bool _currentWorkSelected = true;
        public bool CurrentWorkSelected
        {
            get { return _currentWorkSelected; }
            set { Set(ref _currentWorkSelected, value); }
        }

        private bool _newestWorkSelected = false;
        public bool NewestWorkSelected
        {
            get { return _newestWorkSelected; }
            set { Set(ref _newestWorkSelected, value); }
        }

        private bool _incomingWorkSelected = false;
        public bool IncomingWorkSelected
        {
            get { return _incomingWorkSelected; }
            set { Set(ref _incomingWorkSelected, value); }
        }
        #endregion

        #region RoadworkDetailViewModel => RoadworkDetailViewModel

        private RoadworkDetailViewModel _roadworkDetailViewModel;

        public RoadworkDetailViewModel RoadworkDetailViewModel
        {
            get => _roadworkDetailViewModel;
            set => SetProperty(ref _roadworkDetailViewModel, value);
        }

        #endregion

        public RoadworkInformationViewModel(INavigationService navigationService,
                                            ITranslationService translationService,
                                            IPopupService popupService,
                                            ISession session,
                                            IRoadworkInfoService roadworkInfoService,
                                            ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _RoadworkInfoService = roadworkInfoService;
            RoadworkPinsList = new ObservableCollection<Pin>();
            RoadworkList = new List<RoadworkInfoModel>();
            ViewDetailCommand = new Command(ViewDetail);
            SelectRoadworkOnTapCommand = new DelegateCommand<RoadworkInfoModel>(SelectRoadworkOnTap);
            SelectCurrentWorkCommand = new Command(SelectCurrentWork);
            SelectIncomingWorkCommand = new Command(SelectIncomingWork);
            SelectNewestWorkCommand = new Command(SelectNewestWork);
            OnCloseCommand = new Command(Close);
            RoadworkDetailViewModel = App.Locator.GetInstance<RoadworkDetailViewModel>();
            RoadworkDetailViewModel.ParentRoadworkInformationViewModel = this;
        } 

        #region OnAppearing & Cleanup
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            RoadworkList = new List<RoadworkInfoModel>();
            FilteredRoadworkList = new List<RoadworkInfoModel>();
            RoadworkPinsList = new ObservableCollection<Pin>();
            GetRoadworkInfoList();
        }

        public override void Cleanup()
        {
            base.Cleanup();
            CleanBeforeClose();
        }

        private void CleanBeforeClose()
        {
            DoDisplayList = false;
            RoadworkList = new List<RoadworkInfoModel>();
            FilteredRoadworkList = new List<RoadworkInfoModel>();
            RoadworkPinsList = new ObservableCollection<Pin>();
            MyHomeLocation = null;
            MyWorkLocation = null;
            SelectedPin = null;
            SelectedRoadwork = null;
            DisplayRoadworkDetail = false;
            RoadworkCount = string.Empty;
        }
        #endregion



        public void GetRoadworkInfoList()
        {
            CallApi(async () =>
            {
                RoadworkInfoResponse response = await _RoadworkInfoService.GetRoadworks(_session.Profile?.Guid, ElementTypeEnum.InfosTravaux.ToString());
                ManageApiResponses(response, new DefaultCallbackManager<RoadworkInfoResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.RoadworkList.Any())
                        {
                            RoadworkList = res.RoadworkList.OrderByDescending(RoadworkList => RoadworkList.X).ToList();

                        }
                        SelectCurrentWork();
                        GetCurrentWorkCount();
                    }
                });
            });
        }
        private void GetCurrentWorkCount()
        {
            var tempCount = 0;
            foreach (var item in RoadworkList)
            {
                if (item.State != WorkStates.incoming.ToString() && item.State != WorkStates.scheduled.ToString())
                {
                    tempCount++;
                }
            }
            RoadworkCount = tempCount.ToString();
        }

        public void SelectRoadwork(string idRoadwork)
        {
            foreach (var item in RoadworkList)
            {
                if (item.EditId == idRoadwork)
                {
                    SelectedRoadwork = item;
                }
            }
        }

        public void GetRoadworkPins()
        {
            ObservableCollection<Pin> TempList = new ObservableCollection<Pin>();
            string pathPin = "";

            foreach (var item in FilteredRoadworkList)
            {

                switch ((WorkStates)Enum.Parse(typeof(WorkStates), item.State))
                {
                    case WorkStates.current:
                        {
                            pathPin = "OnDijon.Assets.traffic_cone_current.png";
                            break;
                        }
                    case WorkStates.newest:
                        {
                            pathPin = "OnDijon.Assets.traffic_cone_newest.png";
                            break;
                        }
                    case WorkStates.incoming:
                        {
                            pathPin = "OnDijon.Assets.traffic_cone_incoming.png";
                            break;
                        }
                    case WorkStates.scheduled:
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pathPin);
                {
                    if (stream == null) throw new Exception($"Could not find EmbeddedResource {pathPin}");
                    var icon = stream.ToBytes();
                    TempList.Add(new OSMPin()
                    {
                        Position = new Position(item.X, item.Y),
                        //Necessaire à la création de l'épingle
                        Label = item.Title,
                        Type = PinType.Icon,
                        EditId = item.EditId,
                        ObjectType = item.ObjectType,
                        //Necessaire à la création de l'épingle
                        Address = "string qui n'est pas affichée",
                        Scale = 1f,
                        Area = item.Area,
                        Icon = icon,
                        Anchor = new Point(0, 24)


                    });
                }
            }
            RoadworkPinsList = TempList;
        }

        public void ViewDetail()
        {
            RoadworkDetailViewModel.RoadworkDetail = SelectedRoadwork;
            DisplayRoadworkDetail = true;
        }

        public void SelectRoadworkOnTap(RoadworkInfoModel selectedRoadwork)
        {
            RoadworkDetailViewModel.RoadworkDetail = selectedRoadwork;
            DisplayRoadworkDetail = true;
        }

        public void SelectNewestWork()
        {
            CurrentWorkSelected = false;
            NewestWorkSelected = true;
            IncomingWorkSelected = false;
            var tempList = new List<RoadworkInfoModel>();
            foreach (var item in RoadworkList)
            {
                if (item.State == WorkStates.newest.ToString())
                {
                    tempList.Add(item);
                }
            }
            FilteredRoadworkList = tempList;
            GetRoadworkPins();
        }
        public void SelectCurrentWork()
        {
            CurrentWorkSelected = true;
            NewestWorkSelected = false;
            IncomingWorkSelected = false;
            var tempList = new List<RoadworkInfoModel>();
            foreach (var item in RoadworkList)
            {
                if (item.State == WorkStates.newest.ToString() || item.State == WorkStates.current.ToString())
                {
                    tempList.Add(item);
                }
            }
            FilteredRoadworkList = tempList;
            GetRoadworkPins();
        }
        public void SelectIncomingWork()
        {
            CurrentWorkSelected = false;
            NewestWorkSelected = false;
            IncomingWorkSelected = true;
            var tempList = new List<RoadworkInfoModel>();
            foreach (var item in RoadworkList)
            {
                if (item.State == WorkStates.incoming.ToString())
                {
                    tempList.Add(item);
                }
            }
            FilteredRoadworkList = tempList;
            GetRoadworkPins();
        }
        public void Close()
        {
            NavigationService.GoBackAsync();
        }
    }
}
