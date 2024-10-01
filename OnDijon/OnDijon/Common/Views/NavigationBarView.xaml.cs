using System;
using System.Windows.Input;
using Prism.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationBarView : StackLayout
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NavigationBarView), propertyChanged: TitlePropertyChanged);
        public static readonly BindableProperty IsRightButtonVisibileProperty = BindableProperty.Create(nameof(IsRightButtonVisibile), typeof(bool), typeof(NavigationBarView), propertyChanged: IsRightButtonVisiblePropertyChanged);
        public static readonly BindableProperty RightButtonCommandProperty = BindableProperty.Create(nameof(RightButtonCommand), typeof(ICommand), typeof(NavigationBarView), propertyChanged: RighButtonCommandPropertyChanged);
        public static readonly BindableProperty RightIconButtonProperty = BindableProperty.Create(nameof(RightIconButton), typeof(string), typeof(NavigationBarView), propertyChanged: RightIconButtonPropertyChanged);
        public static readonly BindableProperty RightButtonCommandParameterProperty = BindableProperty.Create(nameof(RightButtonCommandParameter), typeof(bool), typeof(NavigationBarView), propertyChanged: RightButtonCommandParameterPropertyChanged);
        public static readonly BindableProperty HasBackButtonProperty = BindableProperty.Create(nameof(HasBackButton), typeof(bool), typeof(NavigationBarView), defaultValue: true, propertyChanged: HasBackButtonPropertyChanged);

        public static readonly BindableProperty BackButtonCommandProperty = BindableProperty.Create(nameof(BackButtonCommand), typeof(ICommand), typeof(NavigationBarView), propertyChanged: BackButtonCommandPropertyChanged);
        public static readonly BindableProperty BackButtonCommandParameterProperty = BindableProperty.Create(nameof(BackButtonCommandParameter), typeof(object), typeof(NavigationBarView), propertyChanged: BackButtonCommandParameterPropertyChanged);


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public bool IsRightButtonVisibile
        {
            get { return (bool)GetValue(IsRightButtonVisibileProperty); }
            set { SetValue(IsRightButtonVisibileProperty, value); }
        }

        public bool HasBackButton
        {
            get { return (bool)GetValue(HasBackButtonProperty); }
            set { SetValue(HasBackButtonProperty, value); }
        }


        public ICommand RightButtonCommand
        {
            get { return (ICommand)GetValue(RightButtonCommandProperty); }
            set { SetValue(RightButtonCommandProperty, value); }
        }

        public string RightIconButton
        {
            get { return (string)GetValue(RightIconButtonProperty); }
            set { SetValue(RightIconButtonProperty, value); }
        }


        public ICommand BackButtonCommand
        {
            get { return (ICommand)GetValue(BackButtonCommandProperty); }
            set { SetValue(BackButtonCommandProperty, value); }
        }


        public bool RightButtonCommandParameter
        {
            get { return (bool)GetValue(RightButtonCommandParameterProperty); }
            set { SetValue(RightButtonCommandParameterProperty, value); }
        }

        public object BackButtonCommandParameter
        {
            get { return GetValue(BackButtonCommandParameterProperty); }
            set { SetValue(BackButtonCommandParameterProperty, value); }
        }


        public event EventHandler NavBarBackButtonPressed;

        public NavigationBarView()
        {
            InitializeComponent();

            UpdateBackButton(HasBackButton);

            // Display popup by default
            RightButtonCommandParameter = true;
            TitleLabel.Text = Title;

            ImageButton.IsVisible = IsRightButtonVisibile;
            if (RightButtonCommand != null)
            {
                ImageButton.Command = RightButtonCommand;
                ImageButton.CommandParameter = RightButtonCommandParameter;
            }

        }




        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationBarView)bindable;

            view.TitleLabel.Text = (string)newValue;
        }


        private static void IsRightButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationBarView)bindable;

            view.ImageButton.IsVisible = (bool)newValue;
        }
        private static void RighButtonCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationBarView)bindable;

            view.ImageButton.Command = (ICommand)newValue;
        }


        private static void BackButtonCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationBarView)bindable;

            view.BackButton.Command = (ICommand)newValue;
        }


        private static void RightIconButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationBarView)bindable;

            view.RightIconButton = newValue.ToString();
            view.ImageButton.Source = new FontImageSource
            {
                Glyph = view.RightIconButton,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["MaterialDesignIcons"],
                Size = 24,
                Color = (Color)Application.Current.Resources["kleinBlue"]
            };
        }



        private static void RightButtonCommandParameterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationBarView)bindable;

            view.RightButtonCommandParameter = (bool)newValue;
            view.ImageButton.CommandParameter = view.RightButtonCommandParameter;
        }

        private static void BackButtonCommandParameterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationBarView)bindable;
            view.BackButtonCommandParameter = newValue;
            view.BackButton.CommandParameter = view.BackButtonCommandParameter;
        }


        private static void HasBackButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (NavigationBarView)bindable;

            view.UpdateBackButton((bool)newValue);
        }


        void UpdateBackButton(bool isVisible)
        {
            HasBackButton = isVisible;
            BackButton.IsVisible = HasBackButton;

            BackButton.Command = new DelegateCommand(() => {  
                Application.Current.MainPage.Navigation.PopAsync(); 
            });
        }
    }
}