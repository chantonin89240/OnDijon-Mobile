using System;
using OnDijon.Modules.Library.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace OnDijon.Modules.Library.Views
{
    public partial class PopupAddCardView : PopupPage
    {
	    private AssociateReaderAccountViewModel _viewModel;

        public PopupAddCardView(AssociateReaderAccountViewModel associateReaderAccountViewModel)
        {
	        _viewModel = associateReaderAccountViewModel;
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