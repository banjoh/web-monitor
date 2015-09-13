using System;

namespace LibMonitor
{
    public class Page
    {
        public Page(Uri url, string pattern)
        {
            Url = url;
            Pattern = pattern;
        }

        public Uri Url { get; private set; }
        public string Pattern { get; private set; }
    }
}
