using System;
using OnDijon.Modules.RoadworkInformation.ViewModels;
using Prism.Navigation;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace OnDijon.Modules.RoadworkInformation.Views
{
    public partial class RoadworkPopupView : PopupPage
    {
        private RoadworkInformationViewModel _viewModel;

      
        
        public RoadworkPopupView(RoadworkInformationViewModel viewmodel, string idRoadwork)
        {
            BindingContext = _viewModel = viewmodel;
            Init();
            _viewModel.SelectRoadwork(idRoadwork);
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