using OnDijon.Modules.Library.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace OnDijon.Modules.Library.Views.Popup
{
    public partial class AccountPopupView : PopupPage
    {
        public AccountPopupView(CatalogDetailViewModel viewModel)
		{
			InitializeComponent();
			BindingContext = viewModel;
		}
    }
}