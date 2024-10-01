using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Java.Lang;
using OnDijon.Droid.Renderers;
using OnDijon.Droid.Renderers.Utils;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRenderer))]
namespace OnDijon.Droid.Renderers
{
    /// <summary>
    /// Fix font styles on Android
    /// </summary>
    class CustomLabelRenderer : LabelRenderer
    {
        public CustomLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Typeface = FontUtils.GetFont(Element.FontFamily, Element.FontAttributes);

                if (Element.FormattedText != null)
                {
                    Control.TextFormatted = UpdateFormattedText(Control.TextFormatted, Element.FormattedText);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            try
            {
                Control.Typeface = FontUtils.GetFont(Element.FontFamily, Element.FontAttributes);
            }
            catch (ObjectDisposedException ex)
            {
                System.Diagnostics.Debug.Fail(ex.ToString());
            }
        }

        private static SpannableString UpdateFormattedText(ICharSequence controlFormattedText, FormattedString elementFormattedText)
        {
            var spannable = new SpannableString(controlFormattedText);

            //remove all font and text decorations spans
            var spansToRemove = spannable.GetSpans(0, spannable.Length(), Class.FromType(typeof(MetricAffectingSpan)));
            foreach (var span in spansToRemove)
            {
                spannable.RemoveSpan(span);
            }

            //recreate font and text decoration spans with correct font
            var previous = 0;
            foreach (var span in elementFormattedText.Spans)
            {
                var start = previous;
                var end = start + (span.Text != null ? span.Text.Length : 0);
                previous = end;

                //font
                var typeface = FontUtils.GetFont(span.FontFamily, span.FontAttributes);
                spannable.SetSpan(new TypefaceSpan(typeface, (float)span.FontSize), start, end, SpanTypes.InclusiveInclusive);

                //text decoration
                if (span.TextDecorations == TextDecorations.Underline)
                {
                    spannable.SetSpan(new UnderlineSpan(), start, end, SpanTypes.InclusiveInclusive);
                }
                else if (span.TextDecorations == TextDecorations.Strikethrough)
                {
                    spannable.SetSpan(new StrikethroughSpan(), start, end, SpanTypes.InclusiveInclusive);
                }
            }

            return spannable;
        }

        /// <summary>
        /// Replace native TypefaceSpan because TypefaceSpan(Typeface) is only available on Android >= 28
        /// </summary>
        class TypefaceSpan : MetricAffectingSpan
        {
            public TypefaceSpan(Typeface typeface, float fontSize)
            {
                Typeface = typeface;
                FontSize = fontSize;
            }

            public Typeface Typeface { get; }

            public float FontSize { get; }

            public override void UpdateDrawState(TextPaint tp)
            {
                Apply(tp);
            }

            public override void UpdateMeasureState(TextPaint textPaint)
            {
                Apply(textPaint);
            }

            void Apply(Paint paint)
            {
                paint.SetTypeface(Typeface);
                paint.TextSize = TypedValue.ApplyDimension(ComplexUnitType.Sp, FontSize, Android.App.Application.Context.Resources.DisplayMetrics);
            }
        }
    }
}