﻿using System;
using Xamarin.Forms.Xaml;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionnairePage : AppViewBase
    {
        public QuestionnairePage()
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