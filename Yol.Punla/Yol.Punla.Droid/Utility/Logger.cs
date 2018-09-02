using System;
using Mindscape.Raygun4Net;
using Xamarin.Forms;
using Yol.Punla.Utility;

[assembly: Dependency(typeof(DrTalk.Droid.Utility.Logger))]
namespace DrTalk.Droid.Utility
{
    public class Logger : ILogger
    {
        public void Log(Exception exception)
        {
            if (exception != null)
            {
                try
                {
                    RaygunClient.Current.SendInBackground(exception);
                }
                catch { }
            }
        }
    }
}