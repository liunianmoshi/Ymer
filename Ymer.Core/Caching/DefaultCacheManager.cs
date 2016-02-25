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
    /// 默认的缓存管理器
    /// </summary>
    public class DefaultCacheManager : ICacheManager, IShim
    {
        private readonly ConcurrentDictionary<CacheKey, IMultilevelCacheRoute> cacheRoutes = new ConcurrentDictionary<CacheKey, IMultilevelCacheRoute>();

        public IHostContainer HostContainer
        {
            get;
            set;
        }


        public DefaultCacheManager(ICacheContextAccessor cacheContextAccessor)
        {
            HostContainerRegistry.RegisterShim(this);
        }

        public TResult Get<TKey, TResult>(TKey key, Func<ICacheContext<TKey>, TResult> contex)
        {
            var cacheKey = new CacheKey(typeof(TKey), typeof(TResult));
            var result = cacheRoutes.GetOrAdd(cacheKey, k => HostContainer.Resolve<IMultilevelCacheRoute<TKey, TResult>>());

            return ((ICacheProvider<TKey, TResult>)result).Get(key, contex);
        }
    }
}
