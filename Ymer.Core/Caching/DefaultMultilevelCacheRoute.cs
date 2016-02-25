using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    public class DefaultMultilevelCacheRoute<TKey, TResult> : IMultilevelCacheRoute<TKey, TResult>
    {
        private IList<IMultilevelCacheProvider<TKey, TResult>> cacheProviders;

        private CacheGetDelegate<TKey, TResult> GetDelegate;

        public int LevelCount
        {
            get;
            private set;
        }

        public DefaultMultilevelCacheRoute(IEnumerable<IMultilevelCacheProvider<TKey, TResult>> cacheProviders)
        {
            this.cacheProviders = cacheProviders.OrderBy(p => p.Level).ToList();
            Init();
        }

        public TResult Get(TKey key, Func<ICacheContext<TKey>, TResult> context)
        {
            return GetDelegate(key, context);
        }

        private void Init()
        {
            LevelCount = cacheProviders.Count;
            if (LevelCount == 0)
            {
                throw new Exception("至少需要实现一个IMultilevelCacheRoute并注册");
            }

            CacheGetDelegate<TKey, TResult> lastProvider = cacheProviders[LevelCount - 1].Get;



            for (var i = LevelCount - 2; i >= 0; i--)
            {
                lastProvider = GetDelegateFactory(cacheProviders[i].Get, lastProvider);
            }

            GetDelegate = lastProvider;
        }

        private CacheGetDelegate<TKey, TResult> GetDelegateFactory(CacheGetDelegate<TKey, TResult> leftProvider, CacheGetDelegate<TKey, TResult> rightProvider)
        {
            return (key, context) => leftProvider(key, c =>
            {
                return rightProvider(key, context);
            });
        }
    }
}
