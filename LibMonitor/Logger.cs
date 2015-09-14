using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LibMonitor
{
    public class Logger : IDisposable
    {
        private Dictionary<Uri, List<PageResultBase>> dataStore;
        private string LogFile { get; set; }

        public Logger(string logFile)
        {
            if (File.Exists(logFile))
            {
                string jsonData = File.ReadAllText(logFile);
                dataStore = JsonConvert.DeserializeObject<Dictionary<Uri, List<PageResultBase>>>(jsonData);
            }

            if (dataStore == null)
            {
                dataStore = new Dictionary<Uri, List<PageResultBase>>();
            }
            LogFile = logFile;
        }

        public void LogResult(PageResult r)
        {
            if (r != null)
            {
                var p = r as PageResultBase;

                // Store results in data store
                if (dataStore.ContainsKey(r.Url))
                {
                    dataStore[r.Url].Add(p);
                }
                else
                {
                    var list = new List<PageResultBase>();
                    list.Add(p);
                    dataStore.Add(r.Url, list);
                }

                // Write log to file
                Console.WriteLine(r);
            }
        }

        public void Dispose()
        {
            try
            {
                File.WriteAllText(LogFile, JsonConvert.SerializeObject(dataStore));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        ~Logger()
        {
            Dispose();
        }
    }
}
