using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Entities.Dto;
using OnDijon.Common.Entities.Response;
using OnDijon.Modules.Notifications.Entities.Dto;
using OnDijon.Modules.Notifications.Entities.Request;
using OnDijon.Modules.Notifications.Services.Interfaces;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Services.Interfaces;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OnDijon.Common.Extensions;
using OnDijon.Common.ViewModels;
using Xamarin.Forms;
using OnDijon.Modules.Notifications.Entities.Response;
using OnDijon.Modules.Notifications.Entities.Models;
using Prism.Navigation;

namespace OnDijon.Modules.Notifications.Services
{
    public class NotificationService : INotificationService
    {

        private readonly IHttpService _httpService;
        private readonly IUserIdService _userIdService;
        private readonly ISession _session;
        private readonly IAccountService _accountService;

        public NotificationService(INavigationService navigationService, IHttpService httpService, IUserIdService userIdService, ISession session, IAccountService accountService)
        {

            _httpService = httpService;
            _userIdService = userIdService;
            _session = session;
            _accountService = accountService;
        }

        public void Relay(IDictionary<string, object> notificationData)
        {
            if (notificationData.TryGetValue("serviceId", out object serviceId) && serviceId != null)
            {
                var pageKey = GetPageKey(serviceId as string);
                if (pageKey != null)
                {
                    //if the app was launched by the notification, first navigate to dashboard for proper back navigation
                    if (Application.Current?.MainPage.Navigation.NavigationStack.Count == 1)
                    {
                        //App.CurrentNavigationService.NavigateTo(Locator.DashboardView);
                    }

                    try
                    {
                        if (notificationData.TryGetValue("itemId", out object itemId) && !string.IsNullOrEmpty(itemId as string))
                        {
                            Debug.WriteLine($"NotificationService - open page {pageKey} with itemId {itemId}");
                            var parameters = new NavigationParameters { { Constants.NotificationItemNavigationKey, itemId } };
                            //App.CurrentNavigationService.NavigateTo(pageKey, navigationParameters: parameters);
                            App.CurrentNavigationService.NavigateTo(Locator.DashboardView + "/" + pageKey, navigationParameters: parameters);
                        }
                        else
                        {
                            Debug.WriteLine($"NotificationService - open page {pageKey}");
                            //App.CurrentNavigationService.NavigateTo(pageKey);
                            App.CurrentNavigationService.NavigateTo(Locator.DashboardView + "/" + pageKey);
                        }

                        MarkAsReadInternal(notificationData);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Debug.WriteLine("NotificationService - error:");
                        Debug.WriteLine($"    {ex}");
                    }
                }
                else
                {
                    Debug.WriteLine($"NotificationService - no page key defined for service {serviceId}");
                }
            }
        }

        public async Task<NotificationListResponse> GetNotifications()
        {
            NotificationListDto sources = await GetNotificationsAsync();

            NotificationListResponse response = Common.Entities.Utils.Translate<NotificationListResponse, NotificationListDto>(sources);

            if (response.IsSuccessful())
            {
                response.Notifications = sources.NotificationsList.Select(notif => new NotificationModel()
                {
                    Body = notif.Body,
                    DateSent = notif.DateSent,
                    Id = notif.Id,
                    IsRead = notif.IsRead,
                    ItemId = notif.ItemId,
                    ServiceId = notif.ServiceId,
                    Title = notif.Title
                }).ToList();
            }

            return response;
        }

        public async Task<NotificationListDto> GetNotificationsAsync()
        {
            NotificationListDto _notificationsList = null;

            try
            {
                string userId = await _userIdService.GetUserId();
                string escapedToken = Uri.EscapeDataString(userId);
                string url = string.Concat(Constants.API_URL, Constants.NOTIF_SERVICES, Constants.NOTIF_GET_ALL);

                NotificationRequest notifRequest = new NotificationRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ProfileEditId = userId,
                    RegistrationToken = escapedToken
                };

                string json = JsonConvert.SerializeObject(notifRequest);

                _notificationsList = await _httpService.PostAsync<NotificationListDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _notificationsList;
        }

        public async Task<NotificationCountResponse> GetNotificationCount()
        {
            NotificationCountDto sources = await GetNotificationCountAsync();

            NotificationCountResponse response = Common.Entities.Utils.Translate<NotificationCountResponse, NotificationCountDto>(sources);
            if (response.IsSuccessful())
            {
                response.NotificationCount = sources.NotificationsCount;
            }

            return response;

        }

        public async Task<NotificationCountDto> GetNotificationCountAsync()
        {
            NotificationCountDto _notifCount = null;

            try
            {
                string userId = await _userIdService.GetUserId();
                string escapedToken = Uri.EscapeDataString(userId);
                string url = string.Concat(Constants.API_URL, Constants.NOTIF_SERVICES, Constants.NOTIF_GET_COUNT);

                NotificationRequest notifRequest = new NotificationRequest()
                {
                    Key = Constants.ONDIJON_KEY,
                    ProfileEditId = userId,
                    RegistrationToken = escapedToken
                };

                string json = JsonConvert.SerializeObject(notifRequest);

                _notifCount = await _httpService.PostAsync<NotificationCountDto>(new Uri(url), json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }


            return _notifCount;
        }

        public async Task<Response> MarkAsRead(int notificationId, bool isRead = true)
        {
            WsDMDto sources = await MarkAsReadAsync(notificationId, isRead);
            return Common.Entities.Utils.Translate<Response, WsDMDto>(sources);
        }

        public async Task<WsDMDto> MarkAsReadAsync(int notificationId, bool isRead = true)
        {
            WsDMDto _res = new WsDMDto();

            try
            {
                string url = string.Concat(Constants.API_URL, Constants.NOTIF_SERVICES, Constants.NOTIF_MARK_AS_READ);

                NotificationStatusRequest request = new NotificationStatusRequest
                {
                    Key = Constants.ONDIJON_KEY,
                    NotificationId = notificationId,
                    IsRead = isRead,
                    ProfileEditId = _session.Profile.Guid
                };

                string json = JsonConvert.SerializeObject(request);

                _res = await _httpService.PostAsync<WsDMDto>(new Uri(url), json).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }

            return _res;
        }

        private void MarkAsReadInternal(IDictionary<string, object> notificationData)
        {
            if (notificationData.TryGetValue("notificationId", out object notificationId))
            {
                Debug.WriteLine($"NotificationService - mark as read notification with id {notificationId}");
                if (!notificationData.ContainsKey("read") || !(bool)notificationData["read"])
                {
                    Task.Run(async () =>
                    {
                        await MarkAsRead(int.Parse(notificationId.ToString())).ConfigureAwait(false);
                    });
                }
            }
            else
            {
                Debug.WriteLine($"NotificationService - can't mark as read because notificationId is missing");
            }
        }

        private string GetPageKey(string serviceId)
        {
            if (Constants.PageKeyByCodeService.ContainsKey(serviceId))
            {
                if (serviceId == "SIGNALEMENT")
                {
                    return Locator.ReportDetailView;
                }
                return Constants.PageKeyByCodeService[serviceId];
            }
            return null;
        }

        public void InitFirebase()
        {
            //abonnement au topic des notifications générales
            CrossFirebasePushNotification.Current.Subscribe(Constants.FIREBASE_TOPIC);

            //évenements sur notifications
            CrossFirebasePushNotification.Current.OnTokenRefresh -= OnTokenRefresh;
            CrossFirebasePushNotification.Current.OnTokenRefresh += OnTokenRefresh;
            CrossFirebasePushNotification.Current.OnNotificationOpened -= OnNotificationOpened;
            CrossFirebasePushNotification.Current.OnNotificationOpened += OnNotificationOpened;
            CrossFirebasePushNotification.Current.OnNotificationReceived -= OnNotificationReceived;
            CrossFirebasePushNotification.Current.OnNotificationReceived += OnNotificationReceived;
            CrossFirebasePushNotification.Current.OnNotificationError -= OnNotificationError;
            CrossFirebasePushNotification.Current.OnNotificationError += OnNotificationError;
        }

        private async void OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            Debug.WriteLine($"OnTokenRefresh: {e.Token}");
            if (_session.IsConnected())
            {
                var tokenResponse = await _accountService.AddMobileToken(e.Token, _session.Profile);
                Debug.WriteLine("AddMobileToken response: " + tokenResponse);
            }
        }

        private void OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs e)
        {
            Debug.WriteLine($"OnNotificationOpened: {string.Join(Environment.NewLine, e.Data)}");
            Relay(e.Data);
        }

        private void OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            Debug.WriteLine($"OnNotificationReceived: {string.Join(Environment.NewLine, e.Data)}");
            if (!e.Data.ContainsKey("priority") || !e.Data["priority"].Equals("high"))
            {
                e.Data["priority"] = "high";
                CrossFirebasePushNotification.Current.NotificationHandler.OnReceived(e.Data);
            }
        }

        private void OnNotificationError(object source, FirebasePushNotificationErrorEventArgs e)
        {
            Debug.WriteLine($"OnNotificationError: {e.Message}");
            Crashes.TrackError(new Exception("OnNotificationError: " + e.Message));
        }

        public string GetFirebaseToken()
        {
            return CrossFirebasePushNotification.Current.Token;
        }
    }
}
