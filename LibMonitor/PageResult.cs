using System;

namespace LibMonitor
{
    public class PageResult : PageResultBase
    {
        public PageResult(Uri url, bool found, bool matched, long respTime)
            : base (found, matched, respTime)
        {
            Url = url;
        }

        public Uri Url { get; private set; }

        public override string ToString()
        {
            return String.Format("Url: {0}, Found: {1}, Matched: {2}, ResponseTime (mills): {3}, TimeStamp: {4}", Url, Found, Matched, ResponseTime, TimeStamp);
        }
    }

    public class PageResultBase
    {
        public PageResultBase(bool found, bool matched, long respTime)
        {
            Found = found;
            Matched = matched;
            ResponseTime = respTime;
            TimeStamp = TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds;
        }

        public bool Found { get; private set; }
        public bool Matched { get; private set; }
        public long ResponseTime { get; private set; }
        public double TimeStamp { get; private set; }
    }
}
