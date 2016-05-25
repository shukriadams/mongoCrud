using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace MongoCrud
{
    public class Memcached : ICache
    {
        private MemcachedClient _client;

        public Memcached(string url, int port, string zone, string username, string password)
        {
            MemcachedClientConfiguration mbcc = new MemcachedClientConfiguration();
            mbcc.Authentication.Type = typeof(PlainTextAuthenticator);
            mbcc.Authentication.Parameters["zone"] = zone;
            mbcc.Authentication.Parameters["userName"] = username;
            mbcc.Authentication.Parameters["password"] = password;
           
            mbcc.AddServer(url, port);
            _client = new MemcachedClient(mbcc);
        }

        public void Add(string key, object entity)
        {
            _client.Cas(StoreMode.Set, key, entity);
        }

        public object Get(string key)
        {
            return _client.GetWithCas(key).Result;
        }

        public void Remove(string key)
        {
            _client.Remove(key);
        }
    }
}
