using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Favorites.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopupFavoritePage : PopupPage
    {
		public PopupFavoritePage ()
		{
            //FavoriteModel favorite
            InitializeComponent();
		}

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return !CloseWhenBackgroundIsClicked;
        }
    }
}