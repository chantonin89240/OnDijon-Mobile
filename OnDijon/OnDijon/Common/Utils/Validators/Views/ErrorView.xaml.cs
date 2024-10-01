using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Utils.Validators.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorView : ContentView
    {
        public static readonly BindableProperty ErrorStateProperty =
            BindableProperty.Create(nameof(ErrorState), typeof(ErrorState),
                typeof(ErrorView), null, BindingMode.OneWay,
                null, null, null, null, null);


        public ErrorState ErrorState
        {
            get { return (ErrorState)GetValue(ErrorStateProperty); }
            set
            {
                SetValue(ErrorStateProperty, value);
            }
           
        }

        public ErrorView()
        {
            InitializeComponent();
        }
    }
}