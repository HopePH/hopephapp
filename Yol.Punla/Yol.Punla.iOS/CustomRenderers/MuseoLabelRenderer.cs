using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.CustomControls;
using Yol.Punla.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(MuseoLabel), typeof(MuseoLabelRenderer))]
namespace Yol.Punla.iOS.CustomRenderers
{
    public class MuseoLabelRenderer : LabelRenderer
    {
        private MuseoLabel xfControl;
        private UIFont retFont;

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control == null) return;

            if (this.Element != null)
            {
                xfControl = this.Element as MuseoLabel;
                var fontStyle = GetFontStyle(xfControl);

                if(fontStyle != null)
                    Control.Font = fontStyle; 
            }
        }

        private UIFont GetFontStyle(MuseoLabel xfLabel)
        {
            switch (xfLabel.FontAttributes)
            {
                case FontAttributes.Bold:
                    retFont = UIFont.FromName("MuseoSans-700", (float)xfControl.FontSize);
                    break;
                case FontAttributes.Italic:
                    retFont = UIFont.FromName("MuseoSans-300Italic", (float)xfControl.FontSize);
                    break;
                default:
                    retFont = UIFont.FromName("MuseoSansCyrl-300", (float)xfControl.FontSize);
                    break;
            }
            return retFont;
        }
    }
}