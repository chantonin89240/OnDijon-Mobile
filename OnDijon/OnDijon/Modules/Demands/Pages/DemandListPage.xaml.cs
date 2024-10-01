using OnDijon.Common.Views;
using OnDijon.Modules.Demands.ViewsModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Demands.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DemandListPage : BasePage<DemandListViewModel>
    {
        public DemandListPage()
        {
            InitializeComponent();
        }
    }
}
