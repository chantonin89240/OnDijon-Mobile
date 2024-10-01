
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ContentProperty(nameof(ChildView))]
    public partial class LoadingView : AbsoluteLayout
    {
        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(LoadingView), defaultValue: false, propertyChanged: IsLoadingPropertyChanged);
        public static readonly BindableProperty LoadingIndicatorColorProperty = BindableProperty.Create(nameof(LoadingIndicatorColor), typeof(Color), typeof(LoadingView), propertyChanged: LoadingIndicatorColorPropertyChanged);
        public static readonly BindableProperty ChildViewProperty = BindableProperty.Create(nameof(ChildView), typeof(View), typeof(LoadingView), propertyChanged: ChildViewPropertyChanged);

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public Color LoadingIndicatorColor
        {
            get { return (Color)GetValue(LoadingIndicatorColorProperty); }
            set { SetValue(LoadingIndicatorColorProperty, value); }
        }

        public View ChildView
        {
            get { return (View)GetValue(ChildViewProperty); }
            set { SetValue(ChildViewProperty, value); }
        }

        public LoadingView()
        {
            InitializeComponent();
        }

        private static void IsLoadingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (LoadingView)bindable;
            view.LoadingIndicator.IsRunning = (bool)newValue;

            //disable child view inputs when loading
            view.ChildContainer.InputTransparent = (bool)newValue;
        }

        private static void LoadingIndicatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (LoadingView)bindable;
            view.LoadingIndicator.Color = (Color)newValue;
        }

        private static void ChildViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (LoadingView)bindable;
            view.ChildContainer.Content = (View)newValue;
        }
    }
}