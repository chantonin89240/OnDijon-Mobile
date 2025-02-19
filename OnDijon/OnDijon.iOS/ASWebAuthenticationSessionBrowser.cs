using System;
using AuthenticationServices;
using Foundation;
using IdentityModel.OidcClient.Browser;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace OnDijon.iOS
{
    public class ASWebAuthenticationSessionBrowser : IBrowser
    {
        ASWebAuthenticationSession _asWebAuthenticationSession;

        public ASWebAuthenticationSessionBrowser()
        {
        }

        public Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<BrowserResult>();

            try
            {
                _asWebAuthenticationSession = new ASWebAuthenticationSession(
                    new NSUrl(options.StartUrl),
                    new NSUrl(options.EndUrl).Scheme,
                    (callbackUrl, error) =>
                    {
                        tcs.SetResult(CreateBrowserResult(callbackUrl, error));
                        _asWebAuthenticationSession.Dispose();
                    });

                // iOS 13 requires the PresentationContextProvider set
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                    _asWebAuthenticationSession.PresentationContextProvider = new PresentationContextProviderToSharedKeyWindow();

                _asWebAuthenticationSession.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
            return tcs.Task;
        }

        class PresentationContextProviderToSharedKeyWindow : NSObject, IASWebAuthenticationPresentationContextProviding
        {
            public UIWindow GetPresentationAnchor(ASWebAuthenticationSession session)
            {
                return UIApplication.SharedApplication.KeyWindow;
            }
        }

        private static BrowserResult CreateBrowserResult(NSUrl callbackUrl, NSError error)
        {
            if (error == null)
                return new BrowserResult
                {
                    ResultType = BrowserResultType.Success,
                    Response = callbackUrl.AbsoluteString
                };

            if (error.Code == (long)ASWebAuthenticationSessionErrorCode.CanceledLogin)
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UserCancel,
                    Error = error.ToString()
                };

            return new BrowserResult
            {
                ResultType = BrowserResultType.UnknownError,
                Error = error.ToString()
            };
        }

    }
}