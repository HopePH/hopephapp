using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Yol.Punla.Utility;

namespace Yol.Punla.iOS.Services
{
    internal class PostFeedService
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start(Contract.PostFeedK currentPost, Contract.ContactK currentUser, string operation)
        {
            Console.WriteLine("HOPEPH Creating task id for background ios");
            _cts = new CancellationTokenSource();

            try
            {
                switch (operation.ToLower())
                {
                    case "deletepostfeed":
                        Console.WriteLine("HOPEPH Deleting Post Feed");
                        _taskId = UIApplication.SharedApplication.BeginBackgroundTask("DeletingPostFeed", OnExpiration);
                        await PostFeedSpokeStatic.DeleteSelfPostToRemote(currentPost.PostFeedID);
                        UIApplication.SharedApplication.EndBackgroundTask(_taskId);
                        break;
                    case "likeorunlikepost":
                        Console.WriteLine("HOPEPH Liking or Unliking Post Feed");
                        _taskId = UIApplication.SharedApplication.BeginBackgroundTask("LikingOrUnlikingPost", OnExpiration);
                        await PostFeedSpokeStatic.LikeOrUnlikePost(true, currentPost.PostFeedID, currentUser.Id);
                        UIApplication.SharedApplication.EndBackgroundTask(_taskId);
                        break;
                    case "addupdatepost":
                        Console.WriteLine("HOPEPH Adding or Updating Post Feed");
                        _taskId = UIApplication.SharedApplication.BeginBackgroundTask("AddingUpdatingPostFeed", OnExpiration);
                        await PostFeedSpokeStatic.AddOrUpdatePostFeed(currentPost);
                        UIApplication.SharedApplication.EndBackgroundTask(_taskId);
                        break;
                    case "logontohub":
                    default:
                        _taskId = UIApplication.SharedApplication.BeginBackgroundTask("LogonToHub", OnExpiration);
                        await PostFeedSpokeStatic.LogonToHub(currentUser.UserName, currentUser.Password);
                        UIApplication.SharedApplication.EndBackgroundTask(_taskId);
                        break;
                }
            }
            catch (OperationCanceledException oex)
            {
                Console.WriteLine(string.Format("HOPEPH An error occured in backgrounding IOS {0}.", oex.Message));
            }
            finally
            {
                if (_cts.IsCancellationRequested)
                {
                    Console.WriteLine("HOPEPH Cancelled token.");
                }
            }
        }
        
        public void Stop()
        {
            StopSession();
        }

        void OnExpiration()
        {
            StopSession();
        }

        void StopSession()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();
                _cts.Cancel();
            }
        }
    }
}