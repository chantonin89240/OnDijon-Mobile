using System;
using System.Linq;
using OnDijon.Modules.Services.ViewModels;
using OnDijon.Common.Utils.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScopesView : BaseView
    {
        private ServicesViewModel _servicesViewModel => BindingContext as ServicesViewModel;


        public ScopesView()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void Frame_TappedToCheck(object sender, EventArgs e)
        {
            CheckBox checkbox = ElementFinder.GetChildren<CheckBox>((Element)sender).FirstOrDefault();
            if (checkbox != null)
            {
                checkbox.IsChecked = !checkbox.IsChecked;
            }
        }

    }
}