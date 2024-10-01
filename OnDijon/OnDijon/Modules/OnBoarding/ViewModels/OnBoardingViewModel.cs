using OnDijon.Common.Entities;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Common.Views.Popup;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using OnDijon.Common.Extensions;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Modules.OnBoarding.ViewModels
{
    public class OnBoardingViewModel : BaseViewModel
    {
        #region Variables
        private readonly ISession _session;
        private readonly IVersionService _versionService;
        private readonly ICacheService _cacheService;


        private bool _showConnexion;
        public bool ShowConnexion
        {
            get { return _showConnexion; }
            set { Set(ref _showConnexion, value); }
        }

        private ObservableCollection<AppVersionNote> _appVersionNotes;
        public ObservableCollection<AppVersionNote> AppVersionNotes
        {
            get { return _appVersionNotes; }
            set
            {
                Set(ref _appVersionNotes, value);
            }
        }

        private ObservableCollection<Slide> _slides;
        public ObservableCollection<Slide> Slides
        {
            get { return _slides; }
            set
            {
                Set(ref _slides, value);
            }
        }

        public ICommand OnBoardingVerifCommand { get; set; }

        #endregion

        public OnBoardingViewModel(INavigationService navigationService,
                                   ITranslationService translationService,
                                   IPopupService popupService,
                                   IVersionService versionService,
                                   ICacheService cacheService,
                                   ISession session,
                                   ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _versionService = versionService;
            _cacheService = cacheService;

            OnBoardingVerifCommand = new DelegateCommand<Slide>(slide => OnBoardingVerif(slide));
            VerifVersion();
        }

       

        private void OnBoardingVerif(Slide slide)
        {
            switch (slide.SlideId)
            {
                case 5:
                    NavigationService.NavigateTo(Locator.DashboardView);
                    break;
                case 4:
                    NavigationService.NavigateTo(Locator.LoginPage);
                    break;
                default:
                    break;
            }
        }

        public void VerifVersion()
        {
            CallApi(async () =>
                {
                    var lastVersion = await _cacheService.Get<string>(Constants.LastVersionLaunched, CacheType.Default);
                    AppVersionStateResponse response = await _versionService.GetAppVersionState(lastVersion);

                    ManageApiResponses(response, new CallbackManager<AppVersionStateResponse>
                    {
                        OnSuccess = (res) =>
                        {
                            switch (res.Versionning.State)
                            {
                                case "Obsolète":
                                    PopupVersionObsoleteView popupObsolete = new PopupVersionObsoleteView()
                                    {
                                        Message = res.Versionning.Message
                                    };
                                    PopupService.Show(popupObsolete);
                                    break;
                                case "Suspendu":
                                    PopupVersionSuspendedView popupSuspended = new PopupVersionSuspendedView()
                                    {
                                        Message = res.Versionning.Message
                                    };
                                    PopupService.Show(popupSuspended);
                                    break;
                                default:
                                    if (_session.IsConnected() && (!res.Versionning.Notes.Any() || lastVersion != null && res.Versionning.Code.Equals(lastVersion)))
                                    {
                                        NavigationService.NavigateAsync(Locator.DashboardView);
                                    }
                                    else
                                    {
                                        _cacheService.Put(Constants.LastVersionLaunched, res.Versionning.Code, CacheType.Default);
                                        AppVersionNotes = new ObservableCollection<AppVersionNote>();
                                        if (res.Versionning.Notes.Any())
                                        {
                                            foreach (var note in res.Versionning.Notes)
                                            {
                                                AppVersionNotes.Add(note);
                                            }
                                        }
                                        GenerateSlides();
                                    }
                                    break;
                            }
                        }
                    });
                });
        }

        private void GenerateSlides()
        {
            Slides = new ObservableCollection<Slide>();
            Slides.Add(new Slide() { SlideId = 1 });
            Slides.Add(new Slide() { SlideId = 2 });
            foreach (var appVersionNote in AppVersionNotes)
            {
                Slides.Add(new Slide() { SlideId = 3, Note = appVersionNote });
            }
            if (!_session.IsConnected())
            {
                Slides.Add(new Slide() { SlideId = 4 });
            }
            else
            {
                Slides.Add(new Slide() { SlideId = 5 });
            }
        }

    }

    public class Slide : BindableObjectBase
    {
        public int SlideId { get; set; }

        public AppVersionNote Note { get; set; }
    }
}
