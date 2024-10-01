using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace OnDijon.Modules.School.Views
{
    public partial class PopupRestaurantView : PopupPage
    {
        public PopupRestaurantView(string sessionScheduleHelper)
        {
            InitializeComponent();
            Content.Text = sessionScheduleHelper;
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