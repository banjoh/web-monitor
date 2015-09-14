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

            Task.Run(async () =>
            {
                // Check web pages forever
                while (true)
                {
                    try
                    {
                        // Test all pages asynchronously
                        List<Task<PageResult>> tasks = new List<Task<PageResult>>();

                        // TODO: Should we pick pages in groups?
                        foreach (Page p in config.Pages)
                        {
                            tasks.Add(Task.Run(() =>
                            {
                                return LibMonitor.Monitor.TestUrl(p);
                            }));
                        }

                        // Wait for all tasks to complete
                        PageResult[] results = await Task.WhenAll(tasks.ToArray());

                        // Read the results of the page test runs
                        // TODO: Next store the result in a log file
                        using (Logger l = new Logger("path to log file"))
                        {
                            foreach (PageResult r in results)
                            {
                                l.LogResult(r);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(config.Interval));
                }
            }).Wait();

            return 0;
        }
    }
}
