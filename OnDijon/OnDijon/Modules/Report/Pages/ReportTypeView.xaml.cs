using System;
using System.Collections.Generic;
using System.ComponentModel;
using FFImageLoading.Transformations;
using OnDijon.Modules.Report.Entities.Dto;
using OnDijon.Common.Utils.Enums;
using OnDijon.Modules.Report.ViewModels;
using OnDijon.Common.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OnDijon.Modules.Services.Pages;

namespace OnDijon.Modules.Report.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportTypeView : BasePage<ReportTypeViewModel>
    {
        public ReportTypeView()
        {
            InitializeComponent();
        }
        
        protected override bool OnBackButtonPressed()
        {
            ViewModel?.CloseCommand.Execute(true);
            // block back button 
            return true;
        }
    }
}
