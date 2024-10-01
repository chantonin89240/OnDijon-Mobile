
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwitchIconView : Frame
    {

        public static readonly BindableProperty LeftIconProperty = BindableProperty.Create(nameof(LeftIcon), typeof(string), typeof(SwitchIconView), propertyChanged: LeftIconPropertyChanged);
        public static readonly BindableProperty RightIconProperty = BindableProperty.Create(nameof(RightIcon), typeof(string), typeof(SwitchIconView), propertyChanged: RightIconPropertyChanged);
        public static readonly BindableProperty IsRightSelectedProperty = BindableProperty.Create(nameof(IsRightSelected), typeof(bool), typeof(SwitchIconView), defaultValue: true, defaultBindingMode: BindingMode.TwoWay, propertyChanged: IsRightSelectedPropertyChanged);

        public string LeftIcon
        {
            get { return (string)GetValue(LeftIconProperty); }
            set { SetValue(LeftIconProperty, value); }
        }

        public string RightIcon
        {
            get { return (string)GetValue(RightIconProperty); }
            set { SetValue(RightIconProperty, value); }
        }

        public bool IsRightSelected
        {
            get { return (bool)GetValue(IsRightSelectedProperty); }
            set { SetValueCore(IsRightSelectedProperty, value, SetValueFlags.RaiseOnEqual); }
        }

        public event EventHandler<ToggledEventArgs> Toggled;

        public SwitchIconView()
        {
            InitializeComponent();

            // Because corner radius doesn't work in the "SwitchIconView" style,
            // you have to put the corner radius here
            RightButton.CornerRadius = 15;
            LeftButton.CornerRadius = 15;
        }



        void OnLeftTapped(object sender, EventArgs args)
        {
            //if (!IsRightSelected)
            //{
            //    SelectButton(IsRightSelected);
            //}
            IsRightSelected = false;
            Toggled?.Invoke(this, new ToggledEventArgs(false));
        }

        void OnRightTapped(object sender, EventArgs args)
        {
            //if (IsRightSelected)
            //{
            //    SelectButton(IsRightSelected);
            //}
            IsRightSelected = true;
            Toggled?.Invoke(this, new ToggledEventArgs(true));
        }


        private void SelectButton(bool isRight)
        {
            var selected = isRight ? RightButton : LeftButton;
            VisualStateManager.GoToState(selected, VisualStateManager.CommonStates.Selected);

            var normal = isRight ? LeftButton : RightButton;
            VisualStateManager.GoToState(normal, VisualStateManager.CommonStates.Normal);
        }

        private static void LeftIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (SwitchIconView)bindable;
            view.SetImage(view.LeftButton, newValue?.ToString());
        }

        private static void RightIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (SwitchIconView)bindable;
            view.SetImage(view.RightButton, newValue?.ToString());
        }

        private static void IsRightSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (SwitchIconView)bindable;
            view.SelectButton((bool)newValue);
        }


        private void SetImage(ImageButton imageButton, string icon)
        {
            imageButton.Source = new FontImageSource
            {
                Glyph = icon,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["MaterialDesignIcons"],
                Size = 24,
                Color = Color.White
            };
        }


    }
}