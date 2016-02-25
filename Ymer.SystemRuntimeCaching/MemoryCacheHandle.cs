using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.SystemRuntimeCaching
{
    public class MemoryCacheHandle<TKey, TResult>
    {
        private volatile MemoryCache cache = null;

        private readonly string cacheName = Guid.NewGuid().ToString();

        public MemoryCacheHandle()
        {
            cache = new MemoryCache(this.cacheName);
        }

        public TResult AddOrUpdate(TKey key, Func<TKey, TResult> addValueFactory, Func<TKey, TResult, TResult> updateValueFactor)
        {
            var keyStr = key.ToString();
            TResult result;
            if (cache.Contains(keyStr))
            {
                result = updateValueFactor(key, (TResult)cache[keyStr]);
                cache.Set(keyStr, result, DateTimeOffset.MaxValue);
            }
            else
            {
                result = addValueFactory(key);
                cache.Add(keyStr, result, DateTimeOffset.MaxValue);
            }

            return result;
        }


    }
}
