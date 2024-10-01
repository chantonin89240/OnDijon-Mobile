using Android.Hardware;
using Xamarin.Essentials;

namespace OnDijon.Droid.Services
{
    public static class SensorSpeedExtensions
    {
        public static SensorDelay ToPlatform(this SensorSpeed sensorSpeed)
        {
            switch (sensorSpeed)
            {
                case SensorSpeed.Fastest:
                    return SensorDelay.Fastest;
                case SensorSpeed.Game:
                    return SensorDelay.Game;
                case SensorSpeed.UI:
                    return SensorDelay.Ui;
            }

            return SensorDelay.Normal;
        }
    }
}