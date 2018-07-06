using Android.Content;
using Android.Graphics;
using Android.Widget;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.CustomControls;
using Yol.Punla.Droid.CustomRenderers;
using Andrd = Android.Graphics;
using Xam = Xamarin.Forms;

[assembly: ExportRenderer(typeof(MultilineEntry), typeof(MultilineEntryRenderer))]
namespace Yol.Punla.Droid.CustomRenderers
{
    public class MultilineEntryRenderer : ViewRenderer<MultilineEntry, EditText>
    {
        EditText nativeEditText;
        string currentText = "";

        protected override void OnElementChanged(ElementChangedEventArgs<MultilineEntry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                nativeEditText = new EditText(Forms.Context);
                
                var element = e.NewElement as MultilineEntry;
                var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "MuseoSansCyrl_0.otf");
                nativeEditText.Typeface = font;
                nativeEditText.TextSize = 14f;
                nativeEditText.TextChanged += NativeEditor_TextChanged;
                
                nativeEditText.Background = Context.GetDrawable(Resource.Drawable.MultilineEntryLayout);
                nativeEditText.Hint = element.MyText;
                nativeEditText.SetHintTextColor(Xam.Color.Gray.ToAndroid());
                nativeEditText.Enabled = true;
                nativeEditText.SetLines(4);
                nativeEditText.SetMinLines(4);

                // background drawable
                var backgroundDrawable = (Andrd.Drawables.LayerDrawable)nativeEditText.Background;
                Andrd.Drawables.GradientDrawable bottomline = (Andrd.Drawables.GradientDrawable)backgroundDrawable.FindDrawableByLayerId(Resource.Id.bottomline);
                Andrd.Color strokeColor = element.BottomLineColor.ToAndroid();  
                bottomline.SetStroke(bottomline.IntrinsicWidth, strokeColor);
                
                SetNativeControl(nativeEditText);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "MultiText")
            {
                var entry = (MultilineEntry)sender;

                if(entry.MultiText != null && entry.MultiText != currentText)
                    nativeEditText.Text = entry.MultiText;
            }
        }

        private void NativeEditor_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            var thisElement = Element as MultilineEntry;
            currentText = e.Text.ToString();
            thisElement.MultiText = currentText;
        }

    }
}