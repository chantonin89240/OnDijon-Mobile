
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BadgeView : Frame
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(BadgeView), propertyChanged: TextPropertyChanged);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeView), propertyChanged: TextColorPropertyChanged);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public BadgeView()
        {
            InitializeComponent();
        }

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (BadgeView)bindable;
            var text = newValue?.ToString() ?? "";
            view.Label.Text = text;

            //set style according to text length to show a perfectly circular badge for single character text
            var styleName = text.Length > 1 ? "Badge" : "CircularBadge";
            view.Style = (Style)Application.Current.Resources[styleName];
        }

        private static void TextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (BadgeView)bindable;
            view.Label.TextColor = (Color)newValue;
        }
    }
}