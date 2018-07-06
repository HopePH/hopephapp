using SQLite;

namespace Yol.Punla.Entity
{
    public class Wiki
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string IconPath { get; set; }
        public string Content { get; set; }
        public bool IsOdd { get; set; }
        public string ForceToVersionNo { get; set; }
        public string ForceToVersionNoIOS { get; set; }
    }
}
