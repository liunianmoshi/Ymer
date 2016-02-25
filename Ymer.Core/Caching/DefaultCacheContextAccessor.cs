using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 默认的缓存上下文的提供者
    /// </summary>
    public class DefaultCacheContextAccessor : ICacheContextAccessor
    {
        [ThreadStatic]
        private static ICacheContext threadInstance;

        /// <summary>
        /// 当前实例的缓存上下文对象
        /// </summary>
        public static ICacheContext ThreadInstance
        {
            get
            {
                return
                  threadInstance;
            }
            set
            {
                threadInstance = value;
            }
        }

        /// <summary>
        /// 当前的缓存上下文
        /// </summary>
        public ICacheContext Current
        {
            get
            {
                return ThreadInstance;
            }
            set
            {
                ThreadInstance = value;
            }
        }
    }
}
