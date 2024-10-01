using System;
using OnDijon.Common.Utils.Tools;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace OnDijon.Modules.School.Views
{
    public partial class PopupMenuView : PopupPage
    {
        public PopupMenuView()
        {
            Init();
        }
        private void Init()
        {
            InitializeComponent();
            PorkIcon.Source = ImageTool.FromUri(Common.Utils.Resources.DMResources.SchoolRestaurantCalendar_Pork_Active_Icon);
            LocalIcon.Source = ImageTool.FromUri(Common.Utils.Resources.DMResources.SchoolRestaurantCalendar_Local_Active_Icon);
            BioIcon.Source = ImageTool.FromUri(Common.Utils.Resources.DMResources.SchoolRestaurantCalendar_Bio_Active_Icon);
            FairTradeIcon.Source = ImageTool.FromUri(Common.Utils.Resources.DMResources.SchoolRestaurantCalendar_FairTrade_Active_Icon);
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