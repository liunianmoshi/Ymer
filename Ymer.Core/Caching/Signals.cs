using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Caching
{
    /// <summary>
    /// 表示一个根据键来生产过期规则的的提供者
    /// </summary>
    public interface ISignals : IVolatileProvider
    {
        /// <summary>
        /// 使数据过期
        /// </summary>
        /// <typeparam name="T">键的类型</typeparam>
        /// <param name="signal">键</param>
        void Trigger<T>(T signal);
        /// <summary>
        /// 生产一个根据键来过期的令牌
        /// </summary>
        /// <typeparam name="T">键的类型</typeparam>
        /// <param name="signal">键</param>
        /// <returns></returns>
        IVolatileToken When<T>(T signal);
    }

    /// <summary>
    /// 表示一个根据键来生产过期规则的的提供者
    /// </summary>
    public class Signals : ISignals
    {
        private readonly IDictionary<object, Token> tokens = new Dictionary<object, Token>();

        /// <summary>
        /// 使数据过期
        /// </summary>
        /// <typeparam name="T">键的类型</typeparam>
        /// <param name="signal">键</param>
        public void Trigger<T>(T signal)
        {
            lock (tokens)
            {
                Token token;
                if (tokens.TryGetValue(signal, out token))
                {
                    tokens.Remove(signal);
                    token.Trigger();
                }
            }

        }

        /// <summary>
        /// 生产一个根据键来过期的令牌
        /// </summary>
        /// <typeparam name="T">键的类型</typeparam>
        /// <param name="signal">键</param>
        /// <returns></returns>
        public IVolatileToken When<T>(T signal)
        {
            lock (tokens)
            {
                Token token;
                if (!tokens.TryGetValue(signal, out token))
                {
                    token = new Token();
                    tokens[signal] = token;
                }
                return token;
            }
        }

        private class Token : IVolatileToken
        {
            public Token()
            {
                IsCurrent = true;
            }
            public bool IsCurrent { get; private set; }
            public void Trigger() { IsCurrent = false; }


            public void Reset()
            {

            }
        }
    }
}
