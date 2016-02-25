using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    public delegate TResult CacheGetDelegate<TKey, TResult>(TKey key, Func<ICacheContext<TKey>, TResult> context);
}
