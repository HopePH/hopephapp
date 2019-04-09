using Prism.Events;
using Yol.Punla.Messages;

namespace Yol.Punla.ViewModels
{
    public class AddUpdatePostFeedToHubResultCodeEventModel : PubSubEvent<HttpResponseMessage<Contract.PostFeedK>> { }
}
