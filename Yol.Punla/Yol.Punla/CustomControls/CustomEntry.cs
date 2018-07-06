using Xamarin.Forms;

namespace Yol.Punla.CustomControls
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty BottomLineColorProperty = BindableProperty.Create("BottomLineColor", typeof(Color), typeof(CustomEntry), Color.Transparent);
        public Color BottomLineColor
        {
            get => (Color)GetValue(BottomLineColorProperty);
            set => SetValue(BottomLineColorProperty, value);
        }

        public static readonly BindableProperty TextSizeProperty = BindableProperty.Create("TextSize", typeof(float), typeof(CustomEntry), 21f);
        public float TextSize
        {
            get => (float)GetValue(TextSizeProperty);
            set => SetValue(BottomLineColorProperty, value);
        }
    }
}
