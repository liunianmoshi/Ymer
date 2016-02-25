using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 表示一个缓存提供者
    /// </summary>
    public interface ICacheProvider
    {

    }

    /// <summary>
    /// 表示一个泛型的缓存提供者
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    /// <typeparam name="TResult">数据类型</typeparam>
    public interface ICacheProvider<TKey, TResult> : ICacheProvider
    {
        TResult Get(TKey key, Func<ICacheContext<TKey>, TResult> context);
    }
}
