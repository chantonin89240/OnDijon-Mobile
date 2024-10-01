
using OnDijon.Common.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Web.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPage : BaseView
    {
        public WebPage()
        {
            InitializeComponent();
           // BindingContext = App.Locator.WebViewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await progress.ProgressTo(0.9, 900, Easing.SpringIn);

        }

        protected void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            progress.IsVisible = true;
        }

        protected void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            progress.IsVisible = false;
        }

        protected override void OnDisappearing()
        {
            //Webview.Source = "about:blank";

            base.OnDisappearing();

        }

        //protected override bool OnBackButtonPressed()
        //{
        //    //var a = new WebView()
        //    //{
        //    //    Source = Constants.INTERNAL_BM_URL + "?logOff=dijon"
        //    //};
        //    a.Reload();
        //    Webview.Source = "about:blank";
        //    Webview.Reload();
        //    return base.OnBackButtonPressed();
        //}
    }
}