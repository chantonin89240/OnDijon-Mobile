using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Firebase.Analytics;
using Firebase.Messaging;
using Plugin.FirebasePushNotification;
using System;

namespace OnDijon.Droid
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            //Firebase settings
            _ = FirebaseAnalytics.GetInstance(this);
            _ = FirebaseMessaging.Instance;
            FirebasePushNotificationManager.NotificationActivityType = typeof(MainActivity);
            FirebasePushNotificationManager.IconResource = Resource.Drawable.ic_notif;
            FirebasePushNotificationManager.Color = new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.color_accent));

            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                FirebasePushNotificationManager.DefaultNotificationChannelId = "OnDijonSignalement";
                FirebasePushNotificationManager.DefaultNotificationChannelName = "Signalement";
                FirebasePushNotificationManager.DefaultNotificationChannelImportance = NotificationImportance.High;
            }

            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
            FirebasePushNotificationManager.Initialize(this, false);
#endif
        }
    }
}