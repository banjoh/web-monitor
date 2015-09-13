using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibMonitor;

namespace UnitTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestUrl1()
        {
            var result = Monitor.TestUrl(new Page (new Uri("https://msdn.microsoft.com/en-us"), "Microsoft"));

            Assert.IsTrue(result.Found);
            Assert.IsTrue(result.Matched);
            Assert.IsTrue(result.ResponseTime > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUrl2()
        {
            var result = Monitor.TestUrl(new Page(new Uri("https://msdn.microsoft.com/en-us"), ""));
        }

        [TestMethod]
        public void TestUrl3()
        {
            var result = Monitor.TestUrl(new Page(new Uri("https://msdn.microsoft.com/en-us"), "asdföjasdlfknasuilgebdfasd"));

            Assert.IsTrue(result.Found);
            Assert.IsFalse(result.Matched);
            Assert.IsTrue(result.ResponseTime > 0);
        }

        [TestMethod]
        public void TestUrl4()
        {
            var result = Monitor.TestUrl(new Page(new Uri("https://blablablablablabalbal"), "Microsoft"));

            Assert.IsFalse(result.Found);
            Assert.IsFalse(result.Matched);
            Assert.IsTrue(result.ResponseTime == 0);
        }

        [TestMethod]
        public void TestLoadingConfig1()
        {
            Config c = Config.Load(Path.Combine(Directory.GetCurrentDirectory(), "TestData", "config.json"));

            Assert.IsNotNull(c);
            Assert.IsTrue(c.Interval == 5);
            Assert.IsTrue(c.Pages.Count == 5);
            Assert.IsTrue(c.Pages[0].Url == new Uri("https://msdn.microsoft.com/en-us"));
            Assert.IsTrue(c.Pages[0].Pattern == "Microsoft");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestLoadingConfig2()
        {
            Config c = Config.Load("Some path");
        }
    }
}
