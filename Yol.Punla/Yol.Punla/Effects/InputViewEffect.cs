using Xamarin.Forms;

namespace Yol.Punla.Effects
{
    public class InputViewEffect : RoutingEffect
    {
        public Color LineColor { get; set; } = Color.Transparent;
        public int Thickness { get; set; }
        public int PlaceholderToLineDistance { get; set; } = 0;
        public bool IsApplyToDroid { get; set; } = true;
        public int NumberOfLines { get; set; } = 0;
        public bool IsBindableAttached { get; set; } = false;
        
        public InputViewEffect() : base("Yol.Punla.Effects.InputViewEffect") { }
    }
}
