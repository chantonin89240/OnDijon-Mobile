using OnDijon.Common.Utils.Enums;
using OnDijon.Common.Services.Interfaces.Front;
using OnDijon.Common.Views;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;

namespace OnDijon.Common.Services
{
    class PopupService : IPopupService
    {
        public void Show(PopupPage popupPage)
        {
            PopupNavigation.Instance.PushAsync(popupPage);
        }

        public void Show(PopupEnum type, string text)
        {
            PopupNavigation.Instance.PushAsync(new PopupView(type, null, null, text));
        }

        public void Show(PopupEnum type, FormattedString formattedText)
        {
            PopupNavigation.Instance.PushAsync(new PopupView(type, null, null, formattedText));
        }

        public void Show(PopupEnum type, string text, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            PopupNavigation.Instance.PushAsync(new PopupView(type, null, null, text, confirmButtonText, confirmButtonAction, cancelButtonText));
        }

        public void Show(PopupEnum type, FormattedString formattedText, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            PopupNavigation.Instance.PushAsync(new PopupView(type, null, null, formattedText, confirmButtonText, confirmButtonAction, cancelButtonText));
        }

        public void Show(PopupEnum type, string title, string text, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            PopupNavigation.Instance.PushAsync(new PopupView(type, title, null, text, confirmButtonText, confirmButtonAction, cancelButtonText));
        }

        public void Show(PopupEnum type, string title, FormattedString formattedText, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            PopupNavigation.Instance.PushAsync(new PopupView(type, title, null, formattedText, confirmButtonText, confirmButtonAction, cancelButtonText));
        }

        public void Show(PopupEnum type, string title, string subtitle, string text, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            PopupNavigation.Instance.PushAsync(new PopupView(type, title, subtitle, text, confirmButtonText, confirmButtonAction, cancelButtonText));
        }

        public void Show(PopupEnum type, string title, string subtitle, FormattedString formattedText, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            PopupNavigation.Instance.PushAsync(new PopupView(type, title, subtitle, formattedText, confirmButtonText, confirmButtonAction, cancelButtonText));
        }
    }
}
