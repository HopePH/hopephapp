using Prism.Events;
using Yol.Punla.Messages;

namespace Yol.Punla.ViewModels
{
    public class DeletePostFeedToHubResultCodeEventModel : PubSubEvent<HttpResponseMessage<int>> { }
}
