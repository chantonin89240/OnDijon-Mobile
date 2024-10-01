using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities;
using OnDijon.Modules.Notifications.Services.Interfaces;
using System.Windows.Input;
using OnDijon.Common.Extensions;
using OnDijon.Modules.Notifications.Entities.Response;
using Prism.Commands;
using Prism.Navigation;

namespace OnDijon.Common.ViewModels
{
    public class TopBarViewModel : BaseViewModel
    {
        private readonly ISession _session;
        private readonly INotificationService _notificationService;

        private int _notificationCount;
        public int NotificationCount
        {
            get { return _notificationCount; }
            set
            {
                Set(ref _notificationCount, value);
                RaisePropertyChanged(nameof(NotificationVisibility));
            }
        }

        public bool NotificationVisibility => NotificationCount > 0;

        public bool IsConnected => _session.IsConnected();

        public ICommand GoToDashboardCommand { get; private set; }

        public ICommand CloseMenuCommand { get; private set; }

        public ICommand OpenMenuCommand { get; private set; }

        public ICommand OpenNotificationCommand { get; private set; }

        public ICommand ExitAddedCommand { get; set; }

        public TopBarViewModel(INavigationService navigationService,
                               ITranslationService translationService,
                               IPopupService popupService,
                               ISession session,
                               INotificationService notificationService,
                               ILoggerService loggerService) : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
            _notificationService = notificationService;
            InitializeCommands();
        }

        internal void InitializeCommands()
        {
            OpenMenuCommand = new DelegateCommand(() => { NavigationService.NavigateTo(Locator.MenuView); });

            OpenNotificationCommand = new DelegateCommand(() =>
            {
                if (IsConnected)
                    NavigateTo(Locator.NotificationsHistoryView);
                else
                    NavigateTo(Locator.LoginPage);
            });

            CloseMenuCommand = new DelegateCommand(() =>
            {
                LaunchExitCommand();
                NavigationService.GoBackAsync();
            });

            GoToDashboardCommand = new DelegateCommand(() =>
            {
                LaunchExitCommand();
                NavigationService.GoBackToPageKey(Locator.DashboardView);
            });
        }

        public void LaunchExitCommand()
        {
            if (ExitAddedCommand != null)
            {
                ExitAddedCommand.Execute(null);
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
            ExitAddedCommand = null;
        }

        public void GetNotificationCount()
        {
            CallApi(async () =>
            {
                NotificationCountResponse response = await _notificationService.GetNotificationCount();
                ManageApiResponses(response, new DefaultCallbackManager<NotificationCountResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        NotificationCount = res.NotificationCount;
                    }
                });
            });
        }

    }
}
