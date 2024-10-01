using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Account.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {
            //retour arrière bloqué
            return true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //TODO Cleanup : OnDisappearing vide la PasswordValue
            // moved to LoginViewModel.OnNavigatedFrom
            //App.Locator.Login.Cleanup();
        }
    }
}