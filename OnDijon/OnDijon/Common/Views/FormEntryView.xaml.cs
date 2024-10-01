using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormEntryView : FormBaseView
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(FormEntryView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: TextPropertyChanged);
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FormEntryView), propertyChanged: PlaceholderPropertyChanged);
        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(FormEntryView), defaultValue: false, propertyChanged: IsPasswordPropertyChanged);
        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(FormEntryView), defaultValue: Keyboard.Default, propertyChanged: KeyboardPropertyChanged);
        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(FormEntryView), defaultValue: ReturnType.Default, propertyChanged: ReturnTypePropertyChanged);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public ReturnType ReturnType
        {
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public FormEntryView()
        {
            InitializeComponent();
        }

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormEntryView)bindable;
            view.FormEntry.Text = newValue?.ToString();
        }

        private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormEntryView)bindable;
            view.FormEntry.Placeholder = newValue?.ToString();
        }

        private static void IsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormEntryView)bindable;
            view.FormEntry.IsPassword = (bool)newValue;
        }

        private static void KeyboardPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormEntryView)bindable;
            view.FormEntry.Keyboard = (Keyboard)newValue;
        }

        private static void ReturnTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormEntryView)bindable;
            view.FormEntry.ReturnType = (ReturnType)newValue;
        }

        void EntryTextChanged(object sender, TextChangedEventArgs e)
        {
            Errors = null;
            Text = e.NewTextValue;
            TextChanged?.Invoke(this, e);
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (FormEntry.IsFocused)
                    FormEntry.Unfocus();

                FormEntry.Focus();
            });
        }

    }
}