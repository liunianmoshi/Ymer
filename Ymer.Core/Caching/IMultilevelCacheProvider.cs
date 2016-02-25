using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 多级缓存提供者
    /// </summary>
    /// <typeparam name="TKey">Key类型</typeparam>
    /// <typeparam name="TResult">数据类型</typeparam>
    public interface IMultilevelCacheProvider<TKey, TResult>: ICacheProvider<TKey, TResult>
    {
        /// <summary>
        /// 缓存层级
        /// </summary>
        int Level
        {
            get;
        }
    }
}
