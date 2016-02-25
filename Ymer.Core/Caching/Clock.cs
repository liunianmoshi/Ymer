using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 表示一个根据时间的过期规则的提供者
    /// </summary>
    public interface IClock : IVolatileProvider
    {
        /// <summary>
        /// 返回当前UTC时间
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// 根据时间间隔生成令牌
        /// </summary>
        /// <param name="duration">过期的时间间隔</param>
        /// <returns></returns>
        IVolatileToken When(TimeSpan duration);

        /// <summary>
        /// 根据过期时间来生成令牌
        /// </summary>
        /// <param name="absoluteUtc">过期时间</param>
        /// <returns></returns>
        IVolatileToken WhenUtc(DateTime absoluteUtc);
    }

    /// <summary>
    /// 根据时间的过期规则的提供者
    /// </summary>
    public class Clock : IClock
    {
        /// <summary>
        /// 返回当前UTC时间
        /// </summary>
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        /// <summary>
        /// 根据时间间隔生成令牌
        /// </summary>
        /// <param name="duration">过期的时间间隔</param>
        /// <returns></returns>
        public IVolatileToken When(TimeSpan duration)
        {
            return new AbsoluteExpirationToken(this, duration);
        }

        /// <summary>
        /// 根据过期时间来生成令牌
        /// </summary>
        /// <param name="absoluteUtc">过期时间</param>
        /// <returns></returns>
        public IVolatileToken WhenUtc(DateTime absoluteUtc)
        {
            return new AbsoluteExpirationToken(this, absoluteUtc);
        }

        /// <summary>
        /// 根据时间过期的令牌
        /// </summary>
        public class AbsoluteExpirationToken : IVolatileToken
        {
            private readonly IClock _clock;
            private readonly DateTime _invalidateUtc;

            public AbsoluteExpirationToken(IClock clock, DateTime invalidateUtc)
            {
                _clock = clock;
                _invalidateUtc = invalidateUtc;
            }

            public AbsoluteExpirationToken(IClock clock, TimeSpan duration)
            {
                _clock = clock;
                _invalidateUtc = _clock.UtcNow.Add(duration);
            }

            public bool IsCurrent
            {
                get
                {
                    return _clock.UtcNow < _invalidateUtc;
                }
            }
        }
    }
}
