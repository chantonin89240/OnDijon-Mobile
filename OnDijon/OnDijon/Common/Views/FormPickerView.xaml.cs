using System;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormPickerView : FormBaseView
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(FormPickerView), propertyChanged: TitlePropertyChanged);
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(FormPickerView), propertyChanged: ItemsSourcePropertyChanged);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(FormPickerView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: SelectedItemPropertyChanged);
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(FormPickerView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: SelectedIndexPropertyChanged);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public BindingBase ItemDisplayBinding
        {
            get { return FormPicker.ItemDisplayBinding; }
            set { FormPicker.ItemDisplayBinding = value; }
        }

        public event EventHandler SelectedIndexChanged;

        public FormPickerView()
        {
            InitializeComponent();
        }

        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormPickerView)bindable;
            view.FormPicker.Title = newValue?.ToString();
        }

        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormPickerView)bindable;
            view.FormPicker.ItemsSource = (IList)newValue;
        }

        private static void SelectedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormPickerView)bindable;
            view.FormPicker.SelectedItem = newValue;
        }

        private static void SelectedIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormPickerView)bindable;
            view.FormPicker.SelectedIndex = (int)newValue;
        }

        void PickerIndexChanged(object sender, EventArgs args)
        {
            Errors = null;
            SelectedItem = FormPicker.SelectedItem;
            SelectedIndex = FormPicker.SelectedIndex;
            SelectedIndexChanged?.Invoke(this, args);

            //set placeholder style if no selection
            var styleKey = SelectedItem != null ? "FormPicker" : "FormPickerPlaceholder";
            FormPicker.Style = (Style)Application.Current.Resources[styleKey];
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (FormPicker.IsFocused)
                    FormPicker.Unfocus();

                FormPicker.Focus();
            });
        }
    }
}