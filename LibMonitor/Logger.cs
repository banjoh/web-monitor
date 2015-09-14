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
                var dataStore = new Dictionary<Uri, List<Tuple<bool, bool, long, long>>>();
                var p = new Tuple<bool, bool, long, long>(r.Found, r.Matched, r.ResponseTime, r.TimeStamp);

                if (File.Exists(logFile))
                {
                    // TODO: Reading the whole file for every log operation is not very optimal
                    string jsonData;
                    lock (lockObj)
                    {
                        jsonData = File.ReadAllText(logFile);
                    }
                    dataStore = JsonConvert.DeserializeObject<Dictionary<Uri, List<Tuple<bool, bool, long, long>>>>(jsonData);
                }

                // Store results in data store
                if (dataStore.ContainsKey(r.Url))
                {
                    dataStore[r.Url].Add(p);
                }
                else
                {
                    var list = new List<Tuple<bool, bool, long, long>>();
                    list.Add(p);
                    dataStore.Add(r.Url, list);
                }

                lock (lockObj)
                {
                    string s = JsonConvert.SerializeObject(dataStore);
                    File.WriteAllText(logFile, s);
                }

                // Write log to file
                Console.WriteLine(r);
            }
        }

        public static Dictionary<Uri, List<Tuple<bool, bool, long, long>>> ReadLogs(string logFile)
        {
            if (File.Exists(logFile))
            {
                string jsonData;
                lock (lockObj)
                {
                    jsonData = File.ReadAllText(logFile);
                }
                return JsonConvert.DeserializeObject<Dictionary<Uri, List<Tuple<bool, bool, long, long>>>>(jsonData);
            }

            return null;
        }
    }
}
