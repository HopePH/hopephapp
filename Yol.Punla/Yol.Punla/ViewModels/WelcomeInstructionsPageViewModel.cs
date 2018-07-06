using Prism.Commands;
using PropertyChanged;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.Entity;
using Yol.Punla.Mapper;
using Yol.Punla.ViewModels.Data;
using System.Linq;
using Yol.Punla.NavigationHeap;
using Prism.Navigation;
using Yol.Punla.Utility;
using Yol.Punla.Barrack;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class WelcomeInstructionsPageViewModel : ViewModelBase
    {
        private IKeyValueCacheUtility _keyValueCacheUtility;
        private INavigationStackService _navigationStackService;
        private INavigationService _navigationService;
        private int _instructionIndex;

        public WelcomeInstruction InstructionContent { get; set; }
        public ICommand NextCommand => new DelegateCommand(GoNext);
        public ICommand PrevCommand => new DelegateCommand(GoBack);
        public bool IsNotFirstInstruction { get; set; }

        public WelcomeInstructionsPageViewModel(IServiceMapper serviceMapper, 
            IAppUser appUser,
            INavigationStackService navigationStackService,
            INavigationService navigationService) : base(serviceMapper, appUser)
        {
            WelcomeInstructionsData.Init();
            _navigationStackService = navigationStackService;
            _navigationService = navigationService;
            _keyValueCacheUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
        }

        public override void PreparingPageBindings()
        {
            _instructionIndex = 0;
            InstructionContent = WelcomeInstructionsData.Instructions.Take(1).FirstOrDefault();
            IsBusy = false;
        }

        private void GoNext()
        {
            ++_instructionIndex;
            IsNotFirstInstruction = true;

            if (_instructionIndex < WelcomeInstructionsData.Instructions.Count())
                InstructionContent = WelcomeInstructionsData.Instructions.Skip(_instructionIndex).Take(1).FirstOrDefault();
            else
            {
                _keyValueCacheUtility.GetUserDefaultsKeyValue("WasWelcomeInstructionLoaded", "true");
                ChangeRootAndNavigateToPageHelper(nameof(ViewNames.WikiPage), _navigationStackService, _navigationService, PassingParameters);
            }
        }

        private void GoBack()
        {
            if (_instructionIndex > 0)
            {
                --_instructionIndex;

                if (_instructionIndex == 0)
                    IsNotFirstInstruction = false;
            }
                
            InstructionContent = WelcomeInstructionsData.Instructions.Skip(_instructionIndex).Take(1).FirstOrDefault();
        }
    }
}
