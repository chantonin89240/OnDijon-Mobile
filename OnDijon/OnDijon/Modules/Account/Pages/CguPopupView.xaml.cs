using OnDijon.Common.Entities;
using OnDijon.Modules.Account.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace OnDijon.Modules.Account.Pages
{
    public partial class CguPopupView : PopupPage
    {
        private CguPopupViewModel _vm = App.Locator.GetInstance<CguPopupViewModel>();
        public CguPopupView()
        {
            InitializeComponent();
            BindingContext = _vm;
        }

        public CguPopupView(IPopupViewSettings settings)
            : this()
        {
            _vm.UpdateSettings(settings);
        }

        protected override void OnAppearing()
        {
            (BindingContext as CguPopupViewModel).QueryCgu();
        }
    }
}