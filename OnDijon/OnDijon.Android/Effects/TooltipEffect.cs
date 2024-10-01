using Android.Views;
using Android.Widget;
using OnDijon.Common.Views.Effects;
using OnDijon.Droid.Effects;
using OnDijon.Droid.Renderers.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CommonFontUtils = OnDijon.Common.Utils.Fonts.FontUtils;

[assembly: ResolutionGroupName("OnDijon")]
[assembly: ExportEffect(typeof(DroidTooltipEffect), nameof(TooltipEffect))]
namespace OnDijon.Droid.Effects
{
    public class DroidTooltipEffect : PlatformEffect
    {
        private PopupWindow _tooltip;

        private void ShowTooltip()
        {
            var text = TooltipEffect.GetText(Element);

            if (!string.IsNullOrEmpty(text))
            {
                var control = Control ?? Container;

                var label = new TextView(control.Context) { Text = text };
                label.SetTextColor(TooltipEffect.GetTextColor(Element).ToAndroid());
                label.TextSize = (float)TooltipEffect.GetFontSize(Element);
                label.Typeface = FontUtils.GetFont(CommonFontUtils.DEFAULT_FONT_FAMILY, FontAttributes.None);
                label.SetBackgroundColor(TooltipEffect.GetBackgroundColor(Element).ToAndroid());
                label.SetPadding(10, 0, 10, 0);
                label.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

                _tooltip = new PopupWindow(label, control.Width, ViewGroup.LayoutParams.WrapContent) { ClippingEnabled = false };
                _tooltip.ShowAsDropDown(control);
            }
        }

        protected override void OnAttached()
        {
            ShowTooltip();
        }

        protected override void OnDetached()
        {
            _tooltip?.Dismiss();
        }
    }
}
