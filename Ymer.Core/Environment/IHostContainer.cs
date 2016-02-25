using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Environment
{
    /// <summary>
    /// 表示一个ioc容器
    /// </summary>
    public interface IHostContainer
    {
        TService Resolve<TService>();

        TService Resolve<TService>(params Parameter[] parameters);

        TService Resolve<TService>(params object[] parameters);
    }
}
