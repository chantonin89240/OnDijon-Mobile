
using OnDijon.Common.Views;
using OnDijon.Modules.CustomContent.ViewModel;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.CustomContent.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomContentView : BasePage<CustomContentViewModel>
    {
        public CustomContentView()
        { 
            InitializeComponent();
        }
    }
}