using fastJSON;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymer.Redis
{
    public class RedisCacheHandle<TKey, TResult>
    {
        private readonly string cacheName = typeof(TKey).FullName + "_" + typeof(TResult).FullName;

        public TResult AddOrUpdate(TKey key, Func<TKey, TResult> addValueFactory, Func<TKey, TResult, TResult> updateValueFactor)
        {
            using (var muxer = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisUri"]))
            {
                muxer.PreserveAsyncOrder = false;
                RedisKey redisKey = cacheName + key.ToString();
                var conn = muxer.GetDatabase();



                var val = conn.StringGet(redisKey);

                TResult result;

                if (val.IsNull)
                {
                    result = addValueFactory(key);
                    conn.StringSet(redisKey, ToJson(result));
                }
                else
                {
                    result = updateValueFactor(key, ToResult(val));
                    conn.StringSet(redisKey, ToJson(result));
                }

                return result;

            }
        }

        private TResult ToResult(RedisValue val)
        {
            return JSON.ToObject<TResult>(val);
        }

        private string ToJson(TResult result)
        {
            return JSON.ToJSON(result);
        }
    }
}
