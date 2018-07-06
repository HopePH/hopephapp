using System;
using System.Diagnostics;
using Xamarin.Forms;
using Yol.Punla.Utility;

[assembly: Dependency(typeof(Yol.Punla.iOS.Utility.LocalDBUtility))]
namespace Yol.Punla.iOS.Utility
{
    public class LocalDBUtility : ILocalDBUtility
    {
        public string GetDBPath(string filename)
        {
            Debug.WriteLine("HOPEPH Set the dbPath from IOS.");
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = System.IO.Path.Combine(docFolder, "..", "Library", "Databases");

            if (!System.IO.Directory.Exists(libFolder))
            {
                System.IO.Directory.CreateDirectory(libFolder);
            }

            return System.IO.Path.Combine(libFolder, filename);
        }
    }
}