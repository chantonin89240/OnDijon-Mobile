using OnDijon.Common.Utils.Enums;
using System;
using System.Windows.Input;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwitchView : StackLayout
    {
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(SwitchView), defaultValue: false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: IsSelectedPropertyChanged);
        public static readonly BindableProperty ShowLabelProperty = BindableProperty.Create(nameof(ShowLabel), typeof(bool), typeof(SwitchView), defaultValue: false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: ShowLabelPropertyChanged);
        public static readonly BindableProperty SelectedLabelProperty = BindableProperty.Create(nameof(SelectedLabel), typeof(string), typeof(SwitchView), propertyChanged: SelectedLabelPropertyChanged);
        public static readonly BindableProperty UnselectedLabelProperty = BindableProperty.Create(nameof(UnselectedLabel), typeof(string), typeof(SwitchView), propertyChanged: UnSelectedLabelPropertyChanged);
        public static readonly BindableProperty IsLockedProperty = BindableProperty.Create(nameof(IsLocked), typeof(bool), typeof(SwitchView), defaultValue: false, defaultBindingMode: BindingMode.TwoWay, propertyChanged: IsLockedPropertyChanged);
        public static readonly BindableProperty IsLockedLabelProperty = BindableProperty.Create(nameof(IsLockedLabel), typeof(string), typeof(SwitchView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: IsLockedLabelPropertyChanged);
        public static readonly BindableProperty OnChangeCommandProperty = BindableProperty.Create(nameof(OnChangeCommand), typeof(ICommand), typeof(NavigationBarView), propertyChanged: OnChangeCommandPropertyChanged);
        
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public bool ShowLabel
        {
            get { return (bool)GetValue(ShowLabelProperty); }
            set { SetValue(ShowLabelProperty, value); }
        }

        public string SelectedLabel
        {
            get { return (string)GetValue(SelectedLabelProperty); }
            set { SetValue(SelectedLabelProperty, value); }
        }

        public string UnselectedLabel
        {
            get { return (string)GetValue(UnselectedLabelProperty); }
            set { SetValue(UnselectedLabelProperty, value); }
        }

        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set {
                padlock.IsVisible = value;
                SetValue(IsLockedProperty, value); 
            }
        }

        public string IsLockedLabel
        {
            get { return (string)GetValue(IsLockedLabelProperty); }
            set { SetValue(IsLockedLabelProperty, value); }
        }

        public ICommand OnChangeCommand
        {
            get { return (ICommand)GetValue(OnChangeCommandProperty); }
            set { SetValue(OnChangeCommandProperty, value); }
        }

        public event EventHandler<ToggledEventArgs> Toggled;

        public SwitchView()
        {
            InitializeComponent();
            //this.Padding = new Thickness(2, 0);
            SelectButton(IsSelected);
            padlock.IsVisible = IsLocked;
        }

        private void SelectButton(bool isSelected)
        {
            FrameContainer.BackgroundColor = (Color)App.Current.Resources[isSelected ? "SwitchOnColor" : "SwitchOffColor"];

            var x = isSelected ? FrameContainer.WidthRequest - LeftButton.WidthRequest : 0;
            LeftButton.TranslateTo(x, LeftButton.Y, 150, Easing.Linear);

            if (ShowLabel)
            {
                TitleLabel.Style = (Style)Resources[isSelected ? "SelectedLabel" : "UnSelectedLabel"];
            }
            else
            {
                TitleLabel.Style = (Style)Resources["HideLabel"];
            }
            SetLabel();

        }

        private void SetLabel()
        {
            TitleLabel.Text = IsSelected ? SelectedLabel : UnselectedLabel;
        }


        private static void IsSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (SwitchView)bindable;
            view.SelectButton((bool)newValue);
        }

        private static void ShowLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (SwitchView)bindable;
            view.ShowLabel = (bool)newValue;

            if (view.ShowLabel)
            {
                view.TitleLabel.Style = (Style)view.Resources[view.IsSelected ? "SelectedLabel" : "UnSelectedLabel"];
            }
            else
            {
                view.TitleLabel.Style = (Style)view.Resources["HideLabel"];
            }
        }


        private static void SelectedLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (SwitchView)bindable;
            view.SelectedLabel = (string)newValue;
            view.SetLabel();
        }


        private static void UnSelectedLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (SwitchView)bindable;

            view.UnselectedLabel = (string)newValue;
            view.SetLabel();
        }


        private static void IsLockedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (SwitchView)bindable;
            view.IsLocked = (bool)newValue;
        }

        private static void IsLockedLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (SwitchView)bindable;
            view.IsLockedLabel = (string)newValue;
        }

        private static void OnChangeCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        private void Toggle(object sender, EventArgs e)
        {
            if (!IsLocked)
            {
                IsSelected = !IsSelected;
                Toggled?.Invoke(this, new ToggledEventArgs(IsSelected));
                if(OnChangeCommand != null)
                {
                    OnChangeCommand.Execute(null);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(IsLockedLabel))
                {
                    App.Locator.GetInstance<IPopupService>().Show(PopupEnum.PopupInfo, IsLockedLabel, "OK");
                }
            }
        }
    }
}