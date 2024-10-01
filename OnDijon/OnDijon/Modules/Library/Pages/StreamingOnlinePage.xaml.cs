using OnDijon.Common.Views;
using OnDijon.Modules.Library.ViewModels;
using Xamarin.Forms.Xaml;

namespace OnDijon.Modules.Library.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StreamingOnlinePage : BasePage<StreamingOnlineViewModel>
    {
        public StreamingOnlinePage()
        {
            InitializeComponent();
        }
    }
}