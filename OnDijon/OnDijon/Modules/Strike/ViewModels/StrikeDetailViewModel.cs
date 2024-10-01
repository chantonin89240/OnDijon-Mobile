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
    public class StrikeDetailViewModel : BaseViewModel
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

        private SessionStrikeModel _SessionStrike;
        public SessionStrikeModel SessionStrike
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


        private List<SchoolStrikeInfoModel> _FilteredSessionStrike;
        public List<SchoolStrikeInfoModel> FilteredSessionStrike
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

        private SchoolStrikeInfoModel _SelectedSchool;
        public SchoolStrikeInfoModel SelectedSchool
        {
            get
            {
                return _SelectedSchool;
            }
            set
            {
                Set(ref _SelectedSchool, value);
            }
        }

        private bool _IsSchoolSelected;
        public bool IsSchoolSelected
        {
            get
            {
                return _IsSchoolSelected;
            }
            set
            {
                Set(ref _IsSchoolSelected, value);
            }
        }

        private bool _IsSchoolListDisplay;
        public bool IsSchoolListDisplay
        {
            get
            {
                return _IsSchoolListDisplay;
            }
            set
            {
                Set(ref _IsSchoolListDisplay, value);
                if (_IsSchoolListDisplay == true)
                {
                    IsSchoolSelected = false;
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

        public ICommand SelectSchoolCommand { get; set; }

        public ICommand UpdatePickerContent { get; set; }
        #endregion

        public void UpdatePicker()
        {          
            IsSchoolListDisplay = !onSelection;                   
            if (Filter != null)
            {
                FilteredSessionStrike = SessionStrike.Strikes.Where(item => item.Name.ToLower().Contains(Filter.ToLower())).ToList();
            }
            else
            {
                FilteredSessionStrike = SessionStrike.Strikes;
            }
        }

        private bool onSelection = false;
        private void GetSelectedSchool(SchoolStrikeInfoModel schoolStrike)
        {
            onSelection = true;
            SelectedSchool = schoolStrike;
            IsSchoolListDisplay = false;
            IsSchoolSelected = true;
           
            Filter = null;
            onSelection = false;

        }

        public void GetSessionStrike()
        {
            CallApi(async () =>
            {
                SessionStrikeResponse resp = await _ListStrikeService.GetSessionStrike(IdSession);
                ManageApiResponses(resp, new DefaultCallbackManager<SessionStrikeResponse>(PopupService)
                {
                    OnSuccess = (res) =>

                    {
                        SessionStrike = res.SessionStrike;
                        Title = "Grève du " + SessionStrike.DateStrike.ToString("dd/MM/yyyy");
                        SessionStrike.Strikes = SessionStrike.Strikes.OrderBy(x => x.Name).ToList();
                        FilteredSessionStrike = SessionStrike.Strikes;
                    }
                });
            });
        }

        #region OnAppearing & Cleanup
        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            if (parameters.TryGetValue(Constants.ServiceNavigationKey, out string idSession))
                IdSession = idSession;
            GetSessionStrike();

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
            IsSchoolListDisplay = false;
            IsSchoolSelected = false;
            SelectedSchool = new SchoolStrikeInfoModel();
            Title = null;
        }
        #endregion


        public StrikeDetailViewModel(INavigationService navigationService,
                                     ITranslationService translationService,
                                     IPopupService popupService,
                                     ISession session,
                                     IListStrikeService ListStrikeService,
                                     ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _ListStrikeService = ListStrikeService;
            FilteredSessionStrike = new List<SchoolStrikeInfoModel>();
            CloseCommand = new Command(() => NavigationService.GoBackAsync());
            UpdatePickerContent = new Command(UpdatePicker);
            SelectSchoolCommand = new DelegateCommand<SchoolStrikeInfoModel>(GetSelectedSchool);
            SelectedSchool = new SchoolStrikeInfoModel();

        }
    }
}
