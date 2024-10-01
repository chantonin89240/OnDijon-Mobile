using System;
using OnDijon.Common.Extensions;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationCardView : Frame
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(NavigationCardView), propertyChanged: TextPropertyChanged);
        public static readonly BindableProperty PageKeyProperty = BindableProperty.Create(nameof(PageKey), typeof(string), typeof(NavigationCardView), propertyChanged: PageKeyPropertyChanged);

        public NavigationCardView()
        {
	        this.InitializeComponent();
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string PageKey
        {
            get { return (string)GetValue(PageKeyProperty); }
            set { SetValue(PageKeyProperty, value); }
        }

        private INavigationService _navigationService => App.CurrentNavigationService;
        

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationCardView)bindable;
            view.Label.Text = newValue?.ToString();
        }

        private static void PageKeyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationCardView)bindable;
            view.PageKey = newValue?.ToString();
        }

        void OnTapped(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(PageKey))
            {
                _navigationService.NavigateTo(PageKey);
            }
        }
    }
}