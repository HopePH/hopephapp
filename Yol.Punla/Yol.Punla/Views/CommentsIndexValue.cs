namespace Yol.Punla.Views
{
    public class CommentsIndexValue
    {
        public int Index { get; set; }
        public Entity.PostFeed Comment { get; set; }
        public bool IsLocallySaved { get; set; }
    }
}
