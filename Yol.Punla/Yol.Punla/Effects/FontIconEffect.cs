using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Yol.Punla.Effects
{
    public class FontIconEffect : RoutingEffect
    {
        public enum FontAwesomeType
        {
            REGULAR, SOLID, DEFAULT
        }

        public string FontWeight { get; set; }
        public string HexColor { get; set; }
        public FontAwesomeType IconType { get; set; } = FontAwesomeType.DEFAULT;

        public FontIconEffect() : base("Yol.Punla.FontIconEffect") { }
    }
}
