using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LibMonitor
{
    public class Logger
    {
        // TODO: What if we have different log files we need to synchronize access?
        private static readonly object lockObj = new Object();
        
        public static void LogResult(PageResult r, string logFile)
        {
            if (r != null)
            {
                var dataStore = Deserialize(logFile);
                var value = new Tuple<string, bool, bool, long, long>(r.Pattern, r.Found, r.Matched, r.Response, r.TimeStamp);

                // Store results in data store
                if (dataStore.ContainsKey(r.Url))
                {
                    dataStore[r.Url].Add(value);
                }
                else
                {
                    var list = new List<Tuple<string, bool, bool, long, long>>();
                    list.Add(value);
                    dataStore.Add(r.Url, list);
                }

                Logger.Serialize(dataStore, logFile);

            }
        }

        public static Dictionary<Uri, List<Tuple<string, bool, bool, long, long>>> Deserialize(string logFile)
        {
            if (File.Exists(logFile))
            {
                string jsonData;
                lock (lockObj)
                {
                    jsonData = File.ReadAllText(logFile);
                }
                return JsonConvert.DeserializeObject<Dictionary<Uri, List<Tuple<string, bool, bool, long, long>>>>(jsonData);
            }

            return new Dictionary<Uri, List<Tuple<string, bool, bool, long, long>>>();
        }

        public static string Serialize(Dictionary<Uri, List<Tuple<string, bool, bool, long, long>>> dataStore, string logFile)
        {
            string s = JsonConvert.SerializeObject(dataStore);
            lock (lockObj)
            {
                File.WriteAllText(logFile, s);
            }
            return s;
        }
    }
}
