using OnDijon.Common.Views;
using OnDijon.Modules.JobOffer.ViewModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.JobOffer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCityPage : BasePage<SelectCityViewModel>
    {
        public SelectCityPage()
        {
            InitializeComponent();
        }
    }
}