using System;
using OnDijon.CG.Enums;
using OnDijon.CG.Services.Interfaces.Front;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace OnDijon.UnitTest.CG.Services.Mocks
{
    class PopupMockService : IPopupService
    {
        public Action ConfirmButtonAction { get; private set; }

        public void Show(PopupEnum type, string text)
        {
            Console.WriteLine($"PopupMockService: Show({type}, {text})");
            ConfirmButtonAction = null;
        }

        public void Show(PopupEnum type, FormattedString formattedText)
        {
            Console.WriteLine($"PopupMockService: Show({type}, {formattedText})");
            ConfirmButtonAction = null;
        }

        public void Show(PopupEnum type, string text, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            Console.WriteLine($"PopupMockService: Show({type}, {text}, {confirmButtonText}, {confirmButtonAction}, {cancelButtonText})");
            ConfirmButtonAction = confirmButtonAction;
        }

        public void Show(PopupEnum type, FormattedString formattedText, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            Console.WriteLine($"PopupMockService: Show({type}, {formattedText}, {confirmButtonText}, {confirmButtonAction}, {cancelButtonText})");
            ConfirmButtonAction = confirmButtonAction;
        }

        public void Show(PopupEnum type, string title, string text, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            Console.WriteLine($"PopupMockService: Show({type}, {title}, {text}, {confirmButtonText}, {confirmButtonAction}, {cancelButtonText})");
            ConfirmButtonAction = confirmButtonAction;
        }

        public void Show(PopupEnum type, string title, FormattedString formattedText, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            Console.WriteLine($"PopupMockService: Show({type}, {title}, {formattedText}, {confirmButtonText}, {confirmButtonAction}, {cancelButtonText})");
            ConfirmButtonAction = confirmButtonAction;
        }

        public void Show(PopupEnum type, string title, string subtitle, string text, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            Console.WriteLine($"PopupMockService: Show({type}, {title}, {subtitle}, {text}, {confirmButtonText}, {confirmButtonAction}, {cancelButtonText})");
            ConfirmButtonAction = confirmButtonAction;
        }

        public void Show(PopupEnum type, string title, string subtitle, FormattedString formattedText, string confirmButtonText, Action confirmButtonAction = null, string cancelButtonText = null)
        {
            Console.WriteLine($"PopupMockService: Show({type}, {title}, {subtitle}, {formattedText}, {confirmButtonText}, {confirmButtonAction}, {cancelButtonText})");
            ConfirmButtonAction = confirmButtonAction;
        }

        public void Show(PopupPage popupPage)
        {

        }
    }
}
