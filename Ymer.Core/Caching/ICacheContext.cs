using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 表示一个缓存的上下文
    /// </summary>
    public interface ICacheContext
    {
        Action<IVolatileToken> Monitor
        {
            get;
        }
    }

    /// <summary>
    /// 表示一个缓存的上下文
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    public interface ICacheContext<TKey> : ICacheContext
    {
        TKey Key
        {
            get;
        }
    }
}
