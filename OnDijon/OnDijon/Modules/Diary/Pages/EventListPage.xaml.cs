using OnDijon.Common.Views;
using OnDijon.Modules.Diary.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Diary.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventListPage : BasePage<EventDiaryListViewModel>
    {

        private int marginScrollAcualizerValue = 300;

        public EventListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ScrollViewPage.ScrollToAsync(0,0,false);
        }
        protected override bool OnBackButtonPressed()
        {
            return ViewModel.GoBack();
        }

        bool scrollableVisibilityVisible = false;

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollview = ((ScrollView)sender);
            if ((e.ScrollY + marginScrollAcualizerValue) >= (scrollview.ContentSize.Height - scrollview.Height))
            {
                ViewModel.LoadMoreEvents();
            }


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