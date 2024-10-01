using Android.Hardware;
using Android.Runtime;
using System;
using System.Diagnostics;
using Android.App;
using Android.Content;
using OnDijon.Common.Utils.Services.Interfaces;
using OnDijon.Droid.Services;
using Xamarin.Essentials;

[assembly:Xamarin.Forms.Dependency(typeof(OrientationService))]
namespace OnDijon.Droid.Services
{
    public class OrientationService : IOrientationService
    {
        private readonly SensorManager sensorManager;

        private OrientationSensorListener listener;
        private Sensor orientationSensor;

        public event EventHandler<DisplayRotation> DisplayRotationChanged;

        public OrientationService()
        {
            sensorManager = Application.Context.GetSystemService(Context.SensorService) as SensorManager;
        }

        public void Start(SensorSpeed speed)
        {
            try
            {
                listener = new OrientationSensorListener(DisplayRotationChanged);
                //on utilise GameRotationVector car il ne dépend pas du magnétomètre (https://developer.android.com/guide/topics/sensors/sensors_position#sensors-pos-gamerot)
                orientationSensor = sensorManager.GetDefaultSensor(SensorType.GameRotationVector);
                sensorManager.RegisterListener(listener, orientationSensor, speed.ToPlatform());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private sealed class OrientationSensorListener : Java.Lang.Object, ISensorEventListener
        {
            private readonly EventHandler<DisplayRotation> _displayRotationChanged;

            private readonly float[] R = new float[9];
            private readonly float[] rotationVector = new float[4];
            private readonly float[] orientation = new float[3];

            public OrientationSensorListener(EventHandler<DisplayRotation> displayRotationChanged)
            {
                _displayRotationChanged = displayRotationChanged;
            }

            void ISensorEventListener.OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
            {
                //not used
            }

            void ISensorEventListener.OnSensorChanged(SensorEvent e)
            {
                if (e?.Values != null && e.Values.Count >= 4)
                {
                    // Calcul du vecteur de rotation à partir des données du capteur d'orientation
                    rotationVector[0] = e.Values[0];
                    rotationVector[1] = e.Values[1];
                    rotationVector[2] = e.Values[2];
                    rotationVector[3] = e.Values[3];
                    SensorManager.GetRotationMatrixFromVector(R, rotationVector);

                    // Récupération des angles de rotation
                    SensorManager.GetOrientation(R, orientation);
                    double pitch = orientation[1] * 180 / Math.PI;
                    double roll = orientation[2] * 180 / Math.PI;
                    _displayRotationChanged?.Invoke(this, GetDisplayRotation(pitch, roll));
                }
            }
        }

        /// <summary>
        /// Détermine l'orientation de l'écran en fonction de l'orientation de l'appareil
        /// </summary>
        /// <param name="pitch"></param>rotation X en degrés
        /// <param name="roll"></param>rotation Y en degrés
        /// <returns></returns>
        private static DisplayRotation GetDisplayRotation(double pitch, double roll)
        {
            if (pitch < -10)
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
                if (listener == null || orientationSensor == null) { return; }

                sensorManager.UnregisterListener(listener, orientationSensor);
                listener.Dispose();
                listener = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}