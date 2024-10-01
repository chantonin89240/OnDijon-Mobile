using System;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormDatePickerView : FormBaseView
    {
        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime?), typeof(FormDatePickerView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: DatePropertyChanged);
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FormDatePickerView), propertyChanged: PlaceholderPropertyChanged);

        public DateTime? Date
        {
            get { return (DateTime?)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public event EventHandler<DateChangedEventArgs> DateSelected;

        public FormDatePickerView()
        {
            InitializeComponent();
        }

        private static void DatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormDatePickerView)bindable;
            view.DateLabel.Text = ((DateTime?)newValue)?.ToString("dd/MM/yyyy");

            var styleKey = "FormDatePicker";
            view.DateLabel.Style = (Style)Application.Current.Resources[styleKey];
        }

        private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormDatePickerView)bindable;
            view.DateLabel.Text = newValue?.ToString();

            var styleKey = "FormPickerPlaceholder";
            view.DateLabel.Style = (Style)Application.Current.Resources[styleKey];
        }

        private void OnDateTapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new DatePickerPopupView(OnDateSelected, Date));
        }

        private void OnDateSelected(DateTime newDate)
        {
            Errors = null;
            var oldDate = Date ?? DateTime.Today;
            Date = newDate;
            DateSelected?.Invoke(this, new DateChangedEventArgs(oldDate, newDate));
        }
    }
}