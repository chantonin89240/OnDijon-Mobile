using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HyperlinkView : StackLayout
    {
        public static readonly BindableProperty UrlProperty = BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkView), null, defaultBindingMode: BindingMode.TwoWay);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(HyperlinkView), null, defaultBindingMode: BindingMode.TwoWay, propertyChanged: TextPropertyChanged);
        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public HyperlinkView()
        {
            InitializeComponent();
            hyperlinkLabel.TextDecorations = TextDecorations.Underline;
            hyperlinkLabel.Text = Text;
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                // Launcher.OpenAsync is provided by Xamarin.Essentials.
                Command = new Command(async () => await Launcher.OpenAsync(Url))
            });
        }
        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (HyperlinkView)bindable;
            view.hyperlinkLabel.Text = newValue.ToString();

        }
    }
}