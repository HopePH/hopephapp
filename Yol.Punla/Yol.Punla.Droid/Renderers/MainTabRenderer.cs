using Android.Content;
using Android.Support.Design.Widget;
using Android.Views;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Yol.Punla.Droid.Renderers;
using Yol.Punla.Utility;
using Yol.Punla.Views;

[assembly: ExportRenderer(typeof(MainTabbedPage), typeof(MainTabRenderer))]
namespace Yol.Punla.Droid.Renderers
{
    public class MainTabRenderer : Xamarin.Forms.Platform.Android.AppCompat.TabbedPageRenderer
    {
        TabLayout layout;

        public MainTabRenderer(Context context) : base(context) {  }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
                ((MainTabbedPage)Element).UpdateIcons += Handle_UpdateIcons;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (layout == null && e.PropertyName == "Renderer")
                layout = (TabLayout)ViewGroup.GetChildAt(1);
        }

        private void Handle_UpdateIcons(object sender, EventArgs e)
        {
            TabLayout tabs = layout;

            if (tabs == null)
                return;

            for (var i = 0; i < Element.Children.Count; i++)
            {
                var child = Element.Children[i].BindingContext as IIconChange;
                var icon = child.CurrentIcon;
                if (string.IsNullOrEmpty(icon))
                    continue;

                TabLayout.Tab tab = tabs.GetTabAt(i);
                SetCurrentTabIcon(tab, icon);
            }
        }

        private void SetCurrentTabIcon(TabLayout.Tab tab, string icon)
        {
            tab.SetIcon(IdFromTitle(icon, ResourceManager.DrawableClass));
        }

        private int IdFromTitle(string title, Type type)
        {
            string name = Path.GetFileNameWithoutExtension(title);
            int id = GetId(type, name);
            return id;
        }

        private int GetId(Type type, string memberName)
        {
            object value = type.GetFields().FirstOrDefault(p => p.Name == memberName)?.GetValue(type)
                ?? type.GetProperties().FirstOrDefault(p => p.Name == memberName)?.GetValue(type);
            if (value is int)
                return (int)value;
            return 0;
        }

    }
}