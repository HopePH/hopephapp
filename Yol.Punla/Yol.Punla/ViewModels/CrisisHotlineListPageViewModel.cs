using Prism.Navigation;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Managers;
using Yol.Punla.Mapper;
using Yol.Punla.NavigationHeap;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class CrisisHotlineListPageViewModel : ViewModelBase
    {
        private readonly IMentalHealthManager _mentalHealthManager;
        private readonly INavigationService _navigationService;
        private readonly INavigationStackService _navigationStackService;
        
        private Entity.MentalHealthFacility _currentSelectedItem;
        public Entity.MentalHealthFacility CurrentSelectedItem
        {
            get => _currentSelectedItem;
            set
            {
                SetProperty(ref _currentSelectedItem, value);

                if (value != null)
                {
                    DialCrisisHotline(_currentSelectedItem);
                    CurrentSelectedItem = null;
                }
            }
        }

        public IEnumerable<Entity.MentalHealthFacility> CrisisHotlines { get; set; }
        public int CrisisHotlineListQty { get; set; }
        public string HotlineDialed { get; set; }

        public CrisisHotlineListPageViewModel(IServiceMapper serviceMapper, IAppUser appUser,
            IMentalHealthManager mentalHealthManager,
            INavigationService navigationService,
            INavigationStackService navigationStackService) : base(navigationService)
        {
            _navigationService = navigationService;
            _navigationStackService = navigationStackService;
            _mentalHealthManager = mentalHealthManager;
        }

        #region Prepare Page Bindings
        
        public override void PreparingPageBindings()
        {
            IsBusy = true;
            PreparePageBindingsAsync();
            PreparePageBindingsAsyncFake();
        }

        [Conditional("DEBUG"), Conditional("TRACE")]
        private async void PreparePageBindingsAsync()
        {
            try
            {
                CreateNewHandledTokenSource("CrisisHotlineListPageViewModel.PreparePageBindingsAsync");

                CrisisHotlines = await Task.Run(async () =>
                {
                    return await _mentalHealthManager.GetCrisisHotlines();
                }, TokenHandler.Token);

                PreparePageBindingsResult(CrisisHotlines, TokenHandler.IsTokenSourceCompleted());
            }
            catch (Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex, true);
            }
        }

        [Conditional("FAKE")]
        private void PreparePageBindingsAsyncFake()
        {
            CrisisHotlines = _mentalHealthManager.GetCrisisHotlines().Result;
            PreparePageBindingsResult(CrisisHotlines);
        }

        private void PreparePageBindingsResult(IEnumerable<Entity.MentalHealthFacility> crisisHotlineList, bool isSuccess = true)
        {
            if (crisisHotlineList != null)
                CrisisHotlineListQty = crisisHotlineList.Count();

            IsBusy = false;
        }

        #endregion

        private void DialCrisisHotline(Entity.MentalHealthFacility item)
        {
            if(item != null)
            {
                HotlineDialed = item.PhoneNumber.ToString();
                Device.OpenUri(new Uri(String.Format("tel:{0}", HotlineDialed)));
            }
        }
    }
}
