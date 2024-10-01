using OnDijon.Common.Services;
using OnDijon.Common.Entities.Model;
using OnDijon.Modules.RoadworkInformation.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mapsui.Projection;
using Mapsui.Utilities;
using Mapsui.UI.Forms;
using System.IO;
using System.Reflection;
using OnDijon.Modules.RoadworkInformation.ViewModels;

namespace OnDijon.Modules.RoadworkInformation.CustomComponent
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomOSM : StackLayout
    {
	    // TODO Refacto : A ne pas regarder spécialement 
        public static readonly BindableProperty MyHomeLocationProperty = BindableProperty.Create(nameof(MyHomeLocation), typeof(AddressModel), typeof(CustomOSM));
        public static readonly BindableProperty MyWorkLocationProperty = BindableProperty.Create(nameof(MyWorkLocation), typeof(AddressModel), typeof(CustomOSM));
        public static readonly BindableProperty DisplayMyHomeButtonProperty = BindableProperty.Create(nameof(DisplayMyHomeButton), typeof(bool), typeof(CustomOSM), defaultValue: false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: DisplayMyHomeButtonChanged);
        public static readonly BindableProperty DisplayMyLocationButtonProperty = BindableProperty.Create(nameof(DisplayMyLocationButton), typeof(bool), typeof(CustomOSM), defaultValue: true);
        public static readonly BindableProperty DisplayMyWorkButtonProperty = BindableProperty.Create(nameof(DisplayMyWorkButton), typeof(bool), typeof(CustomOSM), defaultValue: false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: DisplayMyWorkButtonChanged);
        public static readonly BindableProperty ListPinProperty = BindableProperty.Create(nameof(ListPin), typeof(ObservableCollection<Pin>), typeof(CustomOSM), defaultBindingMode: BindingMode.TwoWay, propertyChanged: ListPinPropertyChanged);


        private PopupService _PopupService = new PopupService();

        public AddressModel MyHomeLocation
        {
            get { return (AddressModel)GetValue(MyHomeLocationProperty); }
            set { SetValue(MyHomeLocationProperty, value); }
        }

        public AddressModel MyWorkLocation
        {
            get { return (AddressModel)GetValue(MyWorkLocationProperty); }
            set { SetValue(MyWorkLocationProperty, value); }
        }

        public bool DisplayMyHomeButton
        {
            get { return (bool)GetValue(DisplayMyHomeButtonProperty); }
            set { SetValue(DisplayMyHomeButtonProperty, value); }
        }

        public bool DisplayMyLocationButton
        {
            get { return (bool)GetValue(DisplayMyLocationButtonProperty); }
            set { SetValue(DisplayMyLocationButtonProperty, value); }
        }

        public bool DisplayMyWorkButton
        {
            get { return (bool)GetValue(DisplayMyWorkButtonProperty); }
            set { SetValue(DisplayMyWorkButtonProperty, value); }
        }

        public ObservableCollection<Pin> ListPin
        {
            get { return (ObservableCollection<Pin>)GetValue(ListPinProperty); }
            set { SetValue(ListPinProperty, value); }
        }

        private static void DisplayMyHomeButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CustomOSM)bindable;
            if ((bool)newValue && view.MyHomeLocation != null)
            {
                view.MyHomeButton.IsVisible = true;
            }
            else
            {
                view.MyHomeButton.IsVisible = false;
            }
        }

        private static void DisplayMyWorkButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CustomOSM)bindable;
            if ((bool)newValue && view.MyWorkLocation != null)
            {
                view.MyWorkButton.IsVisible = true;
            }
            else
            {
                view.MyWorkButton.IsVisible = false;
            }
        }

        private static void ListPinPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CustomOSM)bindable;
            view.DisplayPin();
        }
        
        public CustomOSM()
        {
            InitializeComponent();
            var map = new Mapsui.Map
            {
                CRS = "EPSG:4326",
                Transformation = new MinimalTransformation(),
                RotationLock = true,
            };

            var tileLayer = OpenStreetMap.CreateTileLayer();
            map.Layers.Add(tileLayer);
            UserMap.Map = map;

            //CenterOnMyLocation();
            //UserMap.MyLocationLayer.UpdateMyLocation(new Position(47.321215004369797, 5.0419750890605188));
            //Default postion : Dijon
            UserMap.Navigator.NavigateTo(SphericalMercator.FromLonLat(5.0419750890605188, 47.321215004369797), 20);

            MyHomeButton.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => NavigateToHome()) });
            //MyLocationButton.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => CenterOnMyLocation(1000)) });
            MyLocationButton.Command = new Command(() => CenterOnMyLocation(1000));
            MyWorkButton.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => NavigateToWork()) });
        }

        private void MyHomeButtonClicked(object sender, EventArgs e)
        {
            UserMap.Navigator.NavigateTo(SphericalMercator.FromLonLat(Convert.ToDouble(MyHomeLocation.Y), Convert.ToDouble(MyHomeLocation.X)), (double)10, (long)2000);
        }

        private void NavigateToHome()
        {
            if (MyHomeLocation != null)
            {
                UserMap.Navigator.NavigateTo(SphericalMercator.FromLonLat(Convert.ToDouble(MyHomeLocation.Y), Convert.ToDouble(MyHomeLocation.X)), (double)10, (long)1000);
        }
        }

        private void NavigateToWork()
        {
            if(MyWorkLocation != null)
        {
                UserMap.Navigator.NavigateTo(SphericalMercator.FromLonLat(Convert.ToDouble(MyWorkLocation.Y), Convert.ToDouble(MyWorkLocation.X)), (double)10, (long)1000);
            }
        }

        private void DisplayPin()
        {
            UserMap.PinClicked -= UserMap_PinClicked;
            UserMap.Pins.Clear();
            UserMap.Drawables.Clear();
            if (ListPin.Count != 0)
            {
                foreach (OSMPin pin in ListPin)
                {
                    UserMap.Pins.Add(pin);
                    DrawPolygon(pin);
                }
                UserMap.PinClicked += UserMap_PinClicked;
            }
            if (MyHomeLocation != null)
            {
                DrawPin("PinHome", "OnDijon.Assets.PinHome.png", Double.Parse(MyHomeLocation.X), Double.Parse(MyHomeLocation.Y));
            }
            if (MyWorkLocation != null)
            {
                DrawPin("PinWork", "OnDijon.Assets.PinWork.png", Double.Parse(MyWorkLocation.X), Double.Parse(MyWorkLocation.Y));
            }

        }

        private void DrawPin(string label, string pathPin, Double X, Double Y)
        {
            if (UserMap.Pins.Where(i => i.Label == label).Any())
            {
                UserMap.Pins.Where(i => i.Label == label).FirstOrDefault().Position = new Position(X, Y);
            }
            else
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pathPin);
                {
                    if (stream == null) throw new Exception($"Could not find EmbeddedResource {pathPin}");
                    byte[] icon = stream.ToBytes();
                    OSMPin pin = new OSMPin()
                    {
                        Position = new Position(X, Y),
                        Type = PinType.Icon,
                        Color = new Color(26, 57, 114),
                        Scale = 1f,
                        Icon = icon,
                        Anchor = new Point(0, 24),
                        ObjectType = "",
                        Label = label
                    };
                    UserMap.Pins.Add(pin);
                };
            }
        }

        private void DrawPolygon(OSMPin pin)
        {
            var polygon = new Polygon
            {
                StrokeColor = Color.Blue,
                FillColor = new Color(0, 0, 255, 0.25)
            };
            foreach (var ring in pin.Area)
            {
                polygon.Positions.Add(new Position(ring.Latitude, ring.Longitude));
                polygon.MaxVisible = 1;
            }
            UserMap.Drawables.Add(polygon);
            
        }

        private void UserMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            OSMPin tempPin = (OSMPin)e.Pin;
            e.Handled = true;

            switch ((ElementTypeEnum)Enum.Parse(typeof(ElementTypeEnum), tempPin.ObjectType))
            {
                case ElementTypeEnum.InfosTravaux:
                    {
                        _PopupService.Show(new RoadworkPopupView(BindingContext as RoadworkInformationViewModel,  tempPin.EditId));
                        break;
                    }
                case ElementTypeEnum.POI:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }


        private async void CenterOnMyLocation(long TimeAnim = 0)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
                UserMap.MyLocationLayer.UpdateMyLocation(new Position(location.Latitude, location.Longitude));
                UserMap.Navigator.NavigateTo(SphericalMercator.FromLonLat(location.Longitude, location.Latitude), (double)10, TimeAnim);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " \n " + ex.StackTrace);
            }
        }



    }
}