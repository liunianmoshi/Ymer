using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymer.Caching;

namespace Ymer.Redis
{
    public class RedisCacheProvider<TKey, TResult> : IMultilevelCacheProvider<TKey, TResult>
    {
        private readonly ICacheContextAccessor cacheContextAccessor;
        private readonly RedisCacheHandle<TKey, CacheEntry<TResult>> entries;

        public int Level
        {
            get
            {
                return 2;
            }
        }

        public RedisCacheProvider(ICacheContextAccessor cacheContextAccessor)
        {
            this.cacheContextAccessor = cacheContextAccessor;
            entries = new RedisCacheHandle<TKey, CacheEntry<TResult>>();
        }

        public TResult Get(TKey key, Func<ICacheContext<TKey>, TResult> context)
        {
            var entry = entries.AddOrUpdate(key,
                k => AddEntry(k, context),
                (k, currentEntry) => UpdateEntry(currentEntry, k, context));

            return entry.Result;
        }

        private CacheEntry<TResult> AddEntry(TKey k, Func<ICacheContext<TKey>, TResult> context)
        {
            var entry = CreateEntry(k, context);
            PropagateTokens(entry);
            return entry;
        }

        private CacheEntry<TResult> UpdateEntry(CacheEntry<TResult> currentEntry, TKey k, Func<ICacheContext<TKey>, TResult> context)
        {
            var entry = (currentEntry.Tokens.Any(t => t != null && !t.IsCurrent)) ? CreateEntry(k, context) : currentEntry;
            PropagateTokens(entry);
            return entry;
        }

        private void PropagateTokens(CacheEntry<TResult> entry)
        {
            // Bubble up volatile tokens to parent context
            if (cacheContextAccessor.Current != null)
            {
                foreach (var token in entry.Tokens)
                {
                    cacheContextAccessor.Current.Monitor(token);
                }
            }
        }


        private CacheEntry<TResult> CreateEntry(TKey k, Func<ICacheContext<TKey>, TResult> context)
        {
            var entry = new CacheEntry<TResult>();
            var currentContext = new DefaultCacheContext<TKey>(k, entry.AddToken);

            ICacheContext parentContext = null;
            try
            {
                parentContext = cacheContextAccessor.Current;
                cacheContextAccessor.Current = currentContext;

                entry.Result = context(currentContext);
            }
            finally
            {
                cacheContextAccessor.Current = parentContext;
            }
            entry.CompactTokens();
            return entry;
        }
    }
}
