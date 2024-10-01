using Xamarin.UITest;
using Xamarin.UITest.Configuration;

namespace OnDijon.UITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform, bool clearData)
        {
            AppDataMode dataMode = clearData ? AppDataMode.Clear : AppDataMode.DoNotClear;

            if (platform == Platform.Android)
            {
                return ConfigureApp.Android
                    .InstalledApp("com.capgemini.ondijon")
                    .StartApp(dataMode);
            }

            return ConfigureApp.iOS.StartApp(dataMode);
        }
    }
}