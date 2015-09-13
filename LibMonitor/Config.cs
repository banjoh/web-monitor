using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace LibMonitor
{
    public class Config
    {
        public Config(int interval, ReadOnlyCollection<Page> pages)
        {
            Interval = interval;
            Pages = pages;
        }

        // Interval in seconds
        public int Interval { get; private set; }

        // List of pages
        public ReadOnlyCollection<Page> Pages { get; private set; }

        // Load configuration file
        public static Config Load(string configPath)
        {
            Debug.WriteLine("Loading config file from " + configPath);
            if (!File.Exists(configPath))
                throw new ArgumentException("Configuration path does not exist");

            try
            {
                // Load configuration file and store it in memory
                // Assumption is that the file should fit in memory
                string jsonData = File.ReadAllText(configPath);
                Config c = JsonConvert.DeserializeObject<Config>(jsonData);
                if (c.Interval <= 0)
                {
                    throw new Exception("Interval value is invalid. Please specify a valid value");
                }
                return c;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return null;
        }
    }
}
