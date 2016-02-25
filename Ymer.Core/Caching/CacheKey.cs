using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    public class CacheKey : Tuple<Type, Type>
    {
        public CacheKey(Type key, Type result)
            : base(key, result)
        {
        }
    }
}
