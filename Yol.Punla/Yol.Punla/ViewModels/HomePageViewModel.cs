using Acr.UserDialogs;
using FFImageLoading.Forms;
using Plugin.Geolocator;
using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.Mapper;
using Yol.Punla.Model;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;
using Yol.Punla.Views;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class HomePageViewModel : ViewModelBase
    {
        private readonly INavigationStackService _navigationStackService;
        private readonly INavigationService _navigationService;
        private readonly IMentalHealthManager _mentalHealthManager;
        private readonly IHelperUtility _helperUtility;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;

        private Entity.MentalHealthFacility _currentSelectedItem;
        public Entity.MentalHealthFacility CurrentSelectedItem
        {
            get => _currentSelectedItem;
            set
            {
                SetProperty(ref _currentSelectedItem, value);

                if (value != null)
                {
                    GoToDetails(_currentSelectedItem);
                    CurrentSelectedItem = null;
                }
            }
        }

        private List<string> _specializationsList = new List<string>();
        public List<string> SpecializationList { get => _specializationsList; set => _specializationsList = value; }

        public IEnumerable<CachedImage> Images { get; set; }
        public ICommand ContactFacilityCommand => new DelegateCommand<Entity.MentalHealthFacility>(ContactFacility);
        public ICommand FilterCommand => new DelegateCommand(async () => await FilterHomePage());
        public ICommand SortCommand => new DelegateCommand(async () => await SortHomePage());
        public ICommand ShowOrHideFilterModal => new DelegateCommand<object>(DisplayOrNotFilterBox);
        public ICommand ShowOrHideSortModal => new DelegateCommand<object>(DisplayOrNotSortBox);
        public ICommand RefreshMentalCareCommand => new DelegateCommand(RefreshMentalCareFacilities);
        public IDisposable AlertResult { get; set; }
        public ObservableCollection<Entity.MentalHealthFacility> MentalFacilities { get; set; }
        public TimeSpan CachedImagesDuration { get; set; }
        public int Radius { get; set; }
        public int Rating { get; set; }
        public bool IsRefreshingList { get; set; }
        public bool IsShowFilterModal { get; set; }
        public bool IsShowSortModal { get; set; }
        public bool IsSortByLocation { get; set; }
        public bool IsSortByRating { get; set; }
        public bool IsAlcoholAndDrugTreatmentOn { get; set; }
        public bool IsRehabilitationCenter { get; set; }
        public bool IsBehavioralProblems { get; set; }
        public bool IsMentalPsychologicalProblems { get; set; }
        public bool IsTherapeutic { get; set; }
        public bool IsShowOfflineMessage { get => !IsInternetConnected; }
        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }
        public string PhoneNoDialed { get; set; }

        public HomePageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationStackService navigationStackService, 
            INavigationService navigationService,
            IMentalHealthManager mentalHealthManager)
            : base(serviceMapper, appUser)
        {
            _navigationStackService = navigationStackService;
            _navigationService = navigationService;
            _mentalHealthManager = mentalHealthManager;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
            _helperUtility = AppUnityContainer.InstanceDependencyService.Get<IHelperUtility>();
        }

        #region Prepare Page Bindings

        public override void PreparingPageBindings()
        {
            IsBusy = true;
            Title = "Home Page";
            CachedImagesDuration = TimeSpan.FromDays(7.0);

            if(PassingParameters != null && PassingParameters.ContainsKey("ShowFilterModal"))
                IsShowFilterModal = (bool)PassingParameters["ShowFilterModal"];

            if (PassingParameters != null && PassingParameters.ContainsKey("Radius"))
                Radius = (int)PassingParameters["Radius"];

            if (PassingParameters != null && PassingParameters.ContainsKey("Rating"))
                Rating = (int)PassingParameters["Rating"];

            if (PassingParameters != null && PassingParameters.ContainsKey("Specializations"))
                SpecializationList = (List<string>)PassingParameters["Specializations"];

            PreparePageBindingsAsync();
            PreparePageBindingsAsyncFake();
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private async void PreparePageBindingsAsync(bool isForceToGetToTheRest = false)
        {
            try
            {                
                CreateNewHandledTokenSource("HomePageViewModel.PreparePageBindingsAsync", 20);

                var results = await Task.Run(async () =>
                {
                    return await _mentalHealthManager.GetAllFacilities(isForceToGetToTheRest);
                }, TokenHandler.Token);

                PreparePageBindingsResult(results, TokenHandler.IsTokenSourceCompleted());
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        [Conditional("FAKE")]
        private void PreparePageBindingsAsyncFake(bool isForceToGetToTheRest = false)
        {
            var results = _mentalHealthManager.GetAllFacilities(isForceToGetToTheRest).Result;
            PreparePageBindingsResult(results);
        }

        private void PreparePageBindingsResult(IEnumerable<Entity.MentalHealthFacility> listMentalFacility, bool isSuccess = true)
        {
           if(listMentalFacility!= null && listMentalFacility.Count() > 0)
                MentalFacilities = new ObservableCollection<Entity.MentalHealthFacility>(listMentalFacility);

            IsBusy = false;
        }

        #endregion

        private void RefreshMentalCareFacilities()
        {
            if (ProcessInternetConnection(true))
            {
                IsBusy = true;
                IsRefreshingList = true;
                PreparePageBindingsAsync(true);
                PreparePageBindingsAsyncFake(true);
            }

            IsRefreshingList = false;
        }

        private void ContactFacility(Entity.MentalHealthFacility item)
        {
            if (item?.PhoneNumber != null && item.PhoneNumber.Length > 0)
            {
                PhoneNoDialed = item.PhoneNumber;
                Device.OpenUri(new Uri(String.Format("tel:{0}", PhoneNoDialed)));
            }
        }

        private void GoToDetails(Entity.MentalHealthFacility item)
        {
            PassingParameters.Add("SelectedItem", item);
            NavigateToPageHelper(nameof(ViewNames.MentalCareDetailsPage), _navigationStackService, _navigationService, PassingParameters);
        }

        private async Task FilterHomePage()
        {
            IsBusy = true;
            IsShowFilterModal = false;

            if (Rating > 0 || IsAlcoholAndDrugTreatmentOn || IsRehabilitationCenter || IsBehavioralProblems || IsMentalPsychologicalProblems || IsTherapeutic)
            {
                PopulateSpecializations();
                UserDialogs.Instance.Alert(AppStrings.NotSupportedRatingAndSpecializations, "Not supported", "Ok");
            }

            if (Radius > 0)
            {
                await GetDeviceLocationAsync();
                ComputeDistance();

                MentalFacilities = new ObservableCollection<Entity.MentalHealthFacility>(MentalFacilities.Where(x => x.DistanceFromUser <= Radius));
            }

            IsBusy = false;
        }

        private async Task SortHomePage()
        {
            IsShowSortModal = false;
            IsBusy = true;

            if (!(IsSortByRating || IsSortByLocation))
                AlertResult = UserDialogs.Instance.Alert(AppStrings.SortSelect, "Please select", "Ok");

            if (IsSortByLocation)
            {
                await GetDeviceLocationAsync();
                SortByLocation();
            }

            if (IsSortByRating)
            {
                MentalFacilities = new ObservableCollection<Entity.MentalHealthFacility>(MentalFacilities.OrderByDescending(mf => mf.Rating));
                UserDialogs.Instance.Alert(AppStrings.NotSupportedSortByRating, "Not supported", "Ok");
            }
           
            IsBusy = false;
        }

        private async Task GetDeviceLocationAsync()
        {
            try
            {
                var lat = _keyValueCacheUtility.GetUserDefaultsKeyValue("CurrentLatitude");
                var longi = _keyValueCacheUtility.GetUserDefaultsKeyValue("CurrentLongitude");

                if (!(string.IsNullOrEmpty(lat) && string.IsNullOrEmpty(longi)))
                {
                    CurrentLatitude = double.Parse(lat);
                    CurrentLongitude = double.Parse(longi);
                }
                else
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 10;

                    var position = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(10));
                    CurrentLatitude = position?.Latitude ?? 0;
                    CurrentLongitude = position?.Longitude ?? 0;

                    if (CurrentLatitude > 0 && CurrentLongitude > 0)
                    {
                        _keyValueCacheUtility.GetUserDefaultsKeyValue("CurrentLatitude", CurrentLatitude.ToString());
                        _keyValueCacheUtility.GetUserDefaultsKeyValue("CurrentLongitude", CurrentLongitude.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }
        
        private void SortByLocation()
        {
            ComputeDistance();
            MentalFacilities = new ObservableCollection<Entity.MentalHealthFacility>(MentalFacilities.OrderBy(mf => mf.DistanceFromUser));
        }

        private void ComputeDistance()
        {
            foreach (var item in MentalFacilities)
            {
                var distanceFromUser = GeolocationHelper.DistanceTo(CurrentLatitude, CurrentLongitude, item.LatitudeNonNull, item.LongitudeNonNull);
                item.DistanceFromUser = distanceFromUser;
            }
        }

        private void PopulateSpecializations()
        {
            if (_specializationsList != null)
            {
                if (IsAlcoholAndDrugTreatmentOn)
                    _specializationsList.Add(Specializations.AlcoholAndDrugTreatment);

                if (IsRehabilitationCenter)
                    _specializationsList.Add(Specializations.RehabilitationCenter);

                if (IsBehavioralProblems)
                    _specializationsList.Add(Specializations.BehavioralProblems);

                if (IsMentalPsychologicalProblems)
                    _specializationsList.Add(Specializations.MentalPsychologicalProblems);

                if (IsTherapeutic)
                    _specializationsList.Add(Specializations.Therapeutic);
            }
        }

        private void DisplayOrNotFilterBox(object isShow)
        {
            IsShowFilterModal = bool.Parse(isShow.ToString());
            ResetFilterControls();
        }

        private void DisplayOrNotSortBox(object isShow) => IsShowSortModal = bool.Parse(isShow.ToString());

        private void ResetFilterControls()
        {
            IsAlcoholAndDrugTreatmentOn = false;
            IsRehabilitationCenter = false;
            IsBehavioralProblems = false;
            IsMentalPsychologicalProblems = false;
            IsTherapeutic = false;
            Radius = 0;
            Rating = 0;
        }
    }
}
