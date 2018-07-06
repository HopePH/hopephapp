
using SQLite;

namespace Yol.Punla.Entity
{
    public  class NavigationStackDefinition
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string PageName { get; set; }
    }
}
