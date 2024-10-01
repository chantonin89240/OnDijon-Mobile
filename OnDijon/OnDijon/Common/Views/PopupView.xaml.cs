using System;
using OnDijon.Common.Utils.Enums;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace OnDijon.Common.Views
{
    public partial class PopupView : PopupPage
    {
        public PopupView(PopupEnum type, string title, string subtitle, string text, string confirmButtonText = "OK", Action confirmButtonAction = null, string cancelButtonText = null)
        {
            Init(type, title, subtitle, confirmButtonText, confirmButtonAction, cancelButtonText);
            PopupMessage.Text = text;
        }

        public PopupView(PopupEnum type, string title, string subtitle, FormattedString formattedText, string confirmButtonText = "OK", Action confirmButtonAction = null, string cancelButtonText = null)
        {
            Init(type, title, subtitle, confirmButtonText, confirmButtonAction, cancelButtonText);
            PopupMessage.FormattedText = formattedText;
        }

        private void Init(PopupEnum type, string title, string subtitle, string confirmButtonText, Action confirmButtonAction, string cancelButtonText)
        {
            InitializeComponent();
            Content.AutomationId = type.ToString();

            //header style depends on popup type
            VisualStateManager.GoToState(PopupIcon, type.ToString());
            VisualStateManager.GoToState(PopupTitle, type.ToString());
            VisualStateManager.GoToState(PopupSubtitle, type.ToString());

            //title
            if (!string.IsNullOrEmpty(title))
            {
                PopupTitle.Text = title;
                PopupHeader.IsVisible = true;
            }

            //subtitle
            if (!string.IsNullOrEmpty(subtitle))
            {
                PopupSubtitle.Text = subtitle;
                PopupSubtitle.IsVisible = true;
            }

            //confirm button
            PopupAcceptButton.Text = confirmButtonText;
            if (confirmButtonAction != null)
            {
                PopupAcceptButton.Clicked += (sender, args) => confirmButtonAction.Invoke();
            }

            //cancel button
            if (cancelButtonText != null)
            {
                PopupCancelButton.Text = cancelButtonText;
                PopupCancelButton.IsVisible = true;
            }
            else if (confirmButtonAction != null)
            {
                //disable closing the popup if there is no cancel action
                CloseWhenBackgroundIsClicked = false;
            }
        }

        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return !CloseWhenBackgroundIsClicked;
        }
    }
}