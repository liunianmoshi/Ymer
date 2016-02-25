using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymer.Caching;

namespace Ymer.Environment
{
    /// <summary>
    /// 应用程序启动之前需要事先启动的项
    /// </summary>
    public static class YmerStarter
    {
        /// <summary>
        /// 创建缓存容器
        /// </summary>
        /// <param name="registrations">提供IOC注入功能的委托</param>
        /// <returns></returns>
        public static IContainer CreateHostContainer(Action<ContainerBuilder> registrations = null)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DefaultCacheManager>()
                .As<ICacheManager>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(DefaultMultilevelCacheRoute<,>)).As(typeof(IMultilevelCacheRoute<,>)).InstancePerDependency();

            builder.RegisterType<DefaultCacheContextAccessor>().As<ICacheContextAccessor>().SingleInstance();

            //builder.RegisterType<DefaultHostContainer>().As<IHostContainer>();

            builder.RegisterVolatileProvider<Signals, ISignals>();
            builder.RegisterVolatileProvider<TagSignals, ITagSignals>();
            builder.RegisterVolatileProvider<Clock, IClock>();

            if (registrations != null)
            {
                registrations(builder);
            }

            var container = builder.Build();

            var hostContainer = new DefaultHostContainer(container);

            HostContainerRegistry.RegisterHostContainer(hostContainer);

            return container;
        }

        private static void RegisterVolatileProvider<TRegister, TService>(this ContainerBuilder builder) where TService : IVolatileProvider
        {
            builder.RegisterType<TRegister>()
                .As<TService>()
                .As<IVolatileProvider>()
                .SingleInstance();
        }
    }
}
