using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 变更令牌
    /// </summary>
    public interface IVolatileToken
    {
        /// <summary>
        /// 是否过期
        /// </summary>
        bool IsCurrent
        {
            get;
        }
    }
}
