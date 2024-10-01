using OnDijon.Common.Entities;
using OnDijon.Common.Services.Interfaces;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Notifications.Entities.Models;
using OnDijon.Modules.Notifications.Entities.Response;
using OnDijon.Modules.Notifications.Services.Interfaces;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.Notifications.ViewModels
{
    public class NotificationsHistoryViewModel : BaseViewModel
    {
        private readonly INotificationService _notificationService;

        public ICommand GetNotificationsCommand { get; }

        public ICommand RelayNotificationCommand { get; }

        private IList<NotificationModel> _notifications;
        public IList<NotificationModel> Notifications
        {
            get { return _notifications; }
            set { Set(ref _notifications, value); }
        }

        private static bool _canExecute = true;

        public NotificationsHistoryViewModel(INavigationService navigationService, ITranslationService translationService, IPopupService popupService, INotificationService notificationService, ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _notificationService = notificationService;

            GetNotificationsCommand = new Command(() => GetNotifications());

            RelayNotificationCommand = new Command<NotificationModel>(notif => RelayNotification(notif), notif => _canExecute);
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
	        await base.OnNavigatedToAsync(parameters);
            GetNotifications();
        }

        private void GetNotifications()
        {
            CallApi(async () =>
            {
                NotificationListResponse response = await _notificationService.GetNotifications();
                ManageApiResponses(response, new DefaultCallbackManager<NotificationListResponse>(PopupService)
                {
                    OnSuccess = (res) =>
                    {
                        if (res.Notifications.Any())
                        {
                            Notifications = new List<NotificationModel>(res.Notifications).ToList();
                        }
                    }
                });
            });
        }

        private void RelayNotification(NotificationModel notification)
        {
            _canExecute = false;
            //go to target service page
            var notificationData = new Dictionary<string, object>
            {
                { "notificationId", notification.Id },
                { "serviceId", notification.ServiceId },
                { "itemId", notification.ItemId },
                { "read", notification.IsRead }
            };
            _notificationService.Relay(notificationData);
            _canExecute = true;
        }
    }
}
