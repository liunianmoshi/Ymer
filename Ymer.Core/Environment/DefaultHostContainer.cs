using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Environment
{
    /// <summary>
    /// 默认的ioc容器
    /// </summary>
    public class DefaultHostContainer : IHostContainer
    {
        private readonly IContainer container;


        public DefaultHostContainer(IContainer container)
        {
            this.container = container;
        }

        public TService Resolve<TService>()
        {
            return container.Resolve<TService>();
        }

        public TService Resolve<TService>(params object[] parameters)
        {
            return Resolve<TService>(parameters.Select(o => (Parameter)(new TypedParameter(o.GetType(), o))).ToArray());
        }

        public TService Resolve<TService>(params Parameter[] parameters)
        {
            return container.Resolve<TService>(parameters);
        }
    }
}
