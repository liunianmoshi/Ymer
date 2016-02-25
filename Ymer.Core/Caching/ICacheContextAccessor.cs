using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    ///  默认的缓存上下文的提供者
    /// </summary>
    public interface ICacheContextAccessor
    {
        /// <summary>
        /// 当前的缓存上下文
        /// </summary>
        ICacheContext Current { get; set; }
    }
}
