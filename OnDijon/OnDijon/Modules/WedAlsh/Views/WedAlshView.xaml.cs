using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.WedAlsh.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WedAlshView : StackLayout
    {
	    // TODO Refacto : cette classe ne semble plus utilisée ? 
        public WedAlshView()
        {
            //BindingContext = App.Locator.WedAlsh;
            InitializeComponent();
        }
    }
}