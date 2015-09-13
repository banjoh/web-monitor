using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using LibMonitor;

namespace App
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: WebMonitor.exe config.json");
            }
            
            // Load configuration file
            Config config = Config.Load(args[0]);
            if (config == null)
            {
                Console.WriteLine("Failed to load the configuration file");
                Console.Read();
                return 1;
            }

            // Check web pages forever
            while (true)
            {
                try
                {
                    // Test all pages asynchronously
                    List<Task<PageResult>> tasks = new List<Task<PageResult>>();

                    foreach (Page p in config.Pages)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            return LibMonitor.Monitor.TestUrl(null);
                        }));
                    }

                    // Wait for all tasks to complete
                    Task.WaitAll(tasks.ToArray());

                    // Read the results of the page test runs
                    // TODO: Next store the result in a log file
                    foreach (Task<PageResult> t in tasks)
                    {
                        PageResult r = t.Result;
                        Console.WriteLine("Url: {0}, Found: {1}, Matched: {2}, ResponseTime (mills): {3}", r.Url, r.Found, r.Matched, r.ResponseTime);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                Thread.Sleep(config.Interval * 1000);
            }
        }
    }
}
