using Avalonia.Media;

namespace ArtemisFlyout.Utiles
{
    public class ColorUtiles
    {
        public static string ToHexString(Color color)
        {
            byte r = color.R;
            byte g = color.G;
            byte b = color.B;
            byte a = color.A;

            return $"#{a:x2}{r:x2}{g:x2}{b:x2}";
        }

    }
}
