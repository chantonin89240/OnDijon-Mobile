using OnDijon.Common.Views;
using OnDijon.Modules.WedAlsh.Entities.Models;
using OnDijon.Modules.WedAlsh.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static OnDijon.Modules.WedAlsh.ViewModels.WedAlshViewModel;

namespace OnDijon.Modules.WedAlsh.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WedAlshMainPage : BasePage<WedAlshViewModel>
    {
        public WedAlshMainPage()
        {
            InitializeComponent();
        }



    }
}