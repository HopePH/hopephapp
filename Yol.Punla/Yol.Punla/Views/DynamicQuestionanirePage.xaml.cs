using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DynamicQuestionanirePage : ContentPage
    {
        public DynamicQuestionanirePage()
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

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                ((ListView)sender).SelectedItem = null;
            }
            catch { }
        }
    }
}