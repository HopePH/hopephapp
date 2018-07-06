using Xamarin.Forms;

namespace Yol.Punla.CustomControls
{
    public class MultilineEntry : Entry
    {
        public static readonly BindableProperty MultiTextProperty = BindableProperty.Create("MultiText", typeof(string), typeof(MultilineEntry), "", BindingMode.TwoWay);
        public string MultiText
        {
            get => (string)GetValue(MultiTextProperty);
            set => SetValue(MultiTextProperty, value);
        }

        public static readonly BindableProperty PlaceholderEnabledProperty = BindableProperty.Create("PlaceholderEnable", typeof(bool), typeof(MultilineEntry), true, BindingMode.TwoWay);
        public bool PlaceholderEnabled
        {
            get => (bool)GetValue(PlaceholderEnabledProperty);
            set => SetValue(PlaceholderEnabledProperty, value);
        }

        public static readonly BindableProperty MyTextProperty = BindableProperty.Create("MyText", typeof(string), typeof(MultilineEntry), "", BindingMode.TwoWay);
        public string MyText
        {
            get => (string)GetValue(MyTextProperty); 
            set => SetValue(MyTextProperty, value); 
        }

        public static readonly BindableProperty HasFocusedProperty = BindableProperty.Create("HasFocus", typeof(bool), typeof(MultilineEntry), false, BindingMode.TwoWay);
        public bool HasFocus
        {
            get => (bool)GetValue(HasFocusedProperty);
            set => SetValue(HasFocusedProperty, value);
        }

        public static readonly BindableProperty BottomLineColorProperty = BindableProperty.Create("BottomLineColor", typeof(Color), typeof(MultilineEntry), Color.Transparent, BindingMode.TwoWay);
        public Color BottomLineColor
        {
            get => (Color)GetValue(BottomLineColorProperty);
            set => SetValue(BottomLineColorProperty, value);
        }
    }
}
