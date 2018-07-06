using Yol.Punla.AttributeBase;

namespace Yol.Punla.Messages
{
    [ModuleIgnore]
    public class PostFeedMessage
    {
        public Contract.ContactK CurrentUser { get; set; }
        public Contract.PostFeedK CurrentPost { get; set; }
    }
}
