using System.Linq;
using Xamarin.Forms;

namespace Yol.Punla.Effects
{
    public static class EntryEffectParameters
    {
        private const string GREEN = "#FF417505";
        private const string GRAY = "#1F000000";

        public static readonly BindableProperty IsFocusedProperty = BindableProperty.CreateAttached("IsFocused", typeof(bool), typeof(EntryEffectParameters), (bool)false, BindingMode.TwoWay, null, propertyChanged: OnFocusStateChanged);

        public static void OnFocusStateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Entry;
            if (view == null) return;

            var effect = (EntryEffect)view.Effects.FirstOrDefault(e => e is EntryEffect);
            
            if (effect != null)
            {
                var newEffect = new EntryEffect
                {
                    NumberOfLines = effect.NumberOfLines,
                    PlaceholderToLineDistance = effect.PlaceholderToLineDistance,
                    Thickness = effect.Thickness
                };

                if (view.IsFocused) newEffect.LineColor = Color.FromHex(GREEN);
                else newEffect.LineColor = Color.FromHex(GRAY);
                
                view.Effects.Remove(effect);
                view.Effects.Add(newEffect);
            }
        }
    }
}
