using OnDijon.Common.Views;
using OnDijon.Modules.Library.ViewModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Library.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogSearchPage : BasePage<CatalogSearchViewModel>
    {
        //private int marginScrollAcualizerValue = 300;
        //private bool scrollableVisibilityVisible = false;
        public CatalogSearchPage()
        {
            InitializeComponent();
        }

        //private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        //{
        //    ScrollView scrollview = ((ScrollView)sender);
        //    if ((e.ScrollY + marginScrollAcualizerValue) >= (scrollview.ContentSize.Height - scrollview.Height))
        //    {
        //        ViewModel.LoadMoreResourcesCommand.Execute(true);
        //    }


        //    if (!scrollableVisibilityVisible && e.ScrollY > 40)
        //    {
        //        scrollableVisibilityVisible = true;
        //        scrollableVisibility.TranslateTo(-50, -20, 200);
        //    }
        //    else if (scrollableVisibilityVisible && e.ScrollY < 40)
        //    {
        //        scrollableVisibilityVisible = false;
        //        scrollableVisibility.TranslateTo(0, -20, 200);
        //    }
        //}
        //private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    ScrollViewPage.ScrollToAsync(0, 0, true);
        //}
    }
}