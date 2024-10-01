using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities;
using OnDijon.Common.Utils;
using OnDijon.Modules.Demands.Entities.Models;
using OnDijon.Modules.Demands.Entities.Responses;
using OnDijon.Modules.Demands.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Extensions;
using Prism.Navigation;
using Xamarin.Forms;

namespace OnDijon.Modules.Demands.ViewsModels
{
    public class DemandListViewModel : BaseViewModel
    {
        readonly ISession _session;
        readonly IDemandService _DemandService;

        private ObservableCollection<DemandModel> _demandListFiltered;
        public ObservableCollection<DemandModel> DemandListFiltered { get => _demandListFiltered; set => Set(ref _demandListFiltered, value); }

        private DemandModel _selectedItem;
        public DemandModel SelectedItem { get => _selectedItem; set => Set(ref (_selectedItem), value); }


        private bool _demandLayoutIsVisible;
        public bool DemandLayoutIsVisible { get => _demandLayoutIsVisible; set => Set(ref _demandLayoutIsVisible, value); }

        public ICommand LoadItemsCommand { get; set; }
        public ICommand ActionButtonCommand { get; set; }

        public DemandListViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IPopupService popupService,
                                   ISession session,
                                   IDemandService demandService,
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _DemandService = demandService;

            DemandListFiltered = new ObservableCollection<DemandModel>();
            LoadItemsCommand = new Command(GetDemandList);
            ActionButtonCommand = new Command<ActionModel>((action) =>
            {
	            var navParams = new NavigationParameters();
                navParams.Add(Constants.CancelBookingActionNavigationParameterKey, action);
                navigationService.NavigateTo(Locator.CancelBookingPage, navigationParameters: navParams);
            });
        }


        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            GetDemandList();
        }

        public void GetDemandList()
        {
            CallApi(async () =>
            {
                DemandListResponse response = await _DemandService.GetDemands(Constants.DIJON_CITYCONTEXT, _session.Profile.Guid);
                ManageApiResponses(response, new DefaultCallbackManager<DemandListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.DemandList.Any())
                        {
                            DemandListFiltered = new ObservableCollection<DemandModel>(res.DemandList.Where(d => d.Category == "En cours"));
                            DemandLayoutIsVisible = false;
                        }
                        else
                        {
                            DemandLayoutIsVisible = true;
                        }
                    },
                });
            });
        }

        public override void Cleanup()
        {
            base.Cleanup();
            DemandListFiltered.Clear();
        }

    }
}
