using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(Yol.Punla.Droid.CustomRenderers.FontAwesomeRenderer))]
namespace Yol.Punla.Droid.CustomRenderers
{
    public class FontAwesomeRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var label = (TextView)Control;
            var text = label.Text;

            if (text.Length > 1 && text[0] < 0xf000)
            {
                return;
            }

            var font = Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.ApplicationContext.Assets, "fontawesome-webfont.ttf");
            label.Typeface = font;
        }
    }
}