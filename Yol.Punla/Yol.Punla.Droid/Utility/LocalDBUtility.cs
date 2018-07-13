using System.IO;
using Xamarin.Forms;
using Yol.Punla.Utility;

[assembly: Dependency(typeof(Yol.Punla.Droid.Utility.LocalDBUtility))]
namespace Yol.Punla.Droid.Utility
{
    public class LocalDBUtility : ILocalDBUtility
    {
        public string GetDBPath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}