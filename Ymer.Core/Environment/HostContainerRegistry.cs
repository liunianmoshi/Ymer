using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Environment
{
    /// <summary>
    /// ioc容器注册
    /// </summary>
    public class HostContainerRegistry
    {
        private static readonly IList<WeakReference<IShim>> _shims = new List<WeakReference<IShim>>();
        private static IHostContainer _hostContainer;
        private static readonly object _syncLock = new object();

        public static void RegisterShim(IShim shim)
        {

            lock (_syncLock)
            {
                CleanupShims();

                _shims.Add(new WeakReference<IShim>(shim));
                shim.HostContainer = _hostContainer;
            }
        }

        public static void RegisterHostContainer(IHostContainer container)
        {
            lock (_syncLock)
            {
                CleanupShims();

                _hostContainer = container;
                RegisterContainerInShims();
            }
        }

        private static void RegisterContainerInShims()
        {
            foreach (var shim in _shims)
            {
                IShim target;

                if (shim.TryGetTarget(out target))
                {
                    target.HostContainer = _hostContainer;
                }
            }
        }

        private static void CleanupShims()
        {
            for (int i = _shims.Count - 1; i >= 0; i--)
            {
                IShim target;
                if ((!_shims[i].TryGetTarget(out target)) || target == null)
                {
                    _shims.RemoveAt(i);
                }
            }
        }
    }
}
