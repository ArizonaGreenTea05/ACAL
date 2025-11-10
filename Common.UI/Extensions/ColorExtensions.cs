using System.Drawing;

namespace Common.UI.Extensions
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Calculates the foreground color based on the background color
        /// </summary>
        /// <param name="backColor"> the current background color</param>
        /// <returns>foreground color</returns>
        public static Color GetForeColor(this Color backColor)
        {
            const int threshold = 120;
            var bgDelta = Convert.ToInt32(backColor.R * 0.299 + backColor.G * 0.587 + backColor.B * 0.114);
            return 255 - bgDelta < threshold ? Color.DimGray : Color.WhiteSmoke;
        }

        public static Color GetDimColor(this Color color, double factor = 0.3)
        {
            if (factor is < 0 or > 1)
                throw new ArgumentOutOfRangeException(nameof(factor), "Factor must be between 0.0 and 1.0");

            var r = (int)(color.R * factor);
            var g = (int)(color.G * factor);
            var b = (int)(color.B * factor);

            return Color.FromArgb(color.A, r, g, b);
        }

        public static string ToHex(this Color color, bool includeAlpha = true)
        {
            return includeAlpha
                ? $"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}"
                : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
