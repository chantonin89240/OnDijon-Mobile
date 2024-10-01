using OnDijon.Common.Views;
using OnDijon.Modules.JobOffer.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.JobOffer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListJobOfferPage : BasePage<ListJobOfferViewModel>
    {
        public ListJobOfferPage()
        {
            InitializeComponent();
        }

        bool scrollableVisibilityVisible = false;
        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollview = ((ScrollView)sender);


            if (!scrollableVisibilityVisible && e.ScrollY > 40)
            {
                scrollableVisibilityVisible = true;
                scrollableVisibility.TranslateTo(-50, -20, 200);
            }
            else if (scrollableVisibilityVisible && e.ScrollY < 40)
            {
                scrollableVisibilityVisible = false;
                scrollableVisibility.TranslateTo(0, -20, 200);
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ScrollViewPage.ScrollToAsync(0, 0, true);
        }

    }
}