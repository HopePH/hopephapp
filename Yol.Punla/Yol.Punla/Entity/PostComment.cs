using SQLite;
using System.Collections.Generic;

namespace Yol.Punla.Entity
{
    public class PostComment 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int CommentatorId { get; set; }      // The ID of the person who write comment
        public string Message { get; set; }
        public string DateCommented { get; set; }

        [Ignore]
        public List<PostComment> SubComments { get; set; }
    }
}
