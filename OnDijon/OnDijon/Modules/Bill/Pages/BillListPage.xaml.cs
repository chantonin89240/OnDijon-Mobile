using OnDijon.Common.Views;
using OnDijon.Modules.Bill.ViewModels;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System;

namespace OnDijon.Modules.Bill.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BillListPage : BasePage<BillListViewModel>
    {
        public BillListPage()
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