using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomLabel : StackLayout
    {
        public static readonly BindableProperty CustomStyleProperty = BindableProperty.Create(nameof(CustomStyle), typeof(Style), typeof(CustomLabel));
        public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(CustomLabel));
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomLabel));

        public Style CustomStyle
        {
            get { return (Style)GetValue(CustomStyleProperty); }
            set { SetValue(CustomStyleProperty, value); }
        }

        public bool IsRequired
        {
            get { return (bool)GetValue(IsRequiredProperty); }
            set { SetValue(IsRequiredProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public CustomLabel()
        {
            InitializeComponent();
            BindingContext = this;
        }

    }
}