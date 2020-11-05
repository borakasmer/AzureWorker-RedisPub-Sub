
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RedisCacheService : IRedisCacheService
{
    #region Fields
    int RedisPort = 6379;
    string RedisEndPoint = "*****.redis.cache.windows.net";
    string RedisPassword = "*****";
    private readonly RedisEndpoint conf = null;

    #endregion

    public RedisCacheService()
    {
        conf = new RedisEndpoint { Host = RedisEndPoint, Port = RedisPort, Password = RedisPassword };
    }
    public T Get<T>(string key)
    {
        try
        {
            using (IRedisClient client = new RedisClient(conf))
            {
                return client.Get<T>(key);
            }
        }
        catch
        {
            throw new RedisNotAvailableException();
            //return default;
        }
    }

    public void Set(string key, object data, DateTime time)
    {
        try
        {
            using (IRedisClient client = new RedisClient(conf))
            {
                var dataSerialize = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
                client.Set(key, Encoding.UTF8.GetBytes(dataSerialize), time);
            }
        }
        catch
        {
            throw new RedisNotAvailableException();
        }
    }

    public void Publish(string channel, object data)
    {
        try
        {
            using (IRedisClient client = new RedisClient(conf))
            {
                var dataJson = JsonConvert.SerializeObject(data);
                client.PublishMessage(channel, dataJson);
            }
        }
        catch
        {
            throw new RedisNotAvailableException();
            //return default;
        }
    }
}
