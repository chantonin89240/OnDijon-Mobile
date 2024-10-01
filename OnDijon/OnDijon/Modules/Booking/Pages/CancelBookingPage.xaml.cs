using OnDijon.Common.Views;
using OnDijon.Modules.Booking.ViewModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Booking.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CancelBookingPage : BasePage<CancelBookingViewModel>
    {
        public CancelBookingPage()
        {
            InitializeComponent();
        }

        //public CancelBookingPage(DemandModel param)
        //{
        //    InitializeComponent();
        //    //App.Locator.CancelBooking.Demand = param;
        //}

    }
}