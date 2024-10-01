
using System.Collections.Generic;
using System.Linq;
using OnDijon.Common.Utils.Fonts;
using OnDijon.Common.Utils.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ContentProperty(nameof(ChildView))]
    public abstract partial class FormBaseView : StackLayout
    {
        public static readonly BindableProperty ErrorsProperty = BindableProperty.Create(nameof(Errors), typeof(IList<string>), typeof(FormBaseView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: ErrorsPropertyChanged);
        public static readonly BindableProperty ChildViewProperty = BindableProperty.Create(nameof(ChildView), typeof(View), typeof(FormBaseView), propertyChanged: ChildViewPropertyChanged);



        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        private Color _borderColor;
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                OnPropertyChanged(nameof(BorderColor));
            }
        }


        private bool _isErrorVisible;
        public bool IsErrorVisibile
        {
            get { return _isErrorVisible; }
            set
            {
                _isErrorVisible = value;
                OnPropertyChanged(nameof(IsErrorVisibile));
            }
        }

        public IList<string> Errors
        {
            get { return (IList<string>)GetValue(ErrorsProperty); }
            set { SetValue(ErrorsProperty, value); }
        }

        public View ChildView
        {
            get { return (View)GetValue(ChildViewProperty); }
            set { SetValue(ChildViewProperty, value); }
        }



        protected FormBaseView()
        {
            InitializeComponent();

            BorderColor = (Color)App.Current.Resources["BorderColor"];
        }

        private static void ChildViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormBaseView)bindable;
            view.CreateSubviews((View)newValue);
        }


        private void CreateSubviews(View contentChild)
        {
            this.Children.Clear();

            Color errorColor = (Color)Application.Current.Resources["ErrorColor"];

            // Frame
            Frame frame = new Frame { BindingContext = this, Style = (Style)Application.Current.Resources["FormBaseStyle"] };
            frame.SetBinding(Frame.BorderColorProperty, new Binding(nameof(BorderColor)));
            frame.Content = contentChild;

            // Error label
            Label errorLabel = new Label { BindingContext = this, VerticalOptions = LayoutOptions.Center, TextColor = errorColor };
            errorLabel.SetBinding(Label.TextProperty, new Binding(nameof(ErrorMessage)));

            // Error image
            var source = new FontImageSource
            {
                Glyph = MaterialDesignIcons.Alert,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["MaterialDesignIcons"],
                Size = 20,
                Color = errorColor
            };
            Image image = new Image { Source = source, WidthRequest = 14, HeightRequest = 14 };

            // Error StackLayout
            StackLayout errorStackLayout = new StackLayout { BindingContext = this, Orientation = StackOrientation.Horizontal };
            errorStackLayout.Children.Add(image);
            errorStackLayout.Children.Add(errorLabel);
            errorStackLayout.SetBinding(StackLayout.IsVisibleProperty, new Binding(nameof(IsErrorVisibile)));

            StackLayout stack = new StackLayout { Orientation = StackOrientation.Vertical };

            stack.Children.Add(frame);
            stack.Children.Add(errorStackLayout);

            this.Children.Add(stack);

        }

        private static void ErrorsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormBaseView)bindable;
            var errors = (IList<string>)newValue;
            if (errors != null && errors.Any())
            {
                view.IsErrorVisibile = true;
                view.ErrorMessage = errors.First();
                view.BorderColor = (Color)App.Current.Resources["ErrorColor"];

                var scrollView = ViewHelper.SearchScrollView(view) as ScrollView;
                if (scrollView != null)
                {
                    // scroll to error form
                    scrollView.ScrollToAsync(scrollView.X, view.Y, false);
                }
            }
            else
            {
                view.IsErrorVisibile = false;
                view.ErrorMessage = string.Empty;
                view.BorderColor = (Color)App.Current.Resources["BorderColor"];
            }
        }




    }
}