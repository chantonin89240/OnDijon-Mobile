
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapCompassView : ImageButton
    {
        public static readonly BindableProperty MapViewProperty = BindableProperty.Create(nameof(MapView), typeof(MapView), typeof(MapCompassView), propertyChanged: MapViewPropertyChanged);

        public MapView MapView
        {
            get { return (MapView)GetValue(MapViewProperty); }
            set { SetValue(MapViewProperty, value); }
        }

        public MapCompassView()
        {
            InitializeComponent();
        }

        private static void MapViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (MapCompassView)bindable;

            if (newValue != null)
            {
                var mapView = newValue as MapView;

                if (mapView.DrawStatus == DrawStatus.Completed)
                {
                    UpdateDisplay(view, mapView.MapRotation);
                }
                else
                {
                    mapView.DrawStatusChanged += (sender, e) =>
                    {
                        if (e.Status == DrawStatus.Completed)
                        {
                            UpdateDisplay(view, mapView.MapRotation);
                        }
                    };
                }

                mapView.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(mapView.MapRotation))
                    {
                        UpdateDisplay(view, mapView.MapRotation);
                    }
                };

                //click on compass reset mapview rotation
                view.Clicked += (sender, e) => mapView.SetViewpointRotationAsync(0);
            }
        }

        private static void UpdateDisplay(MapCompassView view, double mapRotation)
        {
            if (double.IsNaN(mapRotation)) return;

            //compass rotation is in sync with map rotation (counter clockwise)
            view.Rotation = -mapRotation;
        }
    }
}