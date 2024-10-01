using System;
using System.Collections.Generic;
using OnDijon.Modules.Services.ViewModels;
using OnDijon.Common.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OnDijon.Modules.Services.Pages;

namespace OnDijon.Modules.Service.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // Refacto tout cette logique a été faite en binding 
    public partial class ServicesView : BaseView
    {
        public ServicesView()
        {
            InitializeComponent();
        }
    }
}