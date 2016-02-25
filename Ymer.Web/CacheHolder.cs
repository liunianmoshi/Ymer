using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymer.Caching;

namespace Ymer.Web
{
    public class CacheHolder
    {
        private ICacheManager cache;

        public CacheHolder(ICacheManager cache)
        {
            this.cache = cache;
        }

        public ICacheManager Cache
        {
            get
            {
                return cache;
            }
        }
    }
}
