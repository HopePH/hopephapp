/* chito. the fonts are downloaded from this site free http://fontsgeek.com/fonts/Museo-300/download
 */

using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.CustomControls;
using Yol.Punla.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(MuseoLabel), typeof(MuseoLabelRenderer))]
namespace Yol.Punla.Droid.CustomRenderers
{
    public class MuseoLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var nativeLabel = Control;
                var oldElement = this.Element as MuseoLabel;
                
                if(oldElement.IsUnderlined)
                    nativeLabel.SetBackgroundResource(Resource.Drawable.Underline);

                var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "MuseoSansCyrl_0.otf");

                if (oldElement.FontAttributes == FontAttributes.Bold)
                    font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "MuseoSansBold.otf");
                else if (oldElement.FontAttributes == FontAttributes.Italic)
                    font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "MuseoSans-300Italic.otf");
                else
                    font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "MuseoSansCyrl_0.otf");

                nativeLabel.Typeface = font;
            }
        }
    }
}