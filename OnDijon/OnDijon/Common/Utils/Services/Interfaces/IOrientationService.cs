using System;
using Xamarin.Essentials;

namespace OnDijon.Common.Utils.Services.Interfaces
{
    public interface IOrientationService
    {
        void Start(SensorSpeed speed);
        void Stop();
        event EventHandler<DisplayRotation> DisplayRotationChanged;
    }
}