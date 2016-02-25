using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 缓存
    /// </summary>
    /// <typeparam name="TResult">数据类型</typeparam>
    public class CacheEntry<TResult>
    {
        public IList<IVolatileToken> tokens;
        /// <summary>
        /// 实际数据类型
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// 令牌集合
        /// </summary>
        public IEnumerable<IVolatileToken> Tokens
        {
            get
            {
                return tokens ?? Enumerable.Empty<IVolatileToken>();
            }
        }

        /// <summary>
        /// 添加令牌
        /// </summary>
        /// <param name="volatileToken">令牌</param>
        public void AddToken(IVolatileToken volatileToken)
        {
            if (tokens == null)
            {
                tokens = new List<IVolatileToken>();
            }

            tokens.Add(volatileToken);
        }

        /// <summary>
        /// 去除重复的令牌
        /// </summary>
        public void CompactTokens()
        {
            if (tokens != null)
                tokens = tokens.Distinct().ToArray();
        }
    }
}
