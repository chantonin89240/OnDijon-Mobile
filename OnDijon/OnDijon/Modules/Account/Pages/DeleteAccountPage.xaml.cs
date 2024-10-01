using OnDijon.Common.Views;
using OnDijon.Modules.Account.ViewModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Account.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteAccountPage : BasePage<DeleteAccountViewModel>
    {
        public DeleteAccountPage()
        {
            InitializeComponent();
        }
    }
}