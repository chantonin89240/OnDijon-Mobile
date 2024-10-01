
using OnDijon.Modules.Report.ViewModels;
using OnDijon.Common.Views;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Report.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportSummaryView : BasePage<ReportSummaryViewModel>
    {
        public ReportSummaryView()
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