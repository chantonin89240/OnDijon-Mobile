using OnDijon.Common.Permissions;
using OnDijon.iOS.Services;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OnDijon.iOS.Services
{
    public class PermissionService : IPermissionService
    {
        public PermissionService()
        {
        }

        public PermissionStatus CheckNotificationPermission()
        {
            if (UIApplication.SharedApplication.CurrentUserNotificationSettings.Types != UIUserNotificationType.None)
            {
                return PermissionStatus.Granted;
            }
            else
            {
                return PermissionStatus.Unknown;
            }
        }
    }
}
