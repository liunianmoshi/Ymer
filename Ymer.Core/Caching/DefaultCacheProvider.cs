using Autofac;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymer.Environment;

namespace Ymer.Caching
{
    /// <summary>
    /// 缓存提供者
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TResult">数据的类型</typeparam>
    public class DefaultCacheProvider<TKey, TResult> : ICacheProvider<TKey, TResult>, IShim
    {

        private readonly ICacheContextAccessor cacheContextAccessor;
        private readonly ConcurrentDictionary<TKey, CacheEntry<TResult>> entries;

        public IHostContainer HostContainer
        {
            get;
            set;
        }

        public DefaultCacheProvider()
        {
            HostContainerRegistry.RegisterShim(this);
        }

        public DefaultCacheProvider(ICacheContextAccessor cacheContextAccessor)
        {
            this.cacheContextAccessor = cacheContextAccessor;
            entries = new ConcurrentDictionary<TKey, CacheEntry<TResult>>();
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
