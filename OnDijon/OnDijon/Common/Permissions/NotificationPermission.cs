using System;
using System.Threading.Tasks;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace OnDijon.Common.Permissions
{
    public class NotificationPermission : BasePermission
    {
        private readonly IPermissionService _permissionService;

        public NotificationPermission()
        {
	        _permissionService = App.Current.Container.Resolve<IPermissionService>();
        }

        public override Task<PermissionStatus> CheckStatusAsync()
        {
            PermissionStatus permissionStatus = _permissionService.CheckNotificationPermission();
            return Task.FromResult(permissionStatus);
        }

        public override void EnsureDeclared()
        {
            throw new NotImplementedException();
        }

        public override Task<PermissionStatus> RequestAsync()
        {
            throw new NotImplementedException();
        }

        public override bool ShouldShowRationale()
        {
            throw new NotImplementedException();
        }
    }
}
