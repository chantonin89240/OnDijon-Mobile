using System.ComponentModel;
using System.Linq;
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
    public partial class ReportsUserView : BasePage<ReportsUserViewModel>
    {
        private GraphicsOverlay _reportsOverlay;

        public ReportsUserView()
        {
            InitializeComponent();
        }
        
		// Todo Refacto, vérifier possibilité de faire le mmême comportement qu'en Binding Mvvm 
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.PropertyChanged += OnPropertyChanged;
            ViewModel.GetReportsCommand.Execute(null);
            ViewModel.InitMap();
            InitMapView();
        }

        protected override void OnDisappearing()
        {
            ViewModel.PropertyChanged -= OnPropertyChanged;
            ResetMapView();

            //TODO Cleanup : OnDisappearing
            base.OnDisappearing();
        }

        private void InitMapView()
        {
            MapView.LayerViewStateChanged += MapView_LayerViewStateChanged;
            MapView.GeoViewTapped += MapView_GeoViewTapped;
            MapView.Map = ViewModel.Map;

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

        private void MapView_LayerViewStateChanged(object sender, LayerViewStateChangedEventArgs e)
        {
            if (e.LayerViewState.Status == LayerViewStatus.Active)
            {
                MapView.ShowReports(ViewModel.Reports, _reportsOverlay);
            }
        }
        
        private async void MapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            //search for a report where the user clicked
            var result = await MapView.IdentifyGraphicsOverlayAsync(_reportsOverlay, e.Position, 20, false, 1);
            if (result.Graphics.Any())
            {
                var reportId = (int)result.Graphics[0].Attributes[MapUtils.REPORT_ID_KEY];
                var report = ViewModel.Reports.First(r => { return r.Id == reportId; });
                ViewModel.GoToReportCommand.Execute(report);
            }
        }
        //TODO : Refacto ???
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ViewModel.Reports)))
            {
                MapView.ShowReports(ViewModel.Reports, _reportsOverlay);
            }
        }
    }
}