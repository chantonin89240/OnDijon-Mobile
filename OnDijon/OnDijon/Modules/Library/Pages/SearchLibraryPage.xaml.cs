using OnDijon.Common.Utils;
using OnDijon.Common.Views;
using OnDijon.Modules.Library.ViewModels;
using Prism.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Library.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchLibraryPage : BaseView
    {

        private SearchLibraryViewModel ViewModel => BindingContext as SearchLibraryViewModel;
        public SearchLibraryPage()
        {
            InitializeComponent();
            // TODO : pas de modif à faire 
            NavigationBarView.BackButtonCommand = new DelegateCommand(() =>
            {

                Webview.GoBack();

            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Webview.IsEnabled = false;
            Webview.Source = Constants.INTERNAL_BM_LOGOFF_URL;
            IsOnLogoff = true;

        }
        
        protected void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url == Constants.INTERNAL_BM_URL)
                e.Cancel = true;
           //LoadingView.IsLoading = true;
        }

        private bool IsOnLogoff = true;
        
        protected void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            if (IsOnLogoff)
            {
                Webview.Source = ViewModel.Url;
                IsOnLogoff = false;
            }
            else
            {
                Webview.IsEnabled = true;
                //LoadingView.IsLoading = false;
            }
        }


    }
}