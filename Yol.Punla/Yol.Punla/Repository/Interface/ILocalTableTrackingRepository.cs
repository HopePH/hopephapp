using System.Collections.Generic;

namespace Yol.Punla.Repository
{
    public interface ILocalTableTrackingRepository
    {
        Entity.LocalTableTracking GetTrackedTable(string tableName);
        IEnumerable<Entity.LocalTableTracking> GetTrackedTables();

        void UpdateItem<T>(T item);
        void DeleteTableByType<T>();
    }
}
