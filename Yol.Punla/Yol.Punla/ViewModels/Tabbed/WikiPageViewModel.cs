using Acr.UserDialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Barrack;
using Yol.Punla.Localized;
using Yol.Punla.Managers;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;
using Yol.Punla.Utility;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class WikiPageViewModel : ChildViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        private readonly IMentalHealthManager _mentalHealthManager;
        private readonly IKeyValueCacheUtility _keyValueCacheUtility;
        private const int ITEMSPERPAGE = 6;

        public ObservableCollection<ObservableCollection<Entity.Wiki>> CarouselItems { get; set; }
        public DelegateCommand<object> ItemNavigationCommand => new DelegateCommand<object>((param) => ItemNavigation(param));

        private ObservableCollection<Entity.Wiki> _wikis;
        public ObservableCollection<Entity.Wiki> Wikis { get => _wikis; set => SetProperty(ref _wikis, value); }

        public ICommand SortCommand => new DelegateCommand(SortWikis);
        public ICommand ShowOrHideFilterModal => new DelegateCommand<object>(DisplayOrNotFilterBox);
        public ICommand ShowOrHideSortModal => new DelegateCommand<object>(DisplayOrNotSortBox);
        public ICommand ShowOrHideAdModal => new DelegateCommand<object>(async (param) => await DisplayOrNotAdModal(param));
        public IDisposable AlertResult { get; set; }
        public bool IsShowFilterModal { get; set; }
        public bool IsShowSortModal { get; set; }
        public bool IsShowAlphaAd { get; set; }
        public bool IsSortAlphabetical { get; set; }
        public bool IsShowOfflineMessage { get => !IsInternetConnected; }
        public string AdMessage { get; set; } = AppStrings.AdMessage;
        public bool IsForceUpdateVersion { get; set; }

        public WikiPageViewModel(IEventAggregator eventAggregator,
            IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationService navigationService, 
            INavigationStackService navigationStackService,
            IMentalHealthManager mentalHealthManager) : base(eventAggregator, serviceMapper, appUser, navigationService)
        {
            _navigationService = navigationService; 
            _navigationStackService = navigationStackService;
            _mentalHealthManager = mentalHealthManager;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
        }

        #region PREPARE PAGE BINDINGS

        public override void PreparingPageBindings()
        {
            IsBusy = true;
            IsShowAlphaAd = false;
            AdMessage = AppStrings.AdMessage;
            AppUnityContainer.Instance.Resolve<Advertisements>().HasShownPullDownInstructionAlready = false;
            PreparePageBindingAsync();
            PreparePageBindingAsyncFake();
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private async void PreparePageBindingAsync()
        {
            try
            {
                CreateNewHandledTokenSource("InfoPageViewModel.PreparePageBindingAsync", 20);

                Wikis = await Task.Run(async () => 
                {
                    var remoteList = await _mentalHealthManager.GetWikis();
                    return new ObservableCollection<Entity.Wiki>(remoteList);
                }, TokenHandler.Token);

                PreparePageBindingsResult(Wikis, TokenHandler.IsTokenSourceCompleted());
            }
            catch (System.Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        [Conditional("FAKE")]
        private void PreparePageBindingAsyncFake()
        {
            Wikis = new ObservableCollection<Entity.Wiki>(_mentalHealthManager.GetWikis().Result);
            PreparePageBindingsResult(Wikis);
        }

        private void PreparePageBindingsResult(ObservableCollection<Entity.Wiki> wikiList, bool isSuccess = true)
        {
            if (wikiList != null && wikiList.Count > 0)
            {
                CarouselItems = new ObservableCollection<ObservableCollection<Entity.Wiki>>();

                for (int i = 0; i < wikiList.Count ; i++)
                {
                    wikiList[i].IconPath = string.IsNullOrEmpty(wikiList[i].IconPath) ? "questionmark" : wikiList[i].IconPath;
                    wikiList[i].IsOdd = ((i + 1) % 2 == 0) ? false : true;
                }

                for (var i = 0; i <= wikiList.Count / ITEMSPERPAGE; i++)
                    CarouselItems.Add(new ObservableCollection<Entity.Wiki>(wikiList.Skip(i * ITEMSPERPAGE).Take(ITEMSPERPAGE)));
            }

            if (!AppUnityContainer.Instance.Resolve<Advertisements>().HasShownBetaMessageAlready)
            {
                IsShowAlphaAd = true;
                AppUnityContainer.Instance.Resolve<Advertisements>().HasShownBetaMessageAlready = true;
            }

            if (isSuccess && wikiList != null)
                CheckingIfUserNeedsToUpdateTheApp(wikiList);

            IsBusy = false;
        } 

        #endregion

        private void ItemNavigation(object param)
        {
            var wikis = (Entity.Wiki)param;
            PassingParameters.Add("ItemSelected", wikis);
            NavigateToPageHelper(nameof(ViewNames.WikiDetailsPage), _navigationStackService, _navigationService, PassingParameters);
        }

        private void DisplayOrNotFilterBox(object isShow) => IsShowFilterModal = bool.Parse(isShow.ToString());

        private void DisplayOrNotSortBox(object isShow) => IsShowSortModal = bool.Parse(isShow.ToString());

        private async Task DisplayOrNotAdModal(object isShow)
        {
            if (!IsForceUpdateVersion)
                IsShowAlphaAd = bool.Parse(isShow.ToString());
            else
            {
                await UserDialogs.Instance.AlertAsync(AppStrings.AdMessageRequiresAttention, "Error", "Ok");

                if (Device.RuntimePlatform == Device.Android)
                    Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=com.haiyangrpdev.HopePH&hl=en"));
                else
                    UserDialogs.Instance.Alert("Please contact the admin");
            }
        }

        private void SortWikis()
        {
            IsShowSortModal = false;

            if (!IsSortAlphabetical)
                AlertResult = UserDialogs.Instance.Alert(AppStrings.SortSelect, "Please select", "Ok");

            if (IsSortAlphabetical)
            {
                Wikis = new ObservableCollection<Entity.Wiki>(Wikis.OrderBy(x => x.Title));
                CarouselItems = new ObservableCollection<ObservableCollection<Entity.Wiki>>();

                for (int i = 0; i < Wikis.Count; i++)
                {
                    Wikis[i].IconPath = string.IsNullOrEmpty(Wikis[i].IconPath) ? "questionmark" : Wikis[i].IconPath;
                    Wikis[i].IsOdd = ((i + 1) % 2 == 0) ? false : true;
                }

                for (var i = 0; i <= Wikis.Count / ITEMSPERPAGE; i++)
                    CarouselItems.Add(new ObservableCollection<Entity.Wiki>(Wikis.Skip(i * ITEMSPERPAGE).Take(ITEMSPERPAGE)));
            }
        }

        private void CheckingIfUserNeedsToUpdateTheApp(ObservableCollection<Entity.Wiki> wikiList)
        {          
            var oneOfTheWiki = wikiList.Where(x => !string.IsNullOrEmpty(x.ForceToVersionNo)).FirstOrDefault();
            var appCurrentVersion = _keyValueCacheUtility.GetUserDefaultsKeyValue("AppCurrentVersion");

            if (oneOfTheWiki != null && appCurrentVersion != null)
            {
                string[] appCurrentVersionParsed = appCurrentVersion.Split('.');
                string[] forceToVersionParsed = oneOfTheWiki.ForceToVersionNo.Split('.');

                if (Device.RuntimePlatform == Device.iOS)
                    forceToVersionParsed = oneOfTheWiki.ForceToVersionNoIOS.Split('.');

                if (appCurrentVersionParsed.Count() == 2 && forceToVersionParsed.Count() == 2)
                {  
                    IsForceUpdateVersion = int.Parse(forceToVersionParsed[0]) > int.Parse(appCurrentVersionParsed[0]);

                    if (int.Parse(forceToVersionParsed[0]) == int.Parse(appCurrentVersionParsed[0]))
                        IsForceUpdateVersion = (int.Parse(forceToVersionParsed[1]) > int.Parse(appCurrentVersionParsed[1]));

                    if(!IsShowAlphaAd)
                        IsShowAlphaAd = IsForceUpdateVersion;

                    if (IsForceUpdateVersion)
                        AdMessage = AppStrings.ForceUpdateMessage;
                }
            }
        }
    }
}
