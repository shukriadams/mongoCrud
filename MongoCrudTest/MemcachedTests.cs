using MongoCrud;
using NUnit.Framework;
using System;
using System.Configuration;

namespace MongoCrudTest
{
    [TestFixture]
    public class MemcachedTests
    {
        private Memcached _memcached;

        [SetUp]
        public void SetUpAttribute()
        {
            _memcached = new Memcached(
                ConfigurationManager.AppSettings["memcachedAddress"],
                Int32.Parse(ConfigurationManager.AppSettings["memcachedPort"]),
                ConfigurationManager.AppSettings["memcachedZone"],
                ConfigurationManager.AppSettings["memcachedUsername"],
                ConfigurationManager.AppSettings["memcachedPassword"]);
        }

        [Test]
        public void AddThenGet()
        {
            _memcached.Add("foo", "bar");
            string bar = _memcached.Get("foo") as string;
            Assert.AreEqual(bar, "bar");
        }

        [Test]
        public void AddThenRemove()
        {
            _memcached.Add("foo", "bar");
            _memcached.Remove("foo");
            string bar = _memcached.Get("foo") as string;
            Assert.IsNull(bar);
        }
    }
}
