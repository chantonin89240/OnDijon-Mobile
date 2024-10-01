using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System;


namespace OnDijon.Droid
{
    [Activity(Label = "OidcCallbackActivity", Exported = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "fr.dm.ondijon")]
    public class OidcCallbackActivity : Activity
    {
        public static event Action<string> Callbacks;

        public OidcCallbackActivity()
        {
            Log.Debug("OidcCallbackActivity", "constructing OidcCallbackActivity");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Callbacks?.Invoke(Intent?.DataString);

            Finish();

            StartActivity(typeof(MainActivity));
        }
    }
}