using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Environment
{
    /// <summary>
    /// 表示一个隔离器
    /// </summary>
    public interface IShim
    {
        /// <summary>
        /// 表示ioc容器
        /// </summary>
        IHostContainer HostContainer { get; set; }
    }
}
