using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ymer.Caching;

namespace Ymer
{
    /// <summary>
    /// 根据标签生产过期规则的提供者
    /// </summary>
    public interface ITagSignals : IVolatileProvider
    {
        /// <summary>
        /// 设置一个或多个标签过期
        /// </summary>
        /// <typeparam name="T">标签类型</typeparam>
        /// <param name="tag">标签集</param>
        void Trigger<T>(IEnumerable<T> tags);
        /// <summary>
        /// 设置一个或多个标签过期
        /// </summary>
        /// <typeparam name="T">标签类型</typeparam>
        /// <param name="tag">标签集</param>
        void Trigger<T>(params T[] tag);
        /// <summary>
        /// 根据标签生成令牌
        /// </summary>
        /// <typeparam name="T">标签类型</typeparam>
        /// <param name="tags">标签集</param>
        /// <returns></returns>
        IVolatileToken When<T>(IEnumerable<T> tags);
        /// <summary>
        /// 根据标签生成令牌
        /// </summary>
        /// <typeparam name="T">标签类型</typeparam>
        /// <param name="tag">标签集</param>
        /// <returns></returns>
        IVolatileToken When<T>(params T[] tag);
    }

    /// <summary>
    /// 根据标签生产过期规则的提供者
    /// </summary>
    public class TagSignals : ITagSignals
    {
        private readonly IDictionary<object, IList<Token>> tokenDictionary = new Dictionary<object, IList<Token>>();

        /// <summary>
        /// 设置一个或多个标签过期
        /// </summary>
        /// <typeparam name="T">标签类型</typeparam>
        /// <param name="tag">标签集</param>
        public void Trigger<T>(params T[] tag)
        {
            Trigger(tag.AsEnumerable());
        }

        /// <summary>
        /// 设置一个或多个标签过期
        /// </summary>
        /// <typeparam name="T">标签类型</typeparam>
        /// <param name="tags">标签集</param>
        public void Trigger<T>(IEnumerable<T> tags)
        {
            lock (tokenDictionary)
            {
                foreach (var tag in tags)
                {
                    IList<Token> tokens;
                    if (tokenDictionary.TryGetValue(tag, out tokens))
                    {
                        tokenDictionary.Remove(tag);
                        foreach (var token in tokens)
                        {
                            token.Trigger();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据标签生成令牌
        /// </summary>
        /// <typeparam name="T">标签类型</typeparam>
        /// <param name="tags">标签集</param>
        /// <returns></returns>
        public IVolatileToken When<T>(params T[] tag)
        {
            return When(tag.AsEnumerable());
        }
        /// <summary>
        /// 根据标签生成令牌
        /// </summary>
        /// <typeparam name="T">标签类型</typeparam>
        /// <param name="tags">标签集</param>
        /// <returns></returns>
        public IVolatileToken When<T>(IEnumerable<T> tags)
        {
            lock (tokenDictionary)
            {
                var token = new Token();
                foreach (var tag in tags)
                {
                    IList<Token> tokens;
                    if (!tokenDictionary.TryGetValue(tag, out tokens))
                    {
                        tokens = new List<Token>();
                        tokens.Add(token);
                        tokenDictionary[tag] = tokens;
                    }
                }

                return token;
            }
        }
        /// <summary>
        /// 令牌
        /// </summary>
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
