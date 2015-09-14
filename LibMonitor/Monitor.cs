using System;
using System.Net;
using System.Net.Cache;
using System.Diagnostics;

namespace LibMonitor
{
    public class Monitor
    {
        public static PageResult TestUrl(Page p)
        {
            if (p == null)
                throw new ArgumentNullException("p is invalid");

            if (String.IsNullOrWhiteSpace(p.Pattern))
                throw new ArgumentException("pattern parameter is invalid");

            bool found = false;
            bool match = false;
            long responseTime = 0;

            using (WebClient wc = new WebClient())
            {
                // Always fetch content from the server
                wc.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                // Read web page content and measure the time interval it takes to load
                string content = string.Empty;
                try
                {
                    Stopwatch watch = Stopwatch.StartNew();
                    content = wc.DownloadString(p.Url);
                    responseTime = watch.ElapsedMilliseconds;
                    watch.Stop();
                    found = true;
                }
                catch (WebException webEx)
                {
                    Debug.WriteLine("Page load exception: " + webEx.Status);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                // Validate that the pattern string is in the page
                if (!String.IsNullOrEmpty(content) && content.Contains(p.Pattern))
                {
                    match = true;
                }
            }

            var res = new PageResult(p.Url, found, match, responseTime, (long)TimeSpan.FromTicks(DateTime.UtcNow.Ticks).TotalSeconds);
            Debug.WriteLine(res);
            return res;
        }
    }
}
