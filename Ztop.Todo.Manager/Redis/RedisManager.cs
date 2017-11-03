using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ztop.Todo.Manager
{
    public class RedisManager
    {
        private static PooledRedisClientManager _prcm { get; set; }
        private static string[] _writeServerList { get; set; }
        private static string[] _readServerList { get; set; }
        private static int _maxWritePoolSize { get; set; }
        private static int _maxReadPoolSize { get; set; }
        private static int _localCacheTime { get; set; }
        private static bool _autoStart { get; set; }

        private static TimeSpan _expiresIn { get; set; }
        private static IRedisClient _client { get; set; }
        public static IRedisClient Client { get { return _client == null ? _client = _prcm.GetClient() : _client; } }
        static RedisManager()
        {
            _writeServerList = System.Configuration.ConfigurationManager.AppSettings["WriteServerList"].Split(',');
            _readServerList = System.Configuration.ConfigurationManager.AppSettings["ReadServerList"].Split(',');
            _maxWritePoolSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxWritePoolSize"].ToString());
            _maxReadPoolSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxReadPoolSize"].ToString());
            _localCacheTime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["LocalCacheTime"].ToString());
            _autoStart = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["AutoStart"].ToString());

            _expiresIn = TimeSpan.FromHours(24);

            _prcm = new PooledRedisClientManager(_readServerList, _writeServerList, new RedisClientManagerConfig
            {
                MaxReadPoolSize = _maxReadPoolSize,
                MaxWritePoolSize = _maxWritePoolSize,
                AutoStart = _autoStart
            });
        }


        public static bool Set<T>(string key,T value,IRedisClient client)
        {
            //return _client.Set<T>(key, value, _expiresIn);
            return client.Set<T>(key, value, _expiresIn);
        }

        public static T Get<T>(string key,IRedisClient client)
        {
            //return _client.Get<T>(key);
            return client.Get<T>(key);
        }

    }
}
