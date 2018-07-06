using Android.App;
using Android.Media;
using Android.Util;
using Java.IO;
using System.Text;
using Xamarin.Forms;
using Yol.Punla.Utility;

[assembly: Dependency(typeof(Yol.Punla.Droid.Utility.KeyValueCacheUtility))]
namespace Yol.Punla.Droid.Utility
{
    public class KeyValueCacheUtility : IKeyValueCacheUtility
    {
        private const string CACHENAME = "HOPECACHE.txt";

        public string GetUserDefaultsKeyValue(string key, string setInitialValueWhenNull = "")
        {
            Log.Info("HOPEPH", string.Format("HOPEPH Android native reading key value of {0}.", key));
            string value = ReadCacheFile(key);

            if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(setInitialValueWhenNull))
            {
                try
                {
                    setInitialValueWhenNull = setInitialValueWhenNull.Replace(":", "|");
                    var context = Forms.Context as Activity;

                    File testFile = new File(context.GetExternalFilesDir(null), CACHENAME);
                    if (!testFile.Exists())
                        testFile.CreateNewFile();

                    BufferedWriter writer = new BufferedWriter(new FileWriter(testFile, true /*append*/));
                    writer.Write(string.Format("{0}:{1},", key, setInitialValueWhenNull));
                    writer.Close();

                    MediaScannerConnection.ScanFile(context,
                                                    new string[] { testFile.ToString() },
                                                    null,
                                                    null);
                }
                catch (IOException e)
                {
                    Log.Error("ReadWriteFile", string.Format("Unable to write to the {0} file.", CACHENAME));
                }

                return "";
            }
            else
                return value;
        }

        public void RemoveKeyObject(string key)
        {
            Log.Info("HOPEPH", string.Format("HOPEPH Android native removing value of {0}.", key));
            string textFromFile = "";
            var context = Forms.Context as Activity;

            File testFile = new File(context.GetExternalFilesDir(null), CACHENAME);
            if (testFile != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                BufferedReader reader = null;

                try
                {
                    reader = new BufferedReader(new FileReader(testFile));
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        textFromFile += line.ToString();
                        textFromFile += "\n";
                    }

                    reader.Close();
                }
                catch (System.Exception e)
                {
                    Log.Error("ReadWriteFile", string.Format("Unable to read the {0} file.", CACHENAME));
                }
            }

            string newTextOfFile = "";

            if (!string.IsNullOrEmpty(textFromFile))
            {
                string[] allValsAndKeys = textFromFile.Split(',');
                int count = allValsAndKeys.Length - 2;

                foreach (var dkey in allValsAndKeys)
                {
                    if (!dkey.Contains(key))
                    {
                        newTextOfFile += dkey + ",";
                    }
                    --count;
                }

                try
                {
                    if (testFile.Exists())
                    {
                        testFile.Delete();
                        testFile.CreateNewFile();
                    }

                    newTextOfFile = newTextOfFile.Replace("\n,", "");

                    BufferedWriter writer = new BufferedWriter(new FileWriter(testFile, true /*append*/));
                    writer.Write(newTextOfFile);
                    writer.Close();

                    MediaScannerConnection.ScanFile(context,
                                                    new string[] { testFile.ToString() },
                                                    null,
                                                    null);

                    string tempTest = ReadCacheFile("WasLogin");
                }
                catch (IOException e)
                {
                    Log.Error("ReadWriteFile", string.Format("Unable to write to the {0} file.", CACHENAME));
                }
            }
        }

        private string ReadCacheFile(string key)
        {
            Log.Info("HOPEPH", string.Format("HOPEPH Android native reading cache file of value {0}.", key));
            string textFromFile = "";
            var context = Forms.Context as Activity;

            File testFile = new File(context.GetExternalFilesDir(null), CACHENAME);
            if (testFile != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                BufferedReader reader = null;

                try
                {
                    reader = new BufferedReader(new FileReader(testFile));
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        textFromFile += line.ToString();
                        textFromFile += "\n";
                    }

                    reader.Close();
                }
                catch (System.Exception e)
                {
                    Log.Error("ReadWriteFile", string.Format("Unable to read the {0} file.", CACHENAME));
                }
            }

            string foundValue = "";

            if (!string.IsNullOrEmpty(textFromFile))
            {
                string[] allValsAndKeys = textFromFile.Split(',');

                foreach (var dkey in allValsAndKeys)
                {
                    if (dkey.Contains(key))
                    {
                        string[] itemKeyAndVal = dkey.Split(':');
                        foundValue = itemKeyAndVal[1];
                        break;
                    }
                }
            }

            return foundValue;
        }
    }
}