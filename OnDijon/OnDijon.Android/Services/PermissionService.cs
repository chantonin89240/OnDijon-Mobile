using OnDijon.Common.Permissions;
using OnDijon.Droid.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OnDijon.Droid.Services
{
    public class PermissionService : IPermissionService
    {
        public PermissionService()
        {
        }

        public PermissionStatus CheckNotificationPermission()
        {
            //Sur Android les notifications sont autorisées par défaut
            return PermissionStatus.Granted;
        }
    }
}
