using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.Tasks.NetworkAnalysis;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Mapsui.UI.Forms;
using OnDijon.Common.Utils.UI;
using OnDijon.Common.Views;
using OnDijon.Modules.Itineraire.ViewModels;
using OnDijon.Modules.UsefulContact.Entities.Models;
using OnDijon.Modules.UsefulContact.ViewsModels;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Itineraire.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItinerairePage : BaseView
    {
        private ContactMapViewModel _viewModel => BindingContext as ContactMapViewModel;
        private GraphicsOverlay _localisationOverlay;
        private GraphicsOverlay _localisationContactOverlay;


        public ItinerairePage()
        {
            InitializeComponent();
            InitMapView();
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPKaf1c2fd2a82e46b88dec9c83eb0f58d4NUnbV_pLcuEwlFiUyEmLHFl7ylM2VvJL95IJBk60pahDXK8FMzc0JNrtZv4aAz82";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = (ItineraireViewModel)BindingContext;
            viewModel.MapView = MapView;
        }

        private void InitMapView()
        {
            MapView.DismissCallout();
            MapView.LayerViewStateChanged += MapView_LayerViewStateChanged;
            MapView.GeoViewTapped += MapView_GeoViewTapped;

            _localisationOverlay = new GraphicsOverlay();
            MapView.GraphicsOverlays.Add(_localisationOverlay);
            _localisationContactOverlay = new GraphicsOverlay();
            MapView.GraphicsOverlays.Add(_localisationContactOverlay);
        }

        private void ResetMapView()
        {
            MapView.LayerViewStateChanged -= MapView_LayerViewStateChanged;
            MapView.GeoViewTapped -= MapView_GeoViewTapped;
            MapView.GraphicsOverlays.Clear();
        }

        private async void MapView_LayerViewStateChanged(object sender, LayerViewStateChangedEventArgs e)
        {
            if (e.LayerViewState.Status == LayerViewStatus.Active)
            {
                await MapView.SetViewpointAsync(MapUtils.DEFAULT_MAP_VIEWPOINT);
            }
        }

        private async void MapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            var result = await MapView.IdentifyGraphicsOverlayAsync(_localisationContactOverlay, e.Position, 20, false, 1);
            if (result.Graphics.Any())
            {
                Graphic selectedGraphicContact = result.Graphics[0];
                ContactModel contact = _viewModel.ContactList.FirstOrDefault(c => c.EditId == (string)selectedGraphicContact.Attributes["contact"]);
                _viewModel.ContactSelected = contact;
                _viewModel.ViewContactPopup();
            }
        }

        private async Task ShowContactOnMapAsync(GraphicsOverlay positionOverlay, List<ContactModel> contactList)
        {
            positionOverlay.Graphics.Clear();
            if (contactList.Count > 0)
            {
                double x, y;
                foreach (var contact in contactList.OrderByDescending(a => a.Y))
                {
                    x = Convert.ToDouble(contact.X.Replace(".", ","));
                    y = Convert.ToDouble(contact.Y.Replace(".", ","));
                    await ShowPinGraphicContact(positionOverlay, new MapPoint(x, y, SpatialReferences.Wgs84), contact);
                }
                if (contactList.Count == 1)
                {
                    await MapView.SetViewpointCenterAsync(new MapPoint(Convert.ToDouble(contactList.First().X.Replace(".", ",")), Convert.ToDouble(contactList.First().Y.Replace(".", ",")), SpatialReferences.Wgs84));
                    await MapView.SetViewpointScaleAsync(10000);
                }
                else if (contactList.Count > 1)
                {
                    // Logic for handling multiple contacts on the map
                }
            }
        }

        private async Task ShowPinGraphicContact(GraphicsOverlay positionOverlay, MapPoint position, ContactModel contact)
        {
            // Create the pin symbol
            var pinSymbol = await MapUtils.CreatePictureMarkerSymbolFromResources("OnDijon.Assets.pinSelected.png");
            pinSymbol.Width = 37;
            pinSymbol.Height = 49;
            pinSymbol.OffsetY = pinSymbol.Height / 2;

            // Add the pin with the contact's ID to the map
            var pinGraphic = new Graphic(position, pinSymbol);
            pinGraphic.Attributes.Add("contact", contact.EditId);
            positionOverlay.Graphics.Add(pinGraphic);
        }

        /*private void OnLocationButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("onlocationbuttonclicked");

            if (_viewModel.CurrentPosition != null)
            {
                Console.WriteLine("currentposition? pas nul");
                MapView.SetViewpointCenterAsync(_viewModel.CurrentPosition);
            }
        }*/


    }
}
