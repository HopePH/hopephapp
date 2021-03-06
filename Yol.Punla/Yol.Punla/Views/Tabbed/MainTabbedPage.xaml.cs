﻿using Prism.Mvvm;
using Prism.Navigation;
using System;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using CONSTANTS = Yol.Punla.Barrack.Constants;

namespace Yol.Punla.Views
{
    [ModuleView]
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainTabbedPage : Xamarin.Forms.TabbedPage, INavigatingAware
    {
        private bool _isTabPageVisible;
        public static readonly BindableProperty SelectedTabIndexProperty =  BindableProperty.Create( nameof(SelectedTabIndex),  typeof(int), typeof(MainTabbedPage), 0,  BindingMode.TwoWay, null,  propertyChanged: OnSelectedTabIndexChanged);

        public int SelectedTabIndex
        {
            get { return (int)GetValue(SelectedTabIndexProperty); }
            set { SetValue(SelectedTabIndexProperty, value); }
        }

        public MainTabbedPage ()
		{
            try
            {
                InitializeComponent();
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetBarItemColor(Color.FromHex(CONSTANTS.PRIMARYGRAY));
                On<Xamarin.Forms.PlatformConfiguration.Android>().SetBarSelectedItemColor(Color.FromHex(CONSTANTS.PRIMARYGREEN));
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _isTabPageVisible = true;
            CurrentPage = Children[SelectedTabIndex];
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _isTabPageVisible = false;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            
            SelectedTabIndex = Children.IndexOf(CurrentPage);
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            System.Diagnostics.Debug.WriteLine($"{Title} OnNavigatingTo");
            var tabs = parameters.GetValues<string>("addTab");

            foreach (var name in tabs)
                AddChild(name);
        }

        private static void OnSelectedTabIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (((MainTabbedPage)bindable)._isTabPageVisible)
            {
                ((MainTabbedPage)bindable).CurrentPage = ((MainTabbedPage)bindable).Children[(int)newValue];
            }
        }

        private void AddChild(string name)
        {
            var page = AppUnityContainer.Instance.Resolve<object>(name) as Page;
            Children.Add(page);
        }
    }
}