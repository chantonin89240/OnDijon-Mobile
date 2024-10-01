using System.Windows.Input;
using OnDijon.Common.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TopBarView : ContentView
    {
        private readonly TopBarViewModel _topBarViewModel = App.Locator.TopBarViewModel;

        public static readonly BindableProperty ExitCommandProperty = BindableProperty.Create(nameof(ExitCommand), typeof(ICommand), typeof(NavigationBarView), propertyChanged: ExitPropertyChanged);
        public static readonly BindableProperty IsCloseButtonVisibleProperty = BindableProperty.Create(nameof(IsCloseButtonVisible), typeof(bool), typeof(TopBarView), propertyChanged: IsCloseButtonVisiblePropertyChanged);
        public static readonly BindableProperty IsNotificationButtonVisibleProperty = BindableProperty.Create(nameof(IsNotificationButtonVisible), typeof(bool), typeof(TopBarView), propertyChanged: IsNotificationButtonVisiblePropertyChanged);

        public ICommand ExitCommand
        {
            get => (ICommand)GetValue(ExitCommandProperty);
            set => SetValue(ExitCommandProperty, value);
        }

        public bool IsCloseButtonVisible
        {
            get => (bool)GetValue(IsCloseButtonVisibleProperty);
            set => SetValue(IsCloseButtonVisibleProperty, value);
        }

        public bool IsNotificationButtonVisible
        {
            get => (bool)GetValue(IsNotificationButtonVisibleProperty);
            set => SetValue(IsNotificationButtonVisibleProperty, value);
        }

        public TopBarView()
        {
            InitializeComponent();

            IsCloseButtonVisiblePropertyChanged(this, null, IsCloseButtonVisible);
            IsNotificationButtonVisible = true;
            IsNotificationButtonVisiblePropertyChanged(this, null, IsNotificationButtonVisible);

            BindingContext = _topBarViewModel;
            _topBarViewModel.InitializeCommands();
            _topBarViewModel.ExitAddedCommand = ExitCommand;

        }

        public void OnAppearing()
        {
            if (_topBarViewModel.IsConnected)
            {
                _topBarViewModel.GetNotificationCount();
            }
        }

        private static void IsCloseButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TopBarView view = (TopBarView)bindable;

            view.CloseButton.IsVisible = (bool)newValue;
            view.OpenMenuButton.IsVisible = !(bool)newValue;
        }

        private static void IsNotificationButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TopBarView view = (TopBarView)bindable;

            view.NotificationContainer.IsVisible = (bool)newValue;
        }

        private static void ExitPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            App.Locator.TopBarViewModel.ExitAddedCommand = (ICommand)newValue;
        }
    }
}