using StackExchange.Redis;

namespace MvcCoreCacheRedis.Helpers
{
    public class HelperCacheMultiplexer
    {
        private static Lazy<ConnectionMultiplexer>
            CreateConnection = new Lazy<ConnectionMultiplexer>
            (() =>
            {
                string cacheRedisKeys = "cacheredisanita.redis.cache.windows.net:6380,password=pJHOCAW4Ukgut03dQfNd3Cgoqs8sMtnETAzCaNv0MPg=,ssl=True,abortConnect=False";
                return ConnectionMultiplexer.Connect(cacheRedisKeys);
            });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
            
    }
}
