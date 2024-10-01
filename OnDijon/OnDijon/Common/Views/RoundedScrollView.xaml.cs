using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoundedScrollView : ContentView
    {
        public static readonly BindableProperty HeaderViewProperty = BindableProperty.Create(nameof(HeaderView), typeof(View), typeof(RoundedScrollView), propertyChanged: HeaderViewPropertyChanged);
        public static readonly BindableProperty ContentViewProperty = BindableProperty.Create(nameof(ContentView), typeof(View), typeof(RoundedScrollView), propertyChanged: ContentViewPropertyChanged);

        public View ContentView
        {
            get { return (View)GetValue(ContentViewProperty); }
            set { SetValue(ContentViewProperty, value); }
        }
        public View HeaderView
        {
            get { return (View)GetValue(HeaderViewProperty); }
            set { SetValue(HeaderViewProperty, value); }
        }

        public RoundedScrollView()
        {
            InitializeComponent();
        }

        private static void ContentViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (RoundedScrollView)bindable;
            view.ContentContainer.Children.Clear();
            view.ContentContainer.Children.Add((View)newValue);
        }
        private static void HeaderViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (RoundedScrollView)bindable;
            view.HeaderContainer.Content = (View)newValue;
        }
    }
}