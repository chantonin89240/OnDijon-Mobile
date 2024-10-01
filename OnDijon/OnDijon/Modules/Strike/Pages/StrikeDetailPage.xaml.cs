using OnDijon.Common.Views;
using OnDijon.Modules.Strike.ViewModels;
using System;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Strike.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StrikeDetailPage : BasePage<StrikeDetailViewModel>
    {
        public StrikeDetailPage()
        {
            InitializeComponent();
        }

        private void UnfocusSearchBar(object sender, EventArgs e)
        {
            this.SearchBarCustom.HideDisplayOnUnfocus = true;
            ScrollViewPage.ScrollToAsync(0, 0, true);
        }
    }
}