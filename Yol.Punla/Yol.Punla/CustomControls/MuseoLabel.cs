using Xamarin.Forms;

namespace Yol.Punla.CustomControls
{
    public class MuseoLabel : Label
    {
        public static readonly BindableProperty IsUnderlinedProperty = BindableProperty.Create("IsUnderlined",typeof(bool),typeof(MuseoLabel),false);
        public bool IsUnderlined { get => (bool)GetValue(IsUnderlinedProperty); set => SetValue(IsUnderlinedProperty, value); }
    }
}
