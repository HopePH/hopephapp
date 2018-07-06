
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Utility;

namespace Yol.Punla.Authentication
{
    [DefaultModuleInterfacedAttribute(ParentInterface = typeof(IAppUser))]
    [DefaultModuleInterfacedFakeAttribute(ParentInterface = typeof(IAppUser))]
    public class AppUser : IAppUser
    {
        private readonly IKeyValueCacheUtility _keyValueCache;

        public bool IsAuthenticated {
            get {
                var wasLoginAlready = _keyValueCache.GetUserDefaultsKeyValue("WasLogin");

                if (bool.TryParse(wasLoginAlready, out bool hasUserLogonAlready))
                    return hasUserLogonAlready;

                return hasUserLogonAlready;
            }
        }

        public bool SignUpCompleted
        {
            get
            {
                var hasUserCompletedSignUp = _keyValueCache.GetUserDefaultsKeyValue("WasSignUpCompleted");

                if (bool.TryParse(hasUserCompletedSignUp, out bool completedSignUpResult))
                    return completedSignUpResult;

                return completedSignUpResult;
            }
        }

        public AppUser()
        {
            _keyValueCache = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
        }
    }
}
