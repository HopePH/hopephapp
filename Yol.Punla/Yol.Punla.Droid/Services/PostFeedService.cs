using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Yol.Punla.Droid.Utility;
using Yol.Punla.Utility;

namespace Yol.Punla.Droid.Services
{
    [Service]
    public class PostFeedService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            try
            {
                Log.Info("DroidPlatform", "HOPEPH OnStartCommand running (PostFeed Service).");
                Contract.ContactK currentUser = ExtendedDataHolder.Instance.HasExtra("CurrentUser") ? 
                    JsonConvert.DeserializeObject<Contract.ContactK>(ExtendedDataHolder.Instance.GetExtra("CurrentUser").ToString()) : null;
                Contract.PostFeedK currentPost = ExtendedDataHolder.Instance.HasExtra("CurrentPost") ? 
                    JsonConvert.DeserializeObject<Contract.PostFeedK>(ExtendedDataHolder.Instance.GetExtra("CurrentPost").ToString()) : null;

                string operation = "";

                if (ExtendedDataHolder.Instance.HasExtra("Operation"))
                    operation = ExtendedDataHolder.Instance.GetExtra("Operation").ToString();
                else
                    throw new ArgumentNullException("Operation in PostFeedService android is null.");

                Task.Run(async () =>
                {
                    switch (operation.ToLower())
                    {
                        case "deletepostfeed":
                            await PostFeedSpokeStatic.DeleteSelfPostToRemote(currentPost.PostFeedID);
                            break;
                        case "likeorunlikepost":
                            await PostFeedSpokeStatic.LikeOrUnlikePost(true, currentPost.PostFeedID, currentUser.Id);
                            break;
                        case "addupdatepost":
                            await PostFeedSpokeStatic.AddOrUpdatePostFeed(currentPost);
                            break;
                        case "logontohub":
                        default:
                            await PostFeedSpokeStatic.LogonToHub(currentUser.UserName, currentUser.Password);
                            break;
                    }
                });
            }
            catch (System.OperationCanceledException ex)
            {
                ProcessErrorReportingForHockeyApp(ex);
            }
            catch (System.Exception ex)
            {
                ProcessErrorReportingForHockeyApp(ex);
            }

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void ProcessErrorReportingForHockeyApp(Exception ex)
        {
#if FAKE
            //chito. do not register to the hockeyapp when unittesting
#else
            //chito. HEA this just means Handled Exception, just make it shorter. Also, there's no need to put if this is Android or IOS since they have unique hockeyid per platform        
            HockeyApp.MetricsManager.TrackEvent(string.Format("HE.{0}", ex.Message ?? ""));
#endif
        }
    }
}