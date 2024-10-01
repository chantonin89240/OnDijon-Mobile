using System.ComponentModel;
using Foundation;
using OnDijon.Common.Utils.Fonts;
using OnDijon.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRenderer))]
namespace OnDijon.iOS.Renderers
{
    /// <summary>
    /// Default font styles on iOS
    /// </summary>
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            SetDefaultFont();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            SetDefaultFont();
        }

        private void SetDefaultFont()
        {
            if (Control != null && Element.FontFamily == null)
            {
                Control.Font = GetFont(Element.FontAttributes, Element.FontSize);

                if (Element.FormattedText != null)
                {
                    Control.AttributedText = UpdateFormattedText(Element.FormattedText);
                }
            }
        }

        private static UIFont GetFont(FontAttributes fontAttributes, double fontSize)
        {
            var fontType = FontUtils.GetFontType(fontAttributes);
            var fontName = $"{FontUtils.DEFAULT_FONT_FAMILY}-{fontType}";
            return UIFont.FromName(fontName, (float)fontSize);
        }

        private static NSAttributedString UpdateFormattedText(FormattedString formattedString)
        {
            var attributed = new NSMutableAttributedString();
            foreach (var span in formattedString.Spans)
            {
                var font = GetFont(span.FontAttributes, span.FontSize);

                var textColor = span.TextColor;
                if (textColor.IsDefault)
                    textColor = Color.Black;

                //text decorations
                bool hasUnderline = false;
                bool hasStrikethrough = false;
                if (span.IsSet(Span.TextDecorationsProperty))
                {
                    var textDecorations = span.TextDecorations;
                    hasUnderline = (textDecorations & TextDecorations.Underline) != 0;
                    hasStrikethrough = (textDecorations & TextDecorations.Strikethrough) != 0;
                }

                var attrString = new NSAttributedString(span.Text, font, textColor.ToUIColor(), span.BackgroundColor.ToUIColor(),
                    underlineStyle: hasUnderline ? NSUnderlineStyle.Single : NSUnderlineStyle.None,
                    strikethroughStyle: hasStrikethrough ? NSUnderlineStyle.Single : NSUnderlineStyle.None);

                attributed.Append(attrString);
            }
            return attributed;
        }
    }
}