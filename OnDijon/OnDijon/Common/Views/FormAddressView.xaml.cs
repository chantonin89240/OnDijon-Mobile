using OnDijon.Common.Entities.Model;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormAddressView : FormBaseView
    {
        public static readonly BindableProperty AddressProperty = BindableProperty.Create(nameof(Address), typeof(AddressModel), typeof(FormDatePickerView), defaultBindingMode: BindingMode.TwoWay, propertyChanged: AddressPropertyChanged);
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FormDatePickerView), propertyChanged: PlaceholderPropertyChanged);

        public AddressModel Address
        {
            get { return (AddressModel)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public FormAddressView()
        {
            InitializeComponent();
        }

        private void OnAddressTapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new AddressPickerView(OnAddressSelected, Address));
        }

        private static void AddressPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormAddressView)bindable;
            if(((AddressModel)newValue).FullAddress != null)
            {
                view.AddressLabel.Text = String.Concat(((AddressModel)newValue).FullAddress, ", ", ((AddressModel)newValue).City, " (", ((AddressModel)newValue).PostalCode, ")");
            }
            else
            {
                view.AddressLabel.Text = String.Concat(((AddressModel)newValue).FullAddress);
            }


            var styleKey = "FormDatePicker";
            view.AddressLabel.Style = (Style)Application.Current.Resources[styleKey];
        }

        private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (FormAddressView)bindable;
            view.AddressLabel.Text = newValue?.ToString();

            var styleKey = "FormPickerPlaceholder";
            view.AddressLabel.Style = (Style)Application.Current.Resources[styleKey];
        }
        private void OnAddressSelected(AddressModel newAddress)
        {
            Errors = null;
            var oldDate = Address ;
            Address = newAddress;
            //DateSelected?.Invoke(this, new AddressChangedEventArgs(oldDate, newDate));
        }

    }
}