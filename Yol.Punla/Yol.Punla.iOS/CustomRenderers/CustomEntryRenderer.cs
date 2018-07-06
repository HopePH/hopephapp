using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Native = Yol.Punla.iOS.CustomRenderers;
using Portable = Yol.Punla.CustomControls;

[assembly: ExportRenderer(typeof(Portable.CustomEntry), typeof(Native.CustomEntryRenderer))]
namespace Yol.Punla.iOS.CustomRenderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var pclEntry = Element as Portable.CustomEntry;

                Control.BorderStyle = UITextBorderStyle.None;

                UIView bottomline = new UIView();
                CGRect bottomlineFrame = new CGRect();
                bottomlineFrame.X = Control.Frame.X;
                bottomlineFrame.Y = Bounds.Y + Bounds.Size.Height;
                bottomlineFrame.Width = Bounds.Size.Width;
                bottomlineFrame.Height = 2f;

                bottomline.Frame = bottomlineFrame;
                bottomline.BackgroundColor = pclEntry.BottomLineColor.ToUIColor();
                bottomline.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin;

                AddSubview(bottomline);
            }
        }
    }
}