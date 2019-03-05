using System.Linq;
using Xamarin.Forms;
using CONSTANTS = Yol.Punla.Barrack.Constants;

namespace Yol.Punla.Effects
{
    public static class EntryEffectParameters
    {
        public static readonly BindableProperty IsFocusedProperty = BindableProperty.CreateAttached("IsFocused", typeof(bool), typeof(EntryEffectParameters), (bool)false, BindingMode.TwoWay, null, propertyChanged: OnFocusStateChanged);

        public static void OnFocusStateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as InputView;
            if (view == null) return;

            var effect = (InputViewEffect)view.Effects.FirstOrDefault(e => e is InputViewEffect);
            
            if (effect != null)
            {
                var newEffect = new InputViewEffect
                {
                    NumberOfLines = effect.NumberOfLines,
                    PlaceholderToLineDistance = effect.PlaceholderToLineDistance,
                    Thickness = effect.Thickness,
                    IsBindableAttached = true
                };

                if (view.IsFocused) newEffect.LineColor = Color.FromHex(CONSTANTS.PRIMARYGREEN);
                else newEffect.LineColor = Color.FromHex(CONSTANTS.PRIMARYGRAY);
                
                view.Effects.Remove(effect);
                view.Effects.Add(newEffect);
            }
        }
    }
}
