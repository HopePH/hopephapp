using CoreGraphics;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Yol.Punla.CustomControls;
using Yol.Punla.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(MultilineEntry), typeof(MultilineEntryRenderer))]
namespace Yol.Punla.iOS.CustomRenderers
{
    public class MultilineEntryRenderer : ViewRenderer<MultilineEntry, UITextView>
    {
        private const int TXTHEIGHT = 20;
        private UILabel labelPlaceholder;
        UITextView textView;
        UIView bottomLine;
        static CGRect bottomLineFrame;
        private const double WIDTHRATIO = 0.667;

        protected override void OnElementChanged(ElementChangedEventArgs<MultilineEntry> e)
        {
            base.OnElementChanged(e);

            if(e.NewElement != null)
            {
                var newElement = e.NewElement as MultilineEntry;
                
                textView = new UITextView(new CGRect(newElement.X, newElement.Y, UIScreen.MainScreen.Bounds.Width, 0.0));
                textView.BackgroundColor = newElement.BackgroundColor.ToUIColor(); 
                textView.TranslatesAutoresizingMaskIntoConstraints = false;

                // Placeholder
                var font = UIFont.FromName("MuseoSansCyrl-300", 14f);
                labelPlaceholder = new UILabel(new CGRect(textView.Frame.X + 8, textView.Frame.Y + 5, textView.Frame.Width - 50, TXTHEIGHT));
                labelPlaceholder.Text = (string.IsNullOrEmpty(newElement.MultiText) && newElement.PlaceholderEnabled) ? newElement.MyText : newElement.MultiText;
                labelPlaceholder.Font = font;
                labelPlaceholder.TextColor = Color.Gray.ToUIColor();
                labelPlaceholder.Lines = 3;
                labelPlaceholder.AdjustsFontSizeToFitWidth = true;

                // Bottom Line
                bottomLine = new UIView();
                bottomLineFrame = new CGRect(textView.Frame.X, textView.Frame.Y + (2 * TXTHEIGHT) + 8, textView.Frame.Width * WIDTHRATIO, 1.0);
                bottomLine.Frame = bottomLineFrame;
                bottomLine.BackgroundColor = newElement.BottomLineColor.ToUIColor();

                this.AddSubview(bottomLine);
                textView.AddSubview(labelPlaceholder);

                textView.Editable = true;
                textView.Font = font;
                textView.Changed += TextView_Changed;

                textView.ShouldBeginEditing = (UITextView tv) =>
                {
                    if (!Element.HasFocus)
                    {
                        Element.Text = Control.Text;
                        Element.HasFocus = true;
                    }

                    return true;
                };

                textView.ShouldEndEditing = (UITextView tv) =>
                {
                    if (Element.HasFocus)
                        Element.HasFocus = false;
                    return true;
                };

                SetNativeControl(textView);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
            if (e.PropertyName == "MultiText")
            {
                var oldElement = this.Element as MultilineEntry;
                this.Control.Text = oldElement.MultiText;
                labelPlaceholder.Hidden = (string.IsNullOrEmpty(oldElement.MultiText)) ? false : true;
            }
        }

        private void TextView_Changed(object sender, System.EventArgs e)
        {
            Element.MultiText = Control.Text;
            textView.Text = Element.MultiText;

            labelPlaceholder.Hidden = (string.IsNullOrEmpty(Control.Text)) ? false : true;
        }

    }
}