using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LibMonitor;
using LibHTTPServer;

namespace App
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("######## Web Monitor #########\n");

            if (args.Length != 1)
            {
                Console.WriteLine("Usage: WebMonitor.exe config.json");
                return 1;
            }
            
            // Load configuration file
            Config config = Config.Load(args[0]);
            if (config == null)
            {
                Console.WriteLine("Failed to load the configuration file");
                Console.Read();
                return 2;
            }

            // Log file to use
            string logFile = Path.Combine(Directory.GetCurrentDirectory(), "log.json");
            Console.WriteLine("Logs will be stored in " + logFile);
            Console.WriteLine("Web pages will be monitored after every " + config.Interval + " seconds");

            HTTPServer srv = new HTTPServer(logFile);
            srv.Start();

            try
            {
                Task.Run(async () =>
                {
                    // TODO: Implement mechanism to exit from forever loop i.e. Pressing ESC key
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
                            foreach (PageResult r in results)
                            {
                                Logger.LogResult(r, logFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }

                        await Task.Delay(TimeSpan.FromSeconds(config.Interval));
                    }
                }).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            srv.Stop();
            return 0;
        }
    }
}
