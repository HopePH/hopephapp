using HockeyApp.Android;

namespace Yol.Punla.Droid.Utility
{
    public class CrashListener : CrashManagerListener
    {
        public override bool ShouldAutoUploadCrashes()
        {
            return true;
        }
    }
}