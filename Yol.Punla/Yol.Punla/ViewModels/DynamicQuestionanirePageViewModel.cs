using Prism.Commands;
using Prism.Navigation;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Yol.Punla.AttributeBase;
using Yol.Punla.Entity;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class DynamicQuestionanirePageViewModel : ViewModelBase
    {
        private ChoiceItem _itemSelected;
        public ChoiceItem ItemSelected
        {
            get => _itemSelected;
            set
            {
                SetProperty(ref _itemSelected, value);
                if (value != null)
                {
                    SelectAnswer(_itemSelected);
                    ItemSelected = null;
                }
            }
        }
        public ObservableCollection<ChoiceItem> Choices { get; set; }
        public QuestionTypes QuestionType { get; set; } = QuestionTypes.YESORNO;
        public ICommand SelectYesOrNoCommand => new DelegateCommand<string>(SelectYesOrNoOption);
        public bool IsQuestionTypeYesOrNo { get; set; }
        public bool HasSelection { get; set; } = false;
        public bool IsYesSelected { get; set; } = false;
        public bool IsNoSelected { get; set; }
        public bool HasSelectedYesOrNo { get; set; } = false;

        public DynamicQuestionanirePageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void PreparingPageBindings()
        {
            IsQuestionTypeYesOrNo = (QuestionType == QuestionTypes.YESORNO);

            Choices = new ObservableCollection<ChoiceItem>
            {
                new ChoiceItem{Text = "Checkbox selected", IsSelected=true},
                new ChoiceItem{Text = "Checkbox2 unselected", IsSelected=false},
                new ChoiceItem{Text = "Checkbox3 unselected", IsSelected=false},
                new ChoiceItem{Text = "Checkbox4 unselected", IsSelected=false}
            };

            HasSelection = (Choices.Count(i => i.IsSelected) > 0);
        }

        private void SelectAnswer(ChoiceItem choiceSelected)
        {
            if(choiceSelected != null) choiceSelected.IsSelected = !choiceSelected.IsSelected;
            HasSelection = (Choices.Count(i => i.IsSelected) > 0);
        }

        private void SelectYesOrNoOption(string selection)
        {
            IsYesSelected = (selection.ToString().ToLower() == "yes");
            IsNoSelected = !IsYesSelected;

            HasSelectedYesOrNo = (IsYesSelected || IsNoSelected);
        }
    }
}
