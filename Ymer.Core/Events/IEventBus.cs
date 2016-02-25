using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Events
{
    /// <summary>
    /// 表示一个事件总线 eventBus.Handlers.XXX(fefq,tewtw)
    /// </summary>
    public interface IEventBus
    {

        dynamic Handlers
        {
            get;
        }
    }
}
