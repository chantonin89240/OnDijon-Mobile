using System;
using OnDijon.Modules.UsefulContact.ViewsModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace OnDijon.Modules.UsefulContact.Views
{
    public partial class PopupInfoContactView : PopupPage
    {

        public PopupInfoContactView(ContactMapViewModel contactMapViewModel)
        {
	        BindingContext = contactMapViewModel;
            Init();
        }

        private void Init()
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