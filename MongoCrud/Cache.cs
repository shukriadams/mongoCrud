using System.Collections.Generic;

namespace MongoCrud
{
    public class Cache 
    {
        private static Cache _instance;

        public static Cache Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Cache();
                return _instance;
            }
        }

        readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public object Get(string key)
        {
            if (_dictionary.ContainsKey(key))
                return _dictionary[key];
            return null;
        }

        public void Add(string key, object o)
        {
            this.Remove(key);
            _dictionary.Add(key, o);
        }

        public void Remove(string key)
        {
            if (_dictionary.ContainsKey(key))
                _dictionary.Remove(key);
        }

        public void RemoveAll()
        {
            _dictionary.Clear();
        }

        public int CachedItems
        {
            get
            {
                return _dictionary.Count;
            }
        }
    }
}
