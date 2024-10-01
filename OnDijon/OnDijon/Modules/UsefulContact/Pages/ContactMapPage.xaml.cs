 using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using OnDijon.Common.Utils.UI;
using OnDijon.Common.Views;
using OnDijon.Modules.UsefulContact.Entities.Models;
using OnDijon.Modules.UsefulContact.ViewsModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.UsefulContact.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //TODO : Refacto, à vérifier en dernier 
    public partial class ContactMapPage : BaseView
    {
        private ContactMapViewModel _viewModel => BindingContext as ContactMapViewModel;
        private GraphicsOverlay _localisationOverlay;
        private GraphicsOverlay _localisationContactOverlay;
        //private Graphic SelectedGraphicContact;

        public ContactMapPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
           
        }
        protected override void OnDisappearing()
        {
            _viewModel.PropertyChanged -= OnDomainContactChanged;
            ResetMapView();
            base.OnDisappearing();
        }

        protected override void OnBindingContextChanged()
        {
            InitMapView();
            if (BindingContext != null)
                _viewModel.PropertyChanged += OnDomainContactChanged;
            base.OnBindingContextChanged();
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
                MapView.SetViewpoint(MapUtils.DEFAULT_MAP_VIEWPOINT);
            }
        }

        private async void MapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            var result = await MapView.IdentifyGraphicsOverlayAsync(_localisationContactOverlay, e.Position, 20, false, 1);
            if (result.Graphics.Any())
            {
                Graphic SelectedGraphicContact = result.Graphics[0];
                ContactModel contact = _viewModel.ContactList.Where(c => c.EditId == (string)SelectedGraphicContact.Attributes["contact"]).FirstOrDefault();
                _viewModel.ContactSelected = contact;
                _viewModel.ViewContactPopup();
            }
        }


        private void Domain_Selected(object sender, EventArgs e)
        {
            if (DomainPicker.SelectedItem != null)
            {
                _viewModel.DomainSelected = (ContactDomainModel)DomainPicker.SelectedItem;
                _viewModel.GetContactList();
            }
        }

        private async void OnDomainContactChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.Map))
            { 
                //MapView.Map = _viewModel.Map;
                
            }
                
            if (e.PropertyName == nameof(_viewModel.ContactList))
            {
                await ShowContactOnMapAsync(_localisationContactOverlay, _viewModel.ContactList.ToList());
                ResultatNumber.Text = _viewModel.ContactList.Count + " résultat" + (_viewModel.ContactList.Count > 1 ? "s" : "");
            }
            else if (e.PropertyName == nameof(_viewModel.Recherche))
            {
                _viewModel.GetContactList();
            }
            else if (e.PropertyName == nameof(_viewModel.DomainList))
            {
                if (_viewModel.DomainSelected != null)
                {
                    for (int x = 0; x < DomainPicker.ItemsSource.Count; x++)
                    {
                        if (((ContactDomainModel)DomainPicker.ItemsSource[x]).Id == _viewModel.DomainSelected.Id)
                        {
                            DomainPicker.SelectedIndex = x;
                        }
                    }
                    _viewModel.GetContactList();
                }
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
                    await ShowPinGraphicContact(positionOverlay, new MapPoint(x, y, new SpatialReference(4326)), contact);
                }
                if (contactList.Count == 1)
                {
                    await MapView.SetViewpointCenterAsync(new MapPoint(Convert.ToDouble(contactList.First().X.Replace(".", ",")), Convert.ToDouble(contactList.First().Y.Replace(".", ",")), new SpatialReference(4326)));
                    await MapView.SetViewpointScaleAsync(10000);
                }
                else if(contactList.Count > 1)
                {

                }

            }
        }

        private async Task ShowPinGraphicContact(GraphicsOverlay positionOverlay, MapPoint position, ContactModel contact)
        {
            //création de la pin
            var pinSymbol = await MapUtils.CreatePictureMarkerSymbolFromResources("OnDijon.Assets.pinSelected.png");
            pinSymbol.Width = 37;
            pinSymbol.Height = 49;
            pinSymbol.OffsetY = pinSymbol.Height / 2;

            //Ajout sur la carte de la pin avec l'id du contact
            var pinGraphic = new Graphic(position, pinSymbol);
            pinGraphic.Attributes.Add("contact", contact.EditId);
            positionOverlay.Graphics.Add(pinGraphic);
        }

        private void OnLocationButtonClicked(object sender, EventArgs e)
        {
            if (_viewModel.CurrentPosition != null)
            {
                MapView.SetViewpointCenterAsync(_viewModel.CurrentPosition);
            }
        }
    }
}