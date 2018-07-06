using SQLite;

namespace Yol.Punla.Entity
{
    public class LocalTableTracking
    {
        [PrimaryKey]
        public string TableName { get; set; }
        public string LastUpdated { get; set; }
    }
}
