using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Tasks.Geocoding;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using OnDijon.Common.Utils.UI;
using OnDijon.Common.Views;
using OnDijon.Modules.Abris.Entities.Models;
using OnDijon.Modules.Abris.ViewModels;
using OnDijon.Modules.UsefulContact.Entities.Models;
using OnDijon.Modules.UsefulContact.ViewsModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Abris.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbrisPage : BaseView
	{
        private AbrisViewModel _viewModel => BindingContext as AbrisViewModel;
        
        public static MapView mapView;
        public AbrisPage ()
		{
			InitializeComponent ();
            mapView = MapView;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        

    }
}