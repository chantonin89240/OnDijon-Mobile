using CoreMotion;
using Foundation;
using OnDijon.Common.Utils.Services.Interfaces;
using System;
using OnDijon.iOS.Services;
using Xamarin.Essentials;
using System.Diagnostics;

[assembly: Xamarin.Forms.Dependency(typeof(OrientationService))]
namespace OnDijon.iOS.Services
{
    public class OrientationService : IOrientationService
    {
        private readonly CMMotionManager motionManager = new CMMotionManager();

        public event EventHandler<DisplayRotation> DisplayRotationChanged;

        public void Start(SensorSpeed speed)
        {
            try
            {
                motionManager.DeviceMotionUpdateInterval = speed.ToPlatform();

                // use a fixed reference frame where X points north and Z points vertically into the sky
                motionManager.StartDeviceMotionUpdates(CMAttitudeReferenceFrame.XTrueNorthZVertical, new NSOperationQueue(), DataUpdated);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void DataUpdated(CMDeviceMotion data, NSError error)
        {
            if (data == null)
                return;

            // Récupération des angles de rotation
            double pitch = data.Attitude.Pitch * 180 / Math.PI;
            double roll = data.Attitude.Roll * 180 / Math.PI;
            DisplayRotationChanged?.Invoke(this, GetDisplayRotation(pitch, roll));
        }

        /// <summary>
        /// Détermine l'orientation de l'écran en fonction de l'orientation de l'appareil
        /// </summary>
        /// <param name="pitch"></param>rotation X en degrés
        /// <param name="roll"></param>rotation Y en degrés
        /// <returns></returns>
        private static DisplayRotation GetDisplayRotation(double pitch, double roll)
        {
            if (pitch > 10)
            {
                //portrait
                return DisplayRotation.Rotation0;
            }
            else if (roll < 0)
            {
                //paysage
                return DisplayRotation.Rotation90;
            }
            else if (roll >= 0)
            {
                //paysage inversé
                return DisplayRotation.Rotation270;
            }
            else
            {
                return DisplayRotation.Unknown;
            }
        }

        public void Stop()
        {
            try
            {
                motionManager.StopDeviceMotionUpdates();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}