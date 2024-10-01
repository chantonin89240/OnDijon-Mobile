using OnDijon.Common.Views;
using OnDijon.Modules.Booking.ViewModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Booking.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookingPage : BasePage<BookingViewModel>
    {
        private BookingViewModel _viewModel => BindingContext as BookingViewModel;

        public BookingPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            _viewModel.Close();
            // block back button 
            return true;
        }
    }
}