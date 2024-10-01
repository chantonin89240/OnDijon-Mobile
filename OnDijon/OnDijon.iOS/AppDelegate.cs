
using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Forms.Platform;
using Foundation;
using Plugin.FirebasePushNotification;
using System;
using IdentityModel.OidcClient.Browser;
using OnDijon.Common.Permissions;
using OnDijon.iOS.Services;
using Prism;
using Prism.Ioc;
using UIKit;
using Xamarin.Forms;

namespace OnDijon.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            CachedImageRenderer.Init();
            Forms.Init();
            CarouselViewRenderer.Init();
            Plugin.InputKit.Platforms.iOS.Config.Init();
            
            DependencyService.Register<ASWebAuthenticationSessionBrowser>();


            LoadApplication(new App(new iOSInitializer()));

            Firebase.Core.App.Configure();
            FirebasePushNotificationManager.Initialize(options, true);

            Xamarin.FormsMaps.Init();

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
    
    public class iOSInitializer : IPlatformInitializer
    {
	    public void RegisterTypes(IContainerRegistry containerRegistry)
	    {
		    // Register any platform specific implementations
		    containerRegistry.RegisterSingleton<IPermissionService, PermissionService>();
            //containerRegistry.RegisterSingleton<IBrowser, ASWebAuthenticationSessionBrowser>();
        }
    }
}
