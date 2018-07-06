using System.Collections.Generic;
using System.Linq;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Repository
{
    [ModuleIgnore]
    public class LocalTableTrackingRepository : Database, ILocalTableTrackingRepository
    {
        public LocalTableTrackingRepository(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks)
        {

        }

        public void DeleteTableByType<T>() => base.DeleteAll<T>();

        public Entity.LocalTableTracking GetTrackedTable(string tableName)
        {
            var entity = this.Query<Entity.LocalTableTracking>("select * from LocalTableTracking where TableName ='" + tableName + "'").FirstOrDefault();
            return entity;
        }

        public IEnumerable<Entity.LocalTableTracking> GetTrackedTables() => this.Query<Entity.LocalTableTracking>("select * from LocalTableTracking");
    }
}
