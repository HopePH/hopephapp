using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;

namespace Yol.Punla.Utility
{
    public class PostFeedHubConnection : HubConnection
    {
        public PostFeedHubConnection(string url) : base(url)
        {
        }

        protected override void OnClosed()
        {
            base.OnClosed();
        }

        protected override void OnMessageReceived(JToken message)
        {
            base.OnMessageReceived(message);
        }      
    }
}
