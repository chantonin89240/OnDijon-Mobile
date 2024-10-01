using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OnDijon.Common.Views.Popup
{
    public partial class PopupVersionObsoleteView : PopupPage
    {

        public static readonly BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string), typeof(PopupVersionObsoleteView), propertyChanged: MessagePropertyChanged);


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }


        public PopupVersionObsoleteView()
        {
            Init();
        }

        private void Init()
        {
            InitializeComponent();
            MessageLabel.Text = Message;
            CloseWhenBackgroundIsClicked = false;
        }

        private async void OnClose(object sender, EventArgs e)
        {
            if(Device.RuntimePlatform == Device.Android)
            {
                await Launcher.OpenAsync(new Uri("https://play.google.com/store/search?q=ondijon&hl=fr&gl=US"));
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                await Launcher.OpenAsync(new Uri("https://apps.apple.com/fr/app/ondijon/id1540070704"));
            }
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private static void MessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (PopupVersionObsoleteView)bindable;
            view.MessageLabel.Text = (string)newValue;
        }
    }
}