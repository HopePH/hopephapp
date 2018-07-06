using SQLite;

namespace Yol.Punla.Entity
{
    public class PostFeedLike
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PostFeedID { get; set; }
        public int ContactID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AliasName { get; set; }
        public string ContactPhotoURL { get; set; }
    }
}
