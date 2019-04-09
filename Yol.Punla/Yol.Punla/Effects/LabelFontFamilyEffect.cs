using Xamarin.Forms;

namespace Yol.Punla.Effects
{
    public class LabelFontFamilyEffect : RoutingEffect
    {
        public enum IconType { SOLID, REGULAR }
        public enum FontType { FONTAWESOME, MUSEO }

        public IconType FontIconType { get; set; } = IconType.SOLID;
        public FontType FontFamily { get; set; } = FontType.MUSEO;

        public LabelFontFamilyEffect() : base("Yol.Punla.Effects.LabelFontFamilyEffect") {  }
    }
}
