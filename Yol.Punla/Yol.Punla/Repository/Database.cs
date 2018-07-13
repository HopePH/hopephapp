using SQLite;
using System.Diagnostics;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Repository
{
    [ModuleIgnore]
    public abstract class Database : SQLiteConnection
    {
        protected Database(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks)
        {
           
        }

        public void CreateTablesOnce()
        {
            Debug.WriteLine("HOPEPH Creating tables if not yet exist.");
            CreateTable<Entity.NavigationStackDefinition>();
            CreateTable<Entity.MentalHealthFacility>();
            CreateTable<Entity.LocalTableTracking>();
            CreateTable<Entity.Wiki>();
            CreateTable<Entity.Contact>();
            CreateTable<Entity.PostComment>();
            CreateTable<Entity.PostFeed>();
            CreateTable<Entity.PostFeedLike>();
        }

        public void UpdateItem<T>(T item)
        {
            Debug.WriteLine("HOPEPH Updating " + item +  ".");

            if (item != null)
            {
                var updateItem = (T)item;
                int rowsAffected = Update(item);

                if (rowsAffected < 1)
                {
                    Debug.WriteLine("HOPEPH Inserting " + item + ".");
                    Insert(updateItem);
                }
                
            }
        }

        public void DeleteTables()
        {
            Debug.WriteLine("HOPEPH Deleting all user information...");
            DeleteAll<Entity.MentalHealthFacility>();
            DeleteAll<Entity.LocalTableTracking>();
            DeleteAll<Entity.Wiki>();
            DeleteAll<Entity.Contact>();
            DeleteAll<Entity.PostComment>();
            DeleteAll<Entity.PostFeed>();
        }

    }
}
