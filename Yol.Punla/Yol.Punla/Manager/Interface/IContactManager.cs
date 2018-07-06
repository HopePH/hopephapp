using System.Threading.Tasks;

namespace Yol.Punla.Managers
{
    public interface IContactManager
    {
        Task<Entity.Contact> GetContact(string fbEmail,string FbId, bool isGetFromRest = true);
        Task<int> SaveDetailsToRemoteDB(Entity.Contact item);
        Task<string> SendVerificationCode(string emailAddress);
        Task<bool> CheckIfEmailExists(string emailAddress, string companyName);

        Entity.Contact GetCurrentContactFromLocal();
        void SaveNewDetails(Entity.Contact item);
        void LogoutUser();
    }
}
