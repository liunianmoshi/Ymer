using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 默认的缓存上下文
    /// </summary>
    /// <typeparam name="TKey">key类型</typeparam>
    public class DefaultCacheContext<TKey> : ICacheContext<TKey>
    {
        /// <summary>
        /// 创建一个默认的缓存上下文
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="monitor">令牌监视器</param>
        public DefaultCacheContext(TKey key, Action<IVolatileToken> monitor)
        {
            Key = key;
            Monitor = monitor;
        }

        /// <summary>
        /// 键
        /// </summary>
        public TKey Key
        {
            get; private set;
        }

        /// <summary>
        /// 令牌监视器
        /// </summary>
        public Action<IVolatileToken> Monitor
        {
            get; private set;
        }
    }
}
