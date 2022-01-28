using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1._5.Lab
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class NaiveCache<TItem>
    {
        Dictionary<object, (TItem Value, DateTime LastRequest)> _cache = new Dictionary<object, (TItem, DateTime)>();

        public TItem GetOrCreate(object key, Func<TItem> createItem)
        {
            if (!_cache.ContainsKey(key))
            {
                _cache[key] = (createItem(), DateTime.Now);
            }
            return _cache[key].Value;
        }
    }
}
