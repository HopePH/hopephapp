namespace Yol.Punla.Repository
{
    public interface IContactRepository
    {      
        Entity.Contact GetUserProfileFromLocal(string emailAddress = "");

        void LogoutClient();
        void UpdateItem<T>(T item);
        void DeleteTableByType<T>();
    }
}
