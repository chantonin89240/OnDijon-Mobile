using OnDijon.Common.Views;
using OnDijon.Modules.Diary.ViewModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Diary.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventDetailsPage : BasePage<EventDetailDiaryViewModel>
    {

        public EventDetailsPage()
        {
            InitializeComponent();
        }
    }
}