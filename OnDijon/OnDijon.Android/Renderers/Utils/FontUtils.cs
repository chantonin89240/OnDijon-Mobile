using Android.Graphics;
using System.Collections.Generic;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;
using CommonFontUtils = OnDijon.Common.Utils.Fonts.FontUtils;

namespace OnDijon.Droid.Renderers.Utils
{
    public static class FontUtils
    {
        private static readonly Dictionary<(string, FontAttributes), Typeface> _fontCache = new Dictionary<(string, FontAttributes), Typeface>();

        /// <summary>
        /// Return the font corresponding to the given font family and font attributes
        /// </summary>
        public static Typeface GetFont(string fontFamily, FontAttributes fontAttributes)
        {
            if (!_fontCache.ContainsKey((fontFamily, fontAttributes)))
            {
                var fontPath = CommonFontUtils.GetFontPath(fontFamily, fontAttributes);
                _fontCache[(fontFamily, fontAttributes)] = Typeface.CreateFromAsset(AndroidApp.Context.Assets, fontPath);
            }

            return _fontCache[(fontFamily, fontAttributes)];
        }
    }
}