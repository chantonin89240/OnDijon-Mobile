using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace OnDijon.Modules.WedAlsh.Views
{
    public partial class PopupInfoWedAlshView : PopupPage
    {
        public PopupInfoWedAlshView()
        {
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