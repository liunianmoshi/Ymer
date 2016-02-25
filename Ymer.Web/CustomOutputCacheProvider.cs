using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Ymer.Caching;
using Ymer.Environment;

namespace Ymer.Web
{
    /// <summary>
    /// 重写OutputCache
    /// </summary>
    public class CustomOutputCacheProvider : OutputCacheProvider, IShim
    {
        private CacheHolder cacheHolder;


        public ICacheManager Cache
        {
            get
            {
                if (cacheHolder == null)
                {
                    cacheHolder = HostContainer.Resolve<CacheHolder>();
                }
                return cacheHolder.Cache;
            }
        }

        public IHostContainer HostContainer
        {
            get;
            set;
        }

        private ISignals signals;
        private IClock clock;

        public CustomOutputCacheProvider()
        {
            HostContainerRegistry.RegisterShim(this);
            signals = HostContainer.Resolve<ISignals>();
            clock = HostContainer.Resolve<IClock>();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
        }
        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            if (utcExpiry != default(DateTime) && utcExpiry != DateTime.MaxValue)
            {
                return Cache.Get(key, context =>
                {
                    context.Monitor(clock.WhenUtc(utcExpiry));
                    return entry;
                });
            }
            else
            {
                return null;
            }
        }

        private readonly object nullObject = new object();

        public override object Get(string key)
        {

            var obj = Cache.Get(key, context =>
             {
                 context.Monitor(clock.When(new TimeSpan(1)));
                 return nullObject;
             });
            if (obj == nullObject)
            {
                return null;
            }
            else
            {
                return obj;
            }
        }

        public override void Remove(string key)
        {
            signals.Trigger(key);
        }

        /// <summary>
        /// 设置缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        /// <param name="utcExpiry"></param>
        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            if (utcExpiry != default(DateTime) && utcExpiry != DateTime.MaxValue)
            {
                var timeout = TimeSpan.FromTicks(utcExpiry.Ticks - DateTime.UtcNow.Ticks);
                Cache.Get(key, context =>
                {
                    context.Monitor(clock.WhenUtc(utcExpiry));
                    return entry;
                });
            }
        }
    }
}
