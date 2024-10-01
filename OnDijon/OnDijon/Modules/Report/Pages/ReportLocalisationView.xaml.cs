using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using OnDijon.Common.Utils.UI;
using OnDijon.Modules.Report.ViewModels;
using OnDijon.Common.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Report.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //TODO : Refacto modification à réfléchir quand retravaillé
    public partial class ReportLocalisationView : BasePage<ReportLocalisationViewModel>
    {
        private ReportLocalisationViewModel _reportLocalisationVM;

        private GraphicsOverlay _localisationOverlay;
        private GraphicsOverlay _reportsOverlay;

        private string _selectedSuggestion;

        private bool _coordinatesFromAddress = false;
        private bool _addressFromCoordinates = false;

        private int _reportsCount = 0;

        protected override void OnBindingContextChanged()
        {
	        base.OnBindingContextChanged();
	        if (BindingContext is ReportLocalisationViewModel reportLocalisationViewModel)
	        {
		        _reportLocalisationVM = reportLocalisationViewModel;
		        _reportLocalisationVM.OnPageInit();
		        btNext.IsEnabled = _reportLocalisationVM.CurrentPosition != null;
		        _reportLocalisationVM.PropertyChanged += OnPropertyChanged;
		        _reportLocalisationVM.InitMap();
		        InitMapView();
	        }
		        
        }

        public ReportLocalisationView()
        {
	        InitializeComponent();
        }

  

        protected override void OnDisappearing()
        {
            _reportLocalisationVM.PropertyChanged -= OnPropertyChanged;
            ResetMapView();

            //TODO Cleanup : OnDisappearing
            _reportLocalisationVM.Cleanup();
            base.OnDisappearing();
        }
        
        private void InitMapView()
        {
            MapView.LayerViewStateChanged += MapView_LayerViewStateChanged;
            MapView.GeoViewTapped += MapView_GeoViewTapped;
            MapView.Map = _reportLocalisationVM.Map;

            //add localisation overlay
            _localisationOverlay = new GraphicsOverlay();
            MapView.GraphicsOverlays.Add(_localisationOverlay);

            //add reports overlay
            _reportsOverlay = new GraphicsOverlay();
            MapView.GraphicsOverlays.Add(_reportsOverlay);
        }

        private void ResetMapView()
        {
            MapView.LayerViewStateChanged -= MapView_LayerViewStateChanged;
            MapView.GeoViewTapped -= MapView_GeoViewTapped;
            MapView.GraphicsOverlays.Clear();
            MapView.Map = null;
        }

        private async void MapView_LayerViewStateChanged(object sender, LayerViewStateChangedEventArgs e)
        {
            if (e.LayerViewState.Status == LayerViewStatus.Active)
            {
                if (_reportLocalisationVM.CurrentPosition != null)
                {
                    await ShowCurrentPosition();

                    if (_reportLocalisationVM.Reports != null && _reportLocalisationVM.Reports.Any())
                    {
                        await MapView.ShowReports(_reportLocalisationVM.Reports, _reportsOverlay, _reportLocalisationVM.CurrentPosition);
                    }
                    else
                    {
                        await MapView.SetViewpointCenterAsync(_reportLocalisationVM.CurrentPosition, MapUtils.DEFAULT_MAP_SCALE);
                    }
                }
                else
                {
                    MapView.SetViewpoint(MapUtils.DEFAULT_MAP_VIEWPOINT);
                }
            }
        }

        private async void MapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            //search for a report where the user clicked
            var result = await MapView.IdentifyGraphicsOverlayAsync(_reportsOverlay, e.Position, 20, false, 1);
            if (result.Graphics.Any())
            {
                var reportId = (int)result.Graphics[0].Attributes[MapUtils.REPORT_ID_KEY];
                var report = _reportLocalisationVM.Reports.First(r => { return r.Id == reportId; });
                _reportLocalisationVM.GoToReportCommand.Execute(report);
            }
            else
            {
                _addressFromCoordinates = true;
                _reportLocalisationVM.CurrentPosition = e.Location.ToWgs84();
            }
        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_reportLocalisationVM.CurrentPosition))
            {
                if (_reportLocalisationVM.CurrentPosition != null)
                {
                    await ShowCurrentPosition();
                    await MapView.SetViewpointCenterAsync(_reportLocalisationVM.CurrentPosition);

                    if (!_coordinatesFromAddress)
                    {
                        _reportLocalisationVM.GetAddressFromCoordinates(_reportLocalisationVM.CurrentPosition);
                    }

                    _reportLocalisationVM.GetReportsCommand.Execute(_reportLocalisationVM.CurrentPosition);
                }
                else
                {
                    //remove location graphic
                    _localisationOverlay.Graphics.Clear();

                    _reportLocalisationVM.Address = null;
                    btNext.IsEnabled = false;
                }
                ShowReportsNumberLabel(_reportLocalisationVM.CurrentPosition != null);

                _coordinatesFromAddress = false;
            }
            else if (e.PropertyName == nameof(_reportLocalisationVM.Suggestions))
            {
                if (_reportLocalisationVM.Suggestions?.Count > 0)
                {
                    //reset ListView position
                    SuggestionsListView.ScrollTo(_reportLocalisationVM.Suggestions.First(), ScrollToPosition.MakeVisible, false);
                    SuggestionsLayout.IsVisible = true;
                }
                else
                {
                    SuggestionsLayout.IsVisible = false;
                }
            }
            else if (e.PropertyName == nameof(_reportLocalisationVM.Address))
            {
                if (!_addressFromCoordinates && _reportLocalisationVM.Address?.Length > 2 && !_reportLocalisationVM.Address.Equals(_selectedSuggestion))
                {
                    _reportLocalisationVM.GetSuggestions(_reportLocalisationVM.Address);
                }

                _addressFromCoordinates = false;
                _selectedSuggestion = null;
            }
            else if (e.PropertyName == nameof(_reportLocalisationVM.Reports))
            {
                await MapView.ShowReports(_reportLocalisationVM.Reports, _reportsOverlay, _reportLocalisationVM.CurrentPosition);
                _reportsCount = _reportLocalisationVM.Reports?.Count ?? 0;
                UpdateReportsNumberLabel();
            }
        }

        private async Task ShowCurrentPosition()
        {
            await _localisationOverlay.ShowPinGraphic(_reportLocalisationVM.CurrentPosition);

            SuggestionsLayout.IsVisible = false;
            btNext.IsEnabled = true;
        }

        void OnSuggestionSelected(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) { return; }

            _coordinatesFromAddress = true;
            _selectedSuggestion = e.Item as string;
            _reportLocalisationVM.GetCoordinatesFromAddress(_selectedSuggestion);

            //Deselect Item and hide suggestions
            ((ListView)sender).SelectedItem = null;
            SuggestionsLayout.IsVisible = false;
        }

        void OnLocationButtonClicked(object sender, EventArgs args)
        {
            _addressFromCoordinates = true;
        }

        private void UpdateReportsNumberLabel()
        {
            ReportsCountLabel.Text = $"Il y a {_reportsCount} {(_reportsCount > 1 ? "signalements existants" : "signalement existant")} autour de votre position";
        }

        private void ShowReportsNumberLabel(bool show)
        {
            if (show)
            {
                UpdateReportsNumberLabel();
            }
            ReportsCountContainer.IsVisible = show;
            UseLocationButton.IsVisible = !show;
        }
        
        protected override bool OnBackButtonPressed()
        {
            ViewModel?.CloseCommand.Execute(true);
            // block back button 
            return true;
        }
    }
}