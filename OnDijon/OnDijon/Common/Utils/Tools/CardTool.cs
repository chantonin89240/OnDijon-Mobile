namespace OnDijon.Common.Utils.Tools
{
    public class CardTool
    {
        public static string[] Colors = new[] { "#EAF7FF", "#E5E5E5", "#BAFFE8", "#FFDFC2", "#FFD4D4" };
        public static string[] ColorsBM = new[] { "#EAF7FF", "#E5E5E5", "#BAFFE8", "#FFDFC2", "#FFD4D4" };
        public static string[] ColorsPerisco = new[] { "#BAFFE8", "#FFDFC2", "#FFD4D4", "#EAF7FF", "#E5E5E5" };


        internal static string GetColor(int i)
        {
            return Colors[(i++) % Colors.Length];
        }

        internal static string GetColor(int i, string[] colorList)
        {
            return colorList[(i++) % colorList.Length];
        }

    }
}
