using Xamarin.Essentials;

namespace OnDijon.Common.Permissions
{
    public interface IPermissionService
    {
        PermissionStatus CheckNotificationPermission();
    }
}
