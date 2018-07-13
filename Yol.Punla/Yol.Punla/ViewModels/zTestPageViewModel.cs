using PropertyChanged;
using System.Collections.ObjectModel;
using Yol.Punla.AttributeBase;
using Yol.Punla.Authentication;
using Yol.Punla.FakeData;
using Yol.Punla.Mapper;

namespace Yol.Punla.ViewModels
{
    [ModuleIgnore]
    [DefaultModuleFake]
    [AddINotifyPropertyChangedInterface]
    public class zTestPageViewModel : ViewModelBase
    {
        public ObservableCollection<Entity.PostFeed> PostsList { get; set; }

        public zTestPageViewModel(IServiceMapper serviceMapper, IAppUser appUser) : base(serviceMapper, appUser)
        {
            FakePostFeeds.Init();
        }

        public override void PreparingPageBindings()
        {
            PostsList = new ObservableCollection<Entity.PostFeed>(FakePostFeeds.Posts);
        }
    }
}
