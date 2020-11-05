using System;
using System.Collections.Generic;

public interface IRedisCacheService
{
    T Get<T>(string key);
    void Set(string key, object data, DateTime time);

    void Publish(string channel,object data);

}