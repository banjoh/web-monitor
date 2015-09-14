using System;

namespace LibMonitor
{
    public class PageResult
    {
        public PageResult(Uri url, bool found, bool matched, long respTime)
        {
            Url = url;
            Found = found;
            Matched = matched;
            ResponseTime = respTime;
        }

        public Uri Url { get; private set; }
        public bool Found { get; private set; }
        public bool Matched { get; private set; }
        public long ResponseTime { get; private set; }

        public override string ToString()
        {
            return String.Format("Url: {0}, Found: {1}, Matched: {2}, ResponseTime (mills): {3}", Url, Found, Matched, ResponseTime);
        }
    }
}
