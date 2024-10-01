using OnDijon.Modules.Dashboard.ViewModels;
using Xamarin.Forms;
using OnDijon.Common.Views;

namespace OnDijon.Modules.Dashboard.Pages
{
    public partial class DashboardView : BasePage<DashboardViewModel>
    {
        public DashboardView()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            //retour arrière bloqué
            return true;
        }

        bool scrollableVisibilityVisible = true;

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (scrollableVisibilityVisible && e.ScrollY > 40)
            {
                scrollableVisibilityVisible = false;
                scrollableVisibility.FadeTo(0f, 200);
            }
            else if (!scrollableVisibilityVisible && e.ScrollY < 40)
            {
                scrollableVisibilityVisible = true;
                scrollableVisibility.FadeTo(1.0f, 200);
            }
        }
    }
}