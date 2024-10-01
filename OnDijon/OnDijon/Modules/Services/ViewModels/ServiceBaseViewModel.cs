using Microsoft.AppCenter.Analytics;
using OnDijon.Common.Extensions;
using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Modules.Account.Services.Interfaces;
using OnDijon.Common.Utils;
using OnDijon.Common.Utils.Extensions;
using OnDijon.Common.ViewModels;
using OnDijon.Modules.Services.Entities.Models;
using OnDijon.Common.Services.Interfaces;
using Prism.Navigation;
using System.Threading.Tasks;
using System;

namespace OnDijon.Modules.Services.ViewModels
{
    public class ServiceBaseViewModel : BaseViewModel
    {
        private readonly ISession _session;

        public ServiceBaseViewModel(INavigationService navigationService, ITranslationService translationService, IPopupService popupService, ISession session, ILoggerService loggerService)
            : base(navigationService, translationService, popupService, loggerService)
        {
            _session = session;
        }


        protected async Task NavigateToService(ServiceLayout service, string parameter = null)
        {
            if (!service.IsRequiredConnection || _session.IsConnected())
            {
                if (!IsOffline)
                {
                    if (service.Visibility == Constants.SERVICE_VISIBLITY_MAINTENANCE)
                    {
#if DEBUG || STAGING    
                        Analytics.TrackEvent(Constants.NavigationMenuEvents[service.Code]);
                        if (string.IsNullOrEmpty(parameter))
                        {
                            await NavigationService.NavigateTo(service.GetPageKeyByServiceCode());
                        }
                        else
                        {
                            await NavigationService.NavigateTo(service.GetPageKeyByServiceCode(), navigationParameters: NavigationParametersFactory(parameter, Constants.ServiceNavigationKey));
                        }
#else
                        PopupService.Show(PopupEnum.PopupInfo, "Service indisponible", service.MaintenanceMessage, "Retour");
#endif
                    }
                    else
                    {
                        try
                        {
                            Analytics.TrackEvent(Constants.NavigationMenuEvents[service.Code]);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        if (string.IsNullOrEmpty(parameter))
                        {
                            await NavigationService.NavigateTo(service.GetPageKeyByServiceCode());
                        }
                        else
                        {
                            await NavigationService.NavigateTo(service.GetPageKeyByServiceCode(), navigationParameters: NavigationParametersFactory(parameter, Constants.ServiceNavigationKey));
                        }
                    }
                }
                else
                {
                    PopupService.Show(PopupEnum.PopupError, "Action impossible", "Pas de connexion internet", "Retour");
                }
            }
            else
            {
                await NavigationService.NavigateTo(Locator.LoginPage); 
            }
        }
    }
}
