using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.CustomControls;
using Yol.Punla.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Yol.Punla.Droid.CustomRenderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var xfEntry = Element as CustomEntry;
                var lineColor = xfEntry.BottomLineColor.ToAndroid();
                var androidEntry = Control;

                androidEntry.TextSize = (float)xfEntry.TextSize;//DroidResource.MediumFontSize;
                androidEntry.Background = Context.GetDrawable(Resource.Drawable.EntryBLineColor);
                androidEntry.BackgroundTintList = ColorStateList.ValueOf(lineColor);

                if (xfEntry.FontAttributes == FontAttributes.Bold)
                {
                    androidEntry.SetTypeface(null, TypefaceStyle.Bold);
                }
                else if (xfEntry.FontAttributes == FontAttributes.Italic)
                {
                    androidEntry.SetTypeface(null, TypefaceStyle.Italic);
                }
                
            }
            
        }
    }
}