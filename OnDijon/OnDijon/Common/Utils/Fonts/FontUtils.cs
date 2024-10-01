using Xamarin.Forms;

namespace OnDijon.Common.Utils.Fonts
{
    public static class FontUtils
    {
        public const string DEFAULT_FONT_FAMILY = "Isidora";

        /// <summary>
        /// Get the path for the bundled font corresponding to the given font family and font attributes
        /// (defaults to Isidora font)
        /// </summary>
        public static string GetFontPath(string fontFamily, FontAttributes fontAttributes)
        {
            fontFamily = fontFamily ?? DEFAULT_FONT_FAMILY;
            var fontType = GetFontType(fontAttributes);
            return $"font/{fontFamily}-{fontType}.otf";
        }

        public static string GetFontType(FontAttributes fontAttributes)
        {
            switch (fontAttributes)
            {
                case FontAttributes.None:
                    return "Medium";
                case FontAttributes.Bold:
                    return "Bold";
                case FontAttributes.Italic:
                    return "MediumIt";
                default:
                    return "BoldIt";
            }
        }
    }
}
