using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace OnDijon.Common.Views.Popup
{
    public partial class PopupVersionSuspendedView : PopupPage
    {

        public static readonly BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string), typeof(PopupVersionObsoleteView), propertyChanged: MessagePropertyChanged);


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }


        public PopupVersionSuspendedView()
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
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private static void MessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (PopupVersionSuspendedView)bindable;
            view.MessageLabel.Text = (string)newValue;
        }
    }
}