using System;

namespace OnDijon.Common.Entities
{
    public interface IPopupViewSettings
    {
        bool CloseWhenBackgroundIsClicked { get; set; }
        bool DisplayBottomButtons { get; set; }
        Action OnAcceptAction { get; set; }
        Action OnDeclineAction { get; set; }

    }
    public class PopupViewSettings : IPopupViewSettings
    {
        public bool CloseWhenBackgroundIsClicked { get; set; }
        public bool DisplayBottomButtons { get; set; }
        public Action OnAcceptAction { get; set; }
        public Action OnDeclineAction { get; set; }

        public PopupViewSettings()
        {
            // true by default
            CloseWhenBackgroundIsClicked = true;
        }
    }
}
