using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapScaleBarView : StackLayout
    {
        /// <summary>
        /// Scale bar max width in px
        /// </summary>
        private const double MAX_WIDTH_PX = 100;

        /// <summary>
        /// DPI adjustment ratio for mobile screens
        /// </summary>
        private const double DPI_RATIO = 2;

        /// <summary>
        /// Scale values in meters
        /// </summary>
        private static readonly double[] SCALE_VALUES = new double[] { 500000, 200000, 100000, 50000, 20000, 10000, 5000, 2000, 1000, 500, 200, 100, 50, 20, 10, 5, 2, 1 };

        public static readonly BindableProperty MapViewProperty = BindableProperty.Create(nameof(MapView), typeof(MapView), typeof(MapScaleBarView), propertyChanged: MapViewPropertyChanged);

        public MapView MapView
        {
            get { return (MapView)GetValue(MapViewProperty); }
            set { SetValue(MapViewProperty, value); }
        }

        public MapScaleBarView()
        {
            InitializeComponent();
        }

        private static void MapViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (MapScaleBarView)bindable;

            if (newValue != null)
            {
                var mapView = newValue as MapView;

                if (mapView.DrawStatus == DrawStatus.Completed)
                {
                    UpdateDisplay(view.Label, view.ScaleBar, mapView.UnitsPerPixel);
                }
                else
                {
                    mapView.DrawStatusChanged += (sender, e) =>
                    {
                        if (e.Status == DrawStatus.Completed)
                        {
                            UpdateDisplay(view.Label, view.ScaleBar, mapView.UnitsPerPixel);
                        }
                    };
                }

                mapView.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(mapView.UnitsPerPixel))
                    {
                        UpdateDisplay(view.Label, view.ScaleBar, mapView.UnitsPerPixel);
                    }
                };
            }
        }

        private static void UpdateDisplay(Label label, View scaleBar, double mapResolution)
        {
            if (double.IsNaN(mapResolution)) return;

            var adjustedMapResolution = mapResolution * DPI_RATIO;
            var scale = GetDisplayScale(adjustedMapResolution);
            scaleBar.WidthRequest = scale / adjustedMapResolution;
            label.Text = scale < 1000 ? $"{scale} m" : $"{scale / 1000} km";
        }

        private static double GetDisplayScale(double mapResolution)
        {
            foreach (var scale in SCALE_VALUES)
            {
                var widthInPx = scale / mapResolution;
                if (widthInPx <= MAX_WIDTH_PX)
                {
                    return scale;
                }
            }
            return 1;
        }
    }
}