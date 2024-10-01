using OnDijon.Modules.Account.ViewModels;
using OnDijon.Common.Views;

namespace OnDijon.Modules.Account.Pages
{
    public partial class ProfileView : BaseView
    {
        

        public ProfileView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refacto moved to ViewModel.OnNavigatedToAsync
            //_profileVM.GetProfile();
        }
    }
}