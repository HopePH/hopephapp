
namespace Yol.Punla.Contract
{
    public class PostFeedK
    {
        public int PostFeedID { get; set; }
        public int PostFeedParentId { get; set; }
        public int PostFeedLevel { get; set; }
        public string Title { get; set; }
        public string ContentText { get; set; }
        public string ContentURL { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FBPhotoURL { get; set; }
        public string ContactPhotoURL { get; set; }
        public int CommentQty { get; set; }
        public int LikeQty { get; set; }
        public bool IsDelete { get; set; }
        public bool IsSelfLike { get; set; }
        public string AliasName { get; set; }
        public string ContactsWhoLiked { get; set; }
        public string ContactGender { get; set; }
        public bool HasUnReadComments { get; set; }
    }
}
