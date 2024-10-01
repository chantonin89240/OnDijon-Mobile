using OnDijon.Common.Views;
using OnDijon.Modules.RoadworkInformation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.RoadworkInformation.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoadworkInformationPage : BasePage<RoadworkInformationViewModel>
    {
        public RoadworkInformationPage()
        {
            InitializeComponent();
        }
        // Refacto : Fait en binding dans le xaml
        // private void SwitchView_Toggled(object sender, ToggledEventArgs e)
        // {
        //     var listSelected = e.Value;
        //     ViewModel.DoDisplayList = listSelected;
        // }
    }
}