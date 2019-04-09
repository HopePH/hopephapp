using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionarePage : ContentPage
    {
        public QuestionarePage()
        {
            try
            {
                InitializeComponent();
            }
            catch (XamlParseException xp)
            {
                if (!xp.Message.Contains("StaticResource not found for key"))
                    throw;
            }
            catch (Exception ex)
            {
                if (!(ex.Source == "FFImageLoading.Forms" || ex.Source == "FFImageLoading.Transformations"))
                    throw;
            }
        }
    }
}