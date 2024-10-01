using System;
using CoreGraphics;
using OnDijon.Common.Views.Effects;
using OnDijon.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CommonFontUtils = OnDijon.Common.Utils.Fonts.FontUtils;

[assembly: ResolutionGroupName("OnDijon")]
[assembly: ExportEffect(typeof(IosTooltipEffect), nameof(TooltipEffect))]
namespace OnDijon.iOS.Effects
{
    public class IosTooltipEffect : PlatformEffect
    {
        private const float PADDING = 5;

        private UILabel _label;

        private UIView _tooltip;

        private UIScrollView _scrollView;

        private readonly UIWindow _window = UIApplication.SharedApplication.KeyWindow;

        private bool _scrolledEventAdded = false;

        private void ShowTooltip()
        {
            var text = TooltipEffect.GetText(Element);

            if (!string.IsNullOrEmpty(text))
            {
                _label = new UILabel
                {
                    Text = text,
                    TextColor = TooltipEffect.GetTextColor(Element).ToUIColor(),
                    Font = UIFont.FromName(CommonFontUtils.DEFAULT_FONT_FAMILY, (float)TooltipEffect.GetFontSize(Element)),
                    Lines = 0,
                    LineBreakMode = UILineBreakMode.WordWrap
                };

                _tooltip = new UIView
                {
                    BackgroundColor = TooltipEffect.GetBackgroundColor(Element).ToUIColor(),
                };
                _tooltip.AddSubview(_label);
                UpdateTooltipFrame();
                _window.AddSubview(_tooltip);

                _scrollView = GetScrollView(Control.Superview);
                if (_scrollView != null)
                {
                    try
                    {
                        _scrollView.Scrolled += ScrollView_Scrolled;
                        _scrolledEventAdded = true;
                    }
                    catch
                    {
                        Console.WriteLine("Can't add scrolling to TooltipEffect!");
                    }

                    //scroll to the control
                    var rect = Control.ConvertRectToCoordinateSpace(Control.Frame, _scrollView);
                    _scrollView.ScrollRectToVisible(rect, true);
                }
            }
        }

        private void UpdateTooltipFrame()
        {
            var frame = Control.ConvertRectToCoordinateSpace(Control.Frame, _window);
            _label.Frame = new CGRect(PADDING, PADDING, frame.Width - 2 * PADDING, 0);
            _label.SizeToFit();
            _tooltip.Frame = new CGRect(frame.X, frame.Y + frame.Height, frame.Width, _label.Frame.Height + 2 * PADDING);
        }

        private UIScrollView GetScrollView(UIView superView)
        {
            if (superView is UIScrollView scrollView)
            {
                return scrollView;
            }
            else
            {
                return superView.Superview != null ? GetScrollView(superView.Superview) : null;
            }
        }

        private void ScrollView_Scrolled(object sender, EventArgs e)
        {
            UpdateTooltipFrame();
        }

        protected override void OnAttached()
        {
            ShowTooltip();
        }

        protected override void OnDetached()
        {
            _tooltip?.RemoveFromSuperview();
            if (_scrolledEventAdded)
            {
                _scrollView.Scrolled -= ScrollView_Scrolled;
            }
        }
    }
}
