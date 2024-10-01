using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StarsNotationView : Grid
    {
        public static readonly BindableProperty StarsNumberProperty = BindableProperty.Create(nameof(StarsNumber), typeof(int), typeof(StarsNotationView), defaultValue: 0, defaultBindingMode: BindingMode.TwoWay, propertyChanged: StarsNumberChanged);
        public static readonly BindableProperty IsEvaluatedProperty = BindableProperty.Create(nameof(IsEvaluated), typeof(bool), typeof(StarsNotationView), defaultValue: false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: IsEvaluatedPropertyChanged);
        
        public int StarsNumber
        {
            get { return (int)GetValue(StarsNumberProperty); }
            set { SetValue(StarsNumberProperty, value); }
        }

        public bool IsEvaluated
        {
            get { return (bool)GetValue(IsEvaluatedProperty); }
            set { SetValue(IsEvaluatedProperty, value); }
        }

        public StarsNotationView()
        {
            InitializeComponent();
            SetStarsNumber(StarsNumber);
            Star0.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(0)) });
            Star1.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(1)) });
            Star2.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(2)) });
            Star3.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(3)) });
            Star4.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(4)) });
            Star5.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(5)) });
            Star6.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(6)) });
            Star7.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(7)) });
            Star8.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(8)) });
            Star9.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(9)) });
            Star10.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(i => SetStarsNumber(10)) });
        }

        public void SetStarsNumber(int number)
        {
            if (number < 0)
            {
                StarsNumber = 0;
            }
            else if (number > 10)
            {
                StarsNumber = 10;
            }
            else
            {
                StarsNumber = number;
            }
            IsEvaluated = StarsNumber != 0;
            VisualStateManager.GoToState(Star0, StarsNumber >= 0 ? "True" : "False");
            VisualStateManager.GoToState(Star1, StarsNumber >= 1 ? "True" : "False");
            VisualStateManager.GoToState(Star2, StarsNumber >= 2 ? "True" : "False");
            VisualStateManager.GoToState(Star3, StarsNumber >= 3 ? "True" : "False");
            VisualStateManager.GoToState(Star4, StarsNumber >= 4 ? "True" : "False");
            VisualStateManager.GoToState(Star5, StarsNumber >= 5 ? "True" : "False");
            VisualStateManager.GoToState(Star6, StarsNumber >= 6 ? "True" : "False");
            VisualStateManager.GoToState(Star7, StarsNumber >= 7 ? "True" : "False");
            VisualStateManager.GoToState(Star8, StarsNumber >= 8 ? "True" : "False");
            VisualStateManager.GoToState(Star9, StarsNumber >= 9 ? "True" : "False");
            VisualStateManager.GoToState(Star10, StarsNumber >= 10 ? "True" : "False");
        }


        private static void StarsNumberChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (StarsNotationView)bindable;
            view.SetStarsNumber((int)newValue);
        }

        private static void IsEvaluatedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (StarsNotationView)bindable;
        }

        private static void ShowLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (StarsNotationView)bindable;
        }
    }
}