﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Yol.Punla.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationSearchBar : ContentView
    {
        public NavigationSearchBar()
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
                if (!(ex.Source == "FFImageLoading.Forms" || ex.Source == "FFImageLoading.Transformations" || ex.Message.Contains("TransparentBottomBorderSearchBarEffect")))
                    throw;
            }
        }
    }
}