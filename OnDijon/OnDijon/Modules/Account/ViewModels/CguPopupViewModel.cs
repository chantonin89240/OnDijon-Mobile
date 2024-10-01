using OnDijon.Common.Entities;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Modules.Account.Entities.Response;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Splat;
using Xamarin.Forms;

namespace OnDijon.Modules.Account.ViewModels
{
    public class CguPopupViewModel : BaseViewModel
    {
        readonly ICguService _cguService;

        private string _cguHtml;

        private IPopupViewSettings _settings;

        private bool _displayBottomButtons = false;
        public bool DisplayBottomButtons
        {
            get { return _displayBottomButtons; }
            set
            {
                _displayBottomButtons = value;
                RaisePropertyChanged(nameof(DisplayBottomButtons));
            }
        }


        private bool _closeWhenBackgroundIsClicked = true;
        public bool CloseWhenBackgroundIsClicked
        {
            get { return _closeWhenBackgroundIsClicked; }
            set
            {
                _closeWhenBackgroundIsClicked = value;
                RaisePropertyChanged(nameof(CloseWhenBackgroundIsClicked));
            }
        }


        public ICommand AcceptCommand { get; }
        public ICommand DeclineCommand { get; }
        public ICommand GoBackCommand { get; }


        public CguPopupViewModel(INavigationService navigationService,
                                 ITranslationService translationService,
                                 IPopupService popupService,
                                 ICguService cguService,
                                 ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _cguService = cguService;

            AcceptCommand = new Command(() => OnAccept());
            DeclineCommand = new Command(() => OnDecline());
            GoBackCommand = new DelegateCommand(async () => await PopupNavigation.Instance.PopAsync());
        }

        private async void OnDecline()
        {
            await PopupNavigation.Instance.PopAsync();
            _settings?.OnDeclineAction?.Invoke();
        }

        private async void OnAccept()
        {
            await PopupNavigation.Instance.PopAsync();
            _settings?.OnAcceptAction?.Invoke();
        }

        private HtmlWebViewSource _webViewSource;
        public HtmlWebViewSource WebViewSource
        {
            get { return _webViewSource; }
            set
            {
                Set(ref _webViewSource, value);
            }
        }

        public void QueryCgu()
        {
            if (_cguHtml == null)
            {
                CallApi(async () =>
                {
                    CguResponse response = await _cguService.GetCgu();
                    ManageApiResponses(response, new DefaultCallbackManager<CguResponse>(PopupService)
                    {
                        OnSuccess = (res) =>
                        {
                            _cguHtml = res.Cgu.Html;
                            WebViewSource = new HtmlWebViewSource { Html = _cguHtml };
                        }
                    });
                });
            }
        }
        public void UpdateSettings(IPopupViewSettings settings)
        {
            _settings = settings;
            DisplayBottomButtons = settings.DisplayBottomButtons;
            CloseWhenBackgroundIsClicked = settings.CloseWhenBackgroundIsClicked;
        }
    }
}