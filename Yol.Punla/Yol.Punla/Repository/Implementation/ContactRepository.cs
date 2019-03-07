using System.Linq;
using Yol.Punla.AttributeBase;
using Yol.Punla.Entity;

namespace Yol.Punla.Repository
{
    [ModuleIgnore]
    public class ContactRepository : Database, IContactRepository
    {
        public ContactRepository(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks) { }

        public Contact GetUserProfileFromLocal(string emailAdd) => this.Query<Contact>(string.Format("select * from Contact order by Id")).FirstOrDefault();

        public Contact GetContactByRemoteId(int id) => Query<Contact>($"select * from Contact where RemoteId={id}").FirstOrDefault();

        public void DeleteTableByType<T>() => base.DeleteAll<T>();

        public void LogoutClient() => DeleteTables();
    }
}
