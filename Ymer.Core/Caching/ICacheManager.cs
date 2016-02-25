using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 表示一个缓存管理器
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="TKey">键的类型</typeparam>
        /// <typeparam name="TResult">数据的类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="context">一个提供缓存上下文作为参数的缓存值得生成器</param>
        /// <returns></returns>
        TResult Get<TKey, TResult>(TKey key, Func<ICacheContext<TKey>, TResult> context);
    }
}
