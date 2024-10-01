using OnDijon.Modules.OnBoarding.ViewModels;
using Xamarin.Forms;

namespace OnDijon.Modules.OnBoarding.Tools
{
    public class TemplateSelector : DataTemplateSelector
    {
        public DataTemplate Onboarding1 { get; set; }
        public DataTemplate Onboarding2 { get; set; }
        public DataTemplate Note { get; set; }
        public DataTemplate Login { get; set; }
        public DataTemplate GoOnDashboard { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            switch (((Slide)item).SlideId)
            {
                case 1:
                    return Onboarding1;
                case 2:
                    return Onboarding2;
                case 3:
                    return Note;
                case 4:
                    return Login;
                default:
                    return GoOnDashboard;
            };
        }
    }
}
