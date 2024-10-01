using OnDijon.Common.Views;
using OnDijon.Modules.OnBoarding.ViewModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.OnBoarding.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnBoardingPage : BasePage<OnBoardingViewModel>
    {
        public OnBoardingPage()
        {
            InitializeComponent();
        }

    }

    
}