using AsyncAwaitBestPractices.MVVM;
using Newtonsoft.Json;
using OnDijon.Common.Entities;
using OnDijon.Common.Extensions;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Utils;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.UsefulContact.Entities.Models;
using OnDijon.Modules.UsefulContact.Entities.Responses;
using OnDijon.Modules.UsefulContact.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.UsefulContact.ViewsModels
{
    public class ServiceListViewModel : BaseViewModel
    {
        readonly IServiceService _ServiceService;

        private ObservableCollection<ServiceModel> _ServiceList { get; set; }
        public ObservableCollection<ServiceModel> ServiceList
        {
            get
            {
                return _ServiceList;
            }
            set
            {
                _ServiceList = value;
                RaisePropertyChanged(nameof(ServiceList));
            }
        }
        private ServiceModel _ServiceSelected { get; set; }
        public ServiceModel ServiceSelected
        {
            get
            {
                return _ServiceSelected;
            }
            set
            {
                _ServiceSelected = value;
                RaisePropertyChanged(nameof(ServiceSelected));
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { Set(ref _isRefreshing, value); }
        }

        public Command LoadItemsCommand { get; set; }

        public ICommand SelectServiceCommand { get; set; }

        public ServiceListViewModel(INavigationService navigationService,
                                    ITranslationService translationService,
                                    IPopupService popupService,
                                    IServiceService ServiceService,
                                    ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _ServiceService = ServiceService;
            ServiceList = new ObservableCollection<ServiceModel>();
            SelectServiceCommand = new AsyncCommand<ServiceModel>(GetServiceDetail);
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await base.OnNavigatedToAsync(parameters);
            GetServiceList();
        }

        public void GetServiceList()
        {
            CallApi(async () =>
            {
                ServiceListResponse response = await _ServiceService.GetServices();
                ManageApiResponses(response, new DefaultCallbackManager<ServiceListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.ServiceList.Any())
                        {
                            ServiceList = new ObservableCollection<ServiceModel>(res.ServiceList);
                        }
                    }
                });
            });
        }

        private async Task GetServiceDetail(ServiceModel service)
        {
            ServiceSelected = service;
            INavigationParameters param = new NavigationParameters
            {
                { Constants.ServiceModelNavigationKey, JsonConvert.SerializeObject(ServiceSelected) }
            };
            await NavigationService.NavigateAsync(Locator.ServiceDetailPage,param);

        }

    }
}
