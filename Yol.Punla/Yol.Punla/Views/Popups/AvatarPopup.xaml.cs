using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using System;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Yol.Punla.Barrack;
using Yol.Punla.ViewModels;

namespace Yol.Punla.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AvatarPopup : PopupPage
    {
        public AvatarPopup(ViewModelBase viewModel)
        {
            try
            {
                InitializeComponent();

                if (viewModel.GetType() == typeof(AccountRegistrationPageViewModel))
                    BindingContext = (AccountRegistrationPageViewModel)viewModel;
                else
                    throw new ArgumentException("Please set ViewModel type on CategoriesPopup!");
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
        protected override bool OnBackgroundClicked()
           => false;

        protected override bool OnBackButtonPressed()
            => true;

        protected override void OnAppearing()
            => base.OnAppearing();

        protected override void OnDisappearing()
            => base.OnDisappearing();

        private async void ClosePopup(object sender, EventArgs e) 
            => await AppUnityContainer.Instance.Resolve<IPopupNavigation>().PopAsync();

        private async void FlowListView_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender != null)
            {
                if (BindingContext.GetType() == typeof(AccountRegistrationPageViewModel))
                {
                    var vm = (AccountRegistrationPageViewModel)BindingContext;
                    vm.SetAvatarUrlCommand.Execute(e.Item as Avatar);
                }
            }

            await AppUnityContainer.Instance.Resolve<IPopupNavigation>().PopAsync();
        }
    }
}