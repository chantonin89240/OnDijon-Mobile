using OnDijon.Common.Utils.Enums;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;

namespace OnDijon.Common.Services.Interfaces.Front
{
    public interface IPopupService
    {
        void Show(PopupPage popupPage);

        /// <summary>
        /// Show a basic popup with only a message and an OK button
        /// </summary>
        void Show(PopupEnum type, string text);

        /// <summary>
        /// Show a basic popup with only a formatted message and an OK button
        /// </summary>
        void Show(PopupEnum type, FormattedString formattedText);

        /// <summary>
        /// Show a popup with a message, a confirm button with optional action and an optional cancel button
        /// </summary>
        void Show(PopupEnum type, string text, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null);

        /// <summary>
        /// Show a popup with a formatted message, a confirm button with optional action and an optional cancel button
        /// </summary>
        void Show(PopupEnum type, FormattedString formattedText, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null);

        /// <summary>
        /// Show a popup with a title, a message, a confirm button with optional action and an optional cancel button
        /// </summary>
        void Show(PopupEnum type, string title, string text, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null);

        /// <summary>
        /// Show a popup with a title, a formatted message, a confirm button with optional action and an optional cancel button
        /// </summary>
        void Show(PopupEnum type, string title, FormattedString formattedText, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null);

        /// <summary>
        /// Show a popup with a title, a subtitle, a message, a confirm button with optional action and an optional cancel button
        /// </summary>
        void Show(PopupEnum type, string title, string subtitle, string text, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null);

        /// <summary>
        /// Show a popup with a title, a subtitle, a formatted message, a confirm button with optional action and an optional cancel button
        /// </summary>
        void Show(PopupEnum type, string title, string subtitle, FormattedString formattedText, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null);
    }
}
