using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Entities.Model;
using OnDijon.Modules.Notifications.Services.Interfaces;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OnDijon.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressPickerView : PopupPage
    {
        public delegate void AdressChangedDelegate(AddressModel adress);
        private readonly AdressChangedDelegate _adressChangedDelegate;

        public AddressModel Address { get; set; }
        public CityModel City { get; set; }
        public string SearchValue { get; set; }

        public AddressPickerView(AdressChangedDelegate @delegate, AddressModel address)
        {

            InitializeComponent();
            _adressChangedDelegate = @delegate;
            Address = address;
            if (Address != null)
            {
                SearchEntry.Text = Address.FullAddress;
                City = new CityModel() { CodComm = address.CodCom, Name = address.City };
                SearchCityEntry.Text = Address.City;
            }
        }

        void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Address != null && Address.FullAddress == e.NewTextValue)
            {
                SearchAddress.IsVisible = false;
                return;
            }
            if (e.NewTextValue.Length > 3)
            {
                Address = null;
                SearchAddress.IsVisible = true;
                Search(e.NewTextValue);
            }
            else
            {
                SearchAddress.IsVisible = false;
            }
        }

        private async void Search(string pattern)
        {

            var a = App.Locator.GetInstance<IAddressServices>();
            var response = await a.GetAddressFromCity(City, pattern);

            if (response.State == CallStatusEnum.Success)
            {
                SearchList.ItemsSource = response.AddressModel?.ToList();
            }
        }

        private void SearchList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SearchAddress.IsVisible = false;
            Address = e.SelectedItem as AddressModel;
            SearchEntry.Text = Address.FullAddress;
        }

        private async void OnConfirm(object sender, EventArgs e)
        {
            _adressChangedDelegate(Address);
            await PopupNavigation.Instance.PopAsync();
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void SearchCityAsync(string pattern)
        {
            var a = App.Locator.GetInstance<IAddressServices>();
            var response = await a.GetCities(pattern);
            if (response.State == CallStatusEnum.Success)
            {
                SearchCityList.ItemsSource = response.CityModels?.ToList();
            }
        }

        private void SearchCityEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (City != null && City.Name == e.NewTextValue)
            {
                SearchCity.IsVisible = false;
                return;
            }
            if (e.NewTextValue.Length > 1)
            {
                City = null;
                SearchCity.IsVisible = true;
                SearchCityAsync(e.NewTextValue);
            }
            else
            {
                SearchCity.IsVisible = false;
            }

        }

        private void SearchCityList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            City = e.SelectedItem as CityModel;
            SearchCity.IsVisible = false;
            SearchEntryLayout.IsVisible = true;
            SearchEntry.Text = string.Empty;
            SearchCityEntry.Text = City.Name;
        }
    }
}