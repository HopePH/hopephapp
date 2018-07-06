using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Yol.Punla.Localized
{
    [ContentProperty("Text")]
    public class Render : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return LocalizationService.Instance.Write(this.Text);
        }
    }
}
