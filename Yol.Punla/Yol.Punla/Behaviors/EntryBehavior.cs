using System.Linq;
using Xamarin.Forms;

namespace Yol.Punla.Behaviors
{
    public class EntryBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.Focused += Bindable_Focused;
        }

        private void Bindable_Focused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            var effect = (Effects.EntryEffect)entry.Effects.FirstOrDefault(ef => ef is Effects.EntryEffect);

            if(effect != null)
            {
                effect.LineColor = Color.Red;
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
        }
    }
}
