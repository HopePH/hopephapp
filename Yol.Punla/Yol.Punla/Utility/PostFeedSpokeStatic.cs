/*Chito.notes:
 * A. whatever exception occurs here like there is a concurrent liking and deleting as well. One scenario is that when there is concurent liking of post and at the same time it was deleted on the other end
 *    most likely the PostDetails is deleted to the server already and it will cause null to the subscriber spoke.
 **/
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Messages;
using Unity;

namespace Yol.Punla.Utility
{
    [ModuleIgnore]
    public static class PostFeedSpokeStatic
    {   
        private static readonly string HubName = "PostFeedHub";
        private static readonly string HubApi = AppSettingsProvider.Instance.GetValue("PostFeedAPI");
        private static readonly string AbsolutePath = AppSettingsProvider.Instance.GetValue("PostFeedAPI") + "/api/";
        private static string CurrentHubConnectionId = "";
        private static string Iuser = "";
        private static IHubProxy HubProxy = null;
        private static PostFeedHubConnection HubConnection = null;

        //chito.temp get token from the server and connect to the Hub. Add connection headers. user and password will be used in getting token.
        public static async Task LogonToHub(string user, string password)
        {
            if (string.IsNullOrEmpty(Iuser))
                Iuser = user;

            if ((HubConnection != null && HubConnection.State == ConnectionState.Disconnected) || HubConnection == null)
            {
                Debug.WriteLine("HOPEPH Connecting to the hub and get save the hub connection id (Post Feed Spoke).");
                string token = "";
                HubConnection = new PostFeedHubConnection(HubApi);
                HubProxy = HubConnection.CreateHubProxy(HubName);

                Debug.WriteLine("HOPEPH Subs for new/update post.");
                HubProxy.On<PostFeedMessage>("PostFeedPush", message =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            if (!message.CurrentPost.IsDelete && message.CurrentUser.EmailAddress.ToLower().Trim() != Iuser.ToLower().Trim())
                                AppUnityContainer.Instance.Resolve<IEventAggregator>().GetEvent<ViewModels.AddUpdatePostSubsEventModel>().Publish(message);
                        }
                        catch (Exception)
                        {
                            //chito.refer to notes A above
                        }
                    });
                });

                Debug.WriteLine("HOPEPH Subs for delete post.");
                HubProxy.On<PostFeedMessage>("DeletePostFeedPush", message =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            if (message.CurrentPost.IsDelete && message.CurrentUser.EmailAddress.ToLower().Trim() != Iuser.ToLower().Trim())
                                AppUnityContainer.Instance.Resolve<IEventAggregator>().GetEvent<ViewModels.DeletePostFeedSubsEventModel>().Publish(message);
                        }
                        catch (Exception)
                        {
                            //chito.refer to notes A above
                        }
                    });
                });

                Debug.WriteLine("HOPEPH Subs for like or unlike post.");
                HubProxy.On<PostFeedMessage>("LikeOrUnLikeAPostFeedPush", message =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        try
                        {
                            if (!message.CurrentPost.IsDelete && message.CurrentUser.EmailAddress.ToLower().Trim() != Iuser.ToLower().Trim())
                                AppUnityContainer.Instance.Resolve<IEventAggregator>().GetEvent<ViewModels.LikeOrUnlikePostFeedToHubEventModel>().Publish(message);
                        }
                        catch (Exception)
                        {
                            //chito.refer to notes A above
                        }
                    });
                });

                CurrentHubConnectionId = await Task.Run(async () =>
                {
                    await HubConnection.Start();

                    if (HubConnection != null && !string.IsNullOrEmpty(HubConnection.ConnectionId))
                        return HubConnection.ConnectionId;
                    else
                        return "";
                 
                });
            }
        }

        public static async Task LogonToHubFake(string user, string password)
        {
            await Task.Delay(100);
        }

        public static async Task<HttpStatusCode> LikeOrUnlikePost(bool isSupport, int postFeedId, int contactId)
        {
            Debug.WriteLine("HOPEPH Supporting a post feed.");
            var jsonContent = new StringContent("", Encoding.UTF8, "application/json");
            string endPoint = AbsolutePath + "PostFeed/LikeOrUnLikeAPost?postFeedId={0}&contactId={1}";

            HttpClient httpClient = new HttpClient();
            var result = await httpClient.PostAsync(string.Format(endPoint, postFeedId, contactId), jsonContent);
            return result.StatusCode;
        }

        public static async Task AddOrUpdatePostFeed(Contract.PostFeedK newPost)
        {
            var jsonString = JsonConvert.SerializeObject(newPost);
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var postingEndPoint = AbsolutePath + "PostFeed/AddNewPostFeed";

            var result = await PostRemoteAsync<Contract.PostFeedK>(postingEndPoint, jsonContent);
            AppUnityContainer.Instance.Resolve<IEventAggregator>().GetEvent<ViewModels.AddUpdatePostFeedToHubResultCodeEventModel>().Publish(result);
        }

        public static async Task DeleteSelfPostToRemote(int postId)
        {
            Debug.WriteLine("HOPEPH Posting new post feed.");
            var jsonContent = new StringContent("", Encoding.UTF8, "application/json");
            string endPoint = string.Format(AbsolutePath + "PostFeed/DeletePostFeed?postFeedId={0}&companyName=Hopeph", postId);

            HttpClient httpClient = new HttpClient();
            var result = await httpClient.PostAsync(endPoint, jsonContent);
            var resultCode = result.StatusCode;
            AppUnityContainer.Instance.Resolve<IEventAggregator>().GetEvent<ViewModels.DeletePostFeedToHubResultCodeEventModel>().Publish(new HttpResponseMessage<int> { HttpStatusCode = resultCode });
        }

        private static async Task<HttpResponseMessage<T>> PostRemoteAsync<T>(string endPoint, HttpContent httpContent)
        {
            HttpResponseMessage<T> httpStatucCodeMessage = null;
            HttpResponseMessage jsonResponse = new HttpResponseMessage();

            try
            {
                HttpClient httpClient = new HttpClient();
                jsonResponse = await httpClient.PostAsync(endPoint, httpContent);

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var jsonResult = await jsonResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!string.IsNullOrWhiteSpace(jsonResult))
                    {
                        var deserializedObject = JsonConvert.DeserializeObject<T>(jsonResult);
                        httpStatucCodeMessage = new HttpResponseMessage<T> { HttpStatusCode = jsonResponse.StatusCode, Result = deserializedObject };
                        return httpStatucCodeMessage;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(string.Format("HOPEPH An error was in PostFeedSpokeStatic.PostRemoteAsync with message {0}", exception.Message));

                if (exception.InnerException != null)
                    Debug.WriteLine(string.Format("HOPEPH An error was in PostFeedSpokeStatic.PostRemoteAsync with message {0}", exception.InnerException.Message));
            }

            return httpStatucCodeMessage;
        }

        public static void StopConnection()
        {
            if(HubConnection.State != ConnectionState.Disconnected)
                HubConnection.Stop();
        }

        public static ConnectionState GetConnectionState() => HubConnection.State;
    }
}
