using OnDijon.Modules.Rating.ViewModels;
using Rg.Plugins.Popup.Pages;
using OnDijon.Common.Entities;
using Rg.Plugins.Popup.Services;
using System;

namespace OnDijon.Modules.Rating.Views
{
    public partial class RatingPopupView : PopupPage
    {
	    private RatingViewModel _vm => BindingContext as RatingViewModel;
        public RatingPopupView(RatingViewModel ratingViewModel)
        {
            InitializeComponent();
            BindingContext = ratingViewModel;
        }

        public RatingPopupView(IPopupViewSettings settings, RatingViewModel ratingViewModel )
            : this(ratingViewModel)
        {
            _vm?.UpdateSettings(settings);
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnValidate(object sender, EventArgs e)
        {
	        _vm?.SendRating();
            await PopupNavigation.Instance.PopAsync();
        }
    }
}