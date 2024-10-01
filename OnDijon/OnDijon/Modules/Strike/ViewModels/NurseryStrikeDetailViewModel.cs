using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.ViewModels;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Entities;
using OnDijon.Modules.Strike.Entities.Model;
using OnDijon.Modules.Strike.Services.Interfaces;
using OnDijon.Modules.Strike.Entities.Responses;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using OnDijon.Modules.Account.Services.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using OnDijon.Common.Utils;

namespace OnDijon.Modules.Strike.ViewModels
{
    public class NurseryStrikeDetailViewModel : BaseViewModel
    {
        
        readonly ISession _session;
        readonly IListStrikeService _ListStrikeService;

        #region Properties
        private string _IdSession;
        public string IdSession
        {
            get
            {
                return _IdSession;
            }
            set
            {
                Set(ref _IdSession, value);
            }
        }

        private NurserySessionStrikeModel _SessionStrike;
        public NurserySessionStrikeModel SessionStrike
        {
            get
            {
                return _SessionStrike;
            }
            set
            {
                Set(ref _SessionStrike, value);
            }
        }


        private List<NurseryStrikeInfoModel> _FilteredSessionStrike;
        public List<NurseryStrikeInfoModel> FilteredSessionStrike
        {
            get
            {
                return _FilteredSessionStrike;
            }
            set
            {
                Set(ref _FilteredSessionStrike, value);
            }
        }

        private string _Filter;
        public string Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                Set(ref _Filter, value);
            }
        }

        private NurseryStrikeInfoModel _SelectedNursery;
        public NurseryStrikeInfoModel SelectedNursery
        {
            get
            {
                return _SelectedNursery;
            }
            set
            {
                Set(ref _SelectedNursery, value);
            }
        }

        private bool _IsNurserySelected;
        public bool IsNurserySelected
        {
            get
            {
                return _IsNurserySelected;
            }
            set
            {
                Set(ref _IsNurserySelected, value);
            }
        }

        
        private bool _IsNurseryListDisplay;
        public bool IsNurseryListDisplay
        {
            get
            {
                return _IsNurseryListDisplay;
            }
            set
            {
                Set(ref _IsNurseryListDisplay, value);
                if (_IsNurseryListDisplay == true)
                {
                    IsNurserySelected = false;
                }
            }
        }
        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                Set(ref _Title, value);
            }
        }
        #endregion

        #region Commands
        public ICommand CloseCommand { get; }

        public ICommand SelectNurseryCommand { get; set; }

        public ICommand UpdatePickerContent { get; set; }
        #endregion

        public void UpdatePicker()
        {
            IsNurseryListDisplay = !onSelection;
            if (Filter != null)
            {
                FilteredSessionStrike = SessionStrike.Strikes.Where(item => item.Name.ToLower().Contains(Filter.ToLower())).ToList();
            }
            else
            {
                FilteredSessionStrike = SessionStrike.Strikes;
            }
            //Tri de la liste par ordre alphabétique
            FilteredSessionStrike = FilteredSessionStrike.OrderBy(x => x.Name).ToList();
        }

        private bool onSelection = false;
        private void GetSelectedNursery(NurseryStrikeInfoModel nurseryStrike)
        {
            onSelection = true;
            SelectedNursery = nurseryStrike;
            IsNurseryListDisplay = false;
            IsNurserySelected = true;

            Filter = null;
            onSelection = false;

        }

        public void GetNurseryStrike()
        {
            CallApi(async () =>
            {

                NurserySessionStrikeResponse resp = await _ListStrikeService.GetNurserySessionStrike(IdSession);
                ManageApiResponses(resp, new DefaultCallbackManager<NurserySessionStrikeResponse>(PopupService)
                {
                    OnSuccess = (res) =>

                    {
                        SessionStrike = res.NurserySessionStrike;
                        Title = "Grève du " + SessionStrike.DateStrike.ToString("dd/MM/yyyy");
                    }
                });
            });
        }

        #region OnApprearing & Cleanup
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            if(parameters.TryGetValue(Constants.ServiceNavigationKey,out string idSession))
                IdSession= idSession;
            GetNurseryStrike();
        }

        
        public override void Cleanup()
        {
            base.Cleanup();
            Filter = null;
            FilteredSessionStrike.Clear();
            SessionStrike.Strikes.Clear();
            SessionStrike.DateStrike = default;
            SessionStrike.EditId = null;
            //IdSession = null;
            IsNurseryListDisplay = false;
            IsNurserySelected = false;
            SelectedNursery = null;
            Title = null;
        }

        #endregion

        public NurseryStrikeDetailViewModel(INavigationService navigationService,
                                            ITranslationService translationService,
                                            IPopupService popupService,
                                            ISession session,
                                            IListStrikeService ListStrikeService, 
                                            ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _ListStrikeService = ListStrikeService;
            FilteredSessionStrike = new List<NurseryStrikeInfoModel>();
            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            UpdatePickerContent = new Command(UpdatePicker);
            SelectNurseryCommand = new DelegateCommand<NurseryStrikeInfoModel>(GetSelectedNursery);
        }


    }
}
