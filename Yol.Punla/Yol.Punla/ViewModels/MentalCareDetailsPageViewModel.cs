using Plugin.ExternalMaps;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Localized;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class MentalCareDetailsPageViewModel : ViewModelBase
    {
        private readonly INavigationStackService _navigationStackService;
        private readonly INavigationService _navigationService;

        public ICommand NavigateBackCommand => new DelegateCommand(GoBack);
        public ICommand NavigateToHomePage => new DelegateCommand(RedirectToHomePage);
        public ICommand DialHotlineCommand => new DelegateCommand(DialCrisisHotline);
        public ICommand ViewOnMapCommand => new DelegateCommand(async() => await ViewOnGoogleMap());
        public bool HasLauncedExternalAppMap { get; set; }

        public string Name
        {
            get
            {
                if (MentalCareFacility != null)
                    return MentalCareFacility.GroupName ?? "";

                return "";
            }
        }

        public string Address
        {
            get
            {
                if (MentalCareFacility != null)
                    return MentalCareFacility.Location ?? "";

                return "";
            }
        }

        public string PhoneNo
        {
            get
            {
                if (MentalCareFacility != null)
                    return MentalCareFacility.PhoneNumber ?? "";

                return "";
            }
        }

        public string PhotoUrl
        {
            get {
                if (MentalCareFacility != null)
                    return MentalCareFacility.PhotoUrl ?? "";

                return "";
            }
        }

        public string Description
        {
            get {
                if (MentalCareFacility != null)
                    return MentalCareFacility.Description ?? "";

                return "";
            }
        }
        
        public TimeSpan CachedImagesDuration
        {
            get => TimeSpan.FromDays(7.0);
        }

        public Entity.MentalHealthFacility MentalCareFacility { get; set; }

        public MentalCareDetailsPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser, 
            INavigationStackService navigationStackService,
            INavigationService navigationService) : base(serviceMapper, appUser)
        {
            _navigationStackService = navigationStackService;
            _navigationService = navigationService;
            Title = AppStrings.TitleCall;
        }

        public override void PreparingPageBindings()
        {
            if (PassingParameters != null && PassingParameters.ContainsKey("SelectedItem"))
                MentalCareFacility = (Entity.MentalHealthFacility) PassingParameters["SelectedItem"];
            
            IsBusy = false;
        }

        private void GoBack() => NavigateBackHelper(_navigationStackService, _navigationService);

        private void RedirectToHomePage() => ChangeRootAndNavigateToPageHelper(nameof(ViewNames.HomePage), _navigationStackService, _navigationService);

        private void DialCrisisHotline() => Device.OpenUri(new Uri(String.Format("tel:{0}", PhoneNo)));

        private async Task ViewOnGoogleMap()
        {
            try
            {
                HasLauncedExternalAppMap = await CrossExternalMaps.Current.NavigateTo(Name, MentalCareFacility.LatitudeNonNull, MentalCareFacility.LongitudeNonNull);
            }
            catch (Exception ex)
            {
                if (ex.Source != "Plugin.ExternalMaps")
                    throw;

                HasLauncedExternalAppMap = true;
            }
        }
    }
}
