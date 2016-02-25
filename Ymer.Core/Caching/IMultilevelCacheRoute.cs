using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    public interface IMultilevelCacheRoute
    {
        /// <summary>
        /// 缓存的层数
        /// </summary>
        int LevelCount
        {
            get;
        }
    }

    /// <summary>
    /// 多级缓存路由器
    /// </summary>
    public interface IMultilevelCacheRoute<TKey, TResult> : IMultilevelCacheRoute, ICacheProvider<TKey, TResult>
    {


    }
}
