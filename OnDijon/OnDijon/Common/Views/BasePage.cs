using OnDijon.Common.ViewModels;
using OnDijon.Common.Utils.Tools;
using System;
using System.Linq;
using Xamarin.Forms;

namespace OnDijon.Common.Views
{
    public class BasePage<T> : ContentPage where T : BaseViewModel
    {

	    protected T ViewModel => BindingContext as T;
        public BasePage()
        {
	        this.BackgroundColor = Color.FromHex("#1A3972");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var topBarView = this.FindByName<TopBarView>("TopBarView");
            topBarView?.OnAppearing();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public void Frame_TappedToCheck(object sender, EventArgs e)
        {
            CheckBox checkbox = ElementFinder.GetChildren<CheckBox>((Element)sender).FirstOrDefault();
            if (checkbox != null)
            {
                checkbox.IsChecked = !checkbox.IsChecked;
            }
        }
    }
}
