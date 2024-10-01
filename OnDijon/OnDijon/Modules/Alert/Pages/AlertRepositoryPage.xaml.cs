using OnDijon.Common.Views;
using OnDijon.Modules.Alert.ViewModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Alert.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlertRepositoryPage : BasePage<AlertRepositoryViewModel>
    {
        public AlertRepositoryPage()
        {
            InitializeComponent();
        }
    }
}