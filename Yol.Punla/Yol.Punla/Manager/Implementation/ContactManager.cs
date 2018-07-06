using System.Diagnostics;
using System.Threading.Tasks;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Entity;
using Yol.Punla.GatewayAccess;
using Yol.Punla.Repository;
using Yol.Punla.Utility;

namespace Yol.Punla.Managers
{
    [DefaultModuleInterfacedFake(ParentInterface = typeof(IContactManager))]
    [DefaultModuleInterfaced(ParentInterface = typeof(IContactManager))]
    public class ContactManager : ManagerBase, IContactManager
    {
        private readonly IContactRepository _userRepository;
        private readonly IContactService _userService;
        private readonly IKeyValueCacheUtility _keyValueCachedUtility;
        private Contact _item;

        public ContactManager(ILocalTableTrackingRepository localTableTrackingRepository,
            IContactRepository userRepository,
            IContactService userService) : base(localTableTrackingRepository)
        {
            _userRepository = userRepository;
            _userService = userService;
            _keyValueCachedUtility = AppUnityContainer.InstanceDependencyService.Get<IKeyValueCacheUtility>();
        }

        public async Task<Contact> GetContact(string fbEmail, string FbId, bool isGetFromRest = true)
        {
            try
            {
                _item = _userRepository.GetUserProfileFromLocal(fbEmail);

                if (_item == null || isGetFromRest)
                {
                    _item = await _userService.GetUserProfile(fbEmail, FbId);
                    _userRepository.UpdateItem(_item);
                }

                return _item;
            }
            catch (SQLite.SQLiteException)
            {
                return _item;
            }
        }

        public Task<int> SaveDetailsToRemoteDB(Entity.Contact item)
        {
            return _userService.PostReceiver(item);
        }

        public Task<string> SendVerificationCode(string emailAddress)
        {
            return _userService.PostVerificationCode(emailAddress);
        }

        public async Task<bool> CheckIfEmailExists(string emailAddress, string companyName)
        {
            var userFound = await _userService.GetUserViaEmail(emailAddress, companyName);

            if (userFound != null)
                return true;
            else
                return false;
        }

        public Contact GetCurrentContactFromLocal()
        {
            try
            {
                _item = _userRepository.GetUserProfileFromLocal();
                return _item;
            }
            catch (SQLite.SQLiteException)
            {
                return _item;
            }
        }

        public void LogoutUser()
        {
            try
            {
                Debug.WriteLine("HOPEPH Receiver manager remove the waslogin obj.");
                _userRepository.LogoutClient();
                _keyValueCachedUtility.RemoveKeyObject("WasLogin");
            }
            catch (SQLite.SQLiteException)
            {
            }
        }

        public void SaveNewDetails(Contact item)
        {
            try
            {
                var existingUser = _userRepository.GetUserProfileFromLocal(item.EmailAdd);

                if (existingUser == null)
                {
                    _userRepository.UpdateItem(item);
                    return;
                }

                if (existingUser.EmailAdd == null && item.EmailAdd == null && item.UserName != null)
                {
                    existingUser.EmailAdd = item.UserName;
                    item.EmailAdd = item.UserName;
                }

                if (existingUser.EmailAdd != null && existingUser.EmailAdd.Trim() != item.EmailAdd.Trim())
                {
                    _userRepository.DeleteTableByType<Contact>();
                    _userRepository.UpdateItem(item);
                }
                else
                {
                    existingUser.RemoteId = item.RemoteId;
                    existingUser.Password = item.Password;
                    existingUser.EmailAdd = item.EmailAdd;
                    existingUser.Password = item.Password;
                    existingUser.FirstName = item.FirstName;
                    existingUser.LastName = item.LastName;
                    existingUser.PhotoURL = item.PhotoURL;
                    existingUser.FBLink = item.FBLink;
                    existingUser.Birthdate = item.Birthdate;
                    existingUser.GenderCode = item.GenderCode;
                    existingUser.IsActive = item.IsActive;
                    existingUser.MobilePhone = item.MobilePhone;
                    existingUser.Latitude = item.Latitude;
                    existingUser.Longitude = item.Longitude;
                    existingUser.Address = item.Address;
                    existingUser.Country = item.Country;
                    existingUser.MobilePhone = item.MobilePhone;
                    _userRepository.UpdateItem(existingUser);
                }
            }
            catch (SQLite.SQLiteException)
            {
            }
        }
    }
}
