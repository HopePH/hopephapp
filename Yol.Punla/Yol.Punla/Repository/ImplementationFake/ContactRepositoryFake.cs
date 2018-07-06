using Prism.Services;
using System.Linq;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
using Yol.Punla.Utility;
using Unity;

namespace Yol.Punla.Repository
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IContactRepository))]
    public class ContactRepositoryFake : IContactRepository
    {
        public Contact GetUserProfileFromLocal(string emailAdd)
        {
            var keyValueCacheUtility = AppUnityContainer.Instance.Resolve<IDependencyService>().Get<IKeyValueCacheUtility>();
            bool isLogon = bool.Parse(keyValueCacheUtility.GetUserDefaultsKeyValue("WasLogin"));

            if (!isLogon)
                return null;

            return FakeData.FakeUsers.Contacts.Where(c => c.EmailAdd == emailAdd).FirstOrDefault();
        }

        public void LogoutClient()
        {
            //chito.delete tables in reality but here just do nothing since this is fake
        }

        public void UpdateItem<T>(T item) { }

        public void DeleteTableByType<T>() { }
    }
}
