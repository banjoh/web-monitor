using System;

namespace LibMonitor
{
    public class PageResult
    {
        public PageResult(Uri url, string pattern, bool found, bool matched, long respTime, long timeStamp)
        {
            Url = url;
            Pattern = pattern;
            Found = found;
            Matched = matched;
            Response = respTime;
            TimeStamp = timeStamp;
        }

        public Uri Url { get; private set; }
        public string Pattern { get; private set; }
        public bool Found { get; private set; }
        public bool Matched { get; private set; }
        public long Response { get; private set; }
        public long TimeStamp { get; private set; }

        public override string ToString()
        {
            return String.Format("Url: {0}, Pattern: {5}, Found: {1}, Matched: {2}, ResponseTime (mills): {3}, TimeStamp: {4}", Url, Found, Matched, Response, TimeStamp, Pattern);
        }
    }
}
