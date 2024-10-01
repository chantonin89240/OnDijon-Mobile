using OnDijon.Modules.Dashboard.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.RoadworkInformation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoadworkDashboardView : StackLayout
    {
	    //  Refacto : Binding fait dans le vue parent
        public RoadworkDashboardView()
        {
            InitializeComponent();
        }
    }
}