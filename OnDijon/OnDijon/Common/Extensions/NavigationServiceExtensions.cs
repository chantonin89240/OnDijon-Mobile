using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using OnDijon.Common.Services.Interfaces;
using Prism.Ioc;
using Prism.Navigation;

namespace OnDijon.Common.Extensions
{
    public static class NavigationServiceExtensions
    {
        /// <summary>
        /// Pop pages from navigation stack to page <paramref name="pageKey"/>
        /// </summary>
        /// <param name="pageKey"></param>
        public static async Task<INavigationResult> GoBackToPageKey(this INavigationService navigationService, string pageKey, INavigationParameters navigationParameters = null)
        {
            var currentPath = navigationService.GetNavigationUriPath();
            if (currentPath.Contains(pageKey))
            {
                var splitedPath = currentPath.Split('/');
                var indexOfTargetPage = Array.IndexOf(splitedPath, pageKey);
                var currentPageIndex = splitedPath.Length - 1;
                if (indexOfTargetPage == currentPageIndex)
                    return new NavigationResult() { Success = false };

                if (indexOfTargetPage == 0)
                {
                    return await navigationService.GoBackToRootAsync(navigationParameters);
                }

                if (indexOfTargetPage == currentPageIndex - 1)
                {
                    return await navigationService.GoBackAsync(navigationParameters);
                }

                var uri = "";
                for (int i = 0; i <= (currentPageIndex - indexOfTargetPage) + 1; i++)
                {
                    uri += uri.EndsWith("..") ? "/.." : "..";
                }

                return await navigationService.NavigateAsync(uri, navigationParameters);
            }
            return new NavigationResult() { Success = false };

        }

        /// <summary>
        /// Navigate to page <paramref name="pageKey"/>, optionnaly going back to the page if already present in the navigation stack
        /// </summary>
        /// <param name="pageKey"></param>
        /// <param name="popIfPageKeyExists"></param>
#if DEBUG
        public static async Task<INavigationResult> NavigateTo(this INavigationService navigationService, string pageKey, bool popIfPageKeyExists = false, INavigationParameters navigationParameters = null, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0, [CallerMemberName] string callerMember = "")
#else
		public static async Task<INavigationResult> NavigateTo(this INavigationService navigationService, string pageKey, bool popIfPageKeyExists = false, INavigationParameters navigationParameters = null)
#endif
        {

            try
            {
#if DEBUG
                Prism.PrismApplicationBase.Current.Container.Resolve<ILoggerService>().Debug($"NavigateTo {pageKey} from {navigationService.GetNavigationUriPath()} called from {callerFile}:{callerLine} {callerMember}");
#endif
                INavigationResult navResult = new NavigationResult();
                if (popIfPageKeyExists)
                {
                    var currentPath = navigationService.GetNavigationUriPath();
                    if (currentPath.Contains(pageKey))
                    {
                        var splitedPath = currentPath.Split('/');
                        var indexOfTargetPage = Array.IndexOf(splitedPath, pageKey);
                        var currentPageIndex = splitedPath.Length - 1;
                        if (indexOfTargetPage == currentPageIndex)
                        {
                            navResult = new NavigationResult() { Success = false };
                            return navResult;
                        }

                        if (indexOfTargetPage == 0)
                        {
                            navResult = await navigationService.GoBackToRootAsync(navigationParameters);
                            return navResult;
                        }

                        if (indexOfTargetPage == currentPageIndex - 1)
                        {
                            navResult = await navigationService.GoBackAsync(navigationParameters);
                            return navResult;
                        }

                        var uri = "";
                        for (int i = 0; i <= (currentPageIndex - indexOfTargetPage) + 1; i++)
                        {
                            uri += uri.EndsWith("..") ? "/.." : "..";
                        }

                        navResult = await navigationService.NavigateAsync(uri, navigationParameters);
                        return navResult;
                    }
                    else
                    {
                        navResult = await navigationService.NavigateAsync(pageKey, navigationParameters);
                        return navResult;
                    }
                }
                navResult = await navigationService.NavigateAsync(pageKey, navigationParameters);
                return navResult;
            }
            catch (Exception e)
            {
                Prism.PrismApplicationBase.Current.Container.Resolve<ILoggerService>().Error(ex: e);
                //Crashes.TrackError(e);

            }

            return new NavigationResult() { Success = false };
        }
    }
}