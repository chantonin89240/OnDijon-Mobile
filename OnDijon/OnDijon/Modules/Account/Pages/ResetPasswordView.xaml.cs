using OnDijon.Common.Views;
using Xamarin.Forms;

namespace OnDijon.Modules.Account.Pages
{
    public partial class ResetPasswordView : BaseView
    {
        public ResetPasswordView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EmailEntry.Text = "";
        }
        
    }
}