using System;
using OnDijon.Modules.Account.ViewModels;
using OnDijon.Common.Views;

namespace OnDijon.Modules.Account.Pages
{
    public partial class ChangeProfileInfoView : BaseView
    {

	    public ChangeProfileInfoView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
	        // Refacto : Moved to ViewModel OnNavigatedToAsync
            //_changeProfileInfoVM.GetAccount();

            //password fields are empty by default
            OldPassword.Text = "";
            NewPassword.Text = "";
            NewPasswordConfirmation.Text = "";
        }

        // refacto changed to GenderSelectionChangedCommand in ViewModel
    //     void OnGenderRadioButtonCheckedChanged(object sender, EventArgs e)
    //     {
	   //      
	   //      var vm = BindingContext as ChangeProfileInfoViewModel;
	   //      if (vm?.Profile != null)
				// vm.Profile.Gender.Value = GenderRadioButtonGroup.SelectedItem as string;
    //     }
    }
}
