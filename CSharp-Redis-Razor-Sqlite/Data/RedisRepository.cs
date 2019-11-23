using CSharp_Redis_Razor_Sqlite.Interfaces;
using CSharp_Redis_Razor_Sqlite.Models;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System.Collections.Generic;

namespace CSharp_Redis_Razor_Sqlite.Data
{
    public class RedisRepository : IDataRepository<Entry>
    {
        private readonly string _redisUrl;
        private readonly NewtonsoftSerializer _serializer;
        private readonly StackExchangeRedisCacheClient _cacheClient;
        private readonly SqlLiteRepository _sqlLiteRepository;
        public bool RefreshingRedis;

        public RedisRepository()
        {
            _redisUrl = "127.0.0.1";
            RefreshingRedis = false;

            var redisConfiguration = new RedisConfiguration()
            {
                AbortOnConnectFail = true,
                KeyPrefix = "",
                Hosts = new RedisHost[]
                {
                new RedisHost(){Host = this._redisUrl, Port = 6379}
                },
                AllowAdmin = true,
                ConnectTimeout = 3000,
                Database = 0,
                Ssl = false,
                Password = "",
                ServerEnumerationStrategy = new ServerEnumerationStrategy()
                {
                    Mode = ServerEnumerationStrategy.ModeOptions.All,
                    TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                    UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                }
            };

            _serializer = new NewtonsoftSerializer();
            _cacheClient = new StackExchangeRedisCacheClient(_serializer, redisConfiguration);
            _sqlLiteRepository = new SqlLiteRepository();
        }

        public IEnumerable<Entry> Add(Entry entity)
        {
            _cacheClient.Add((entity.EntryId).ToString(), entity);
            var result = _cacheClient.Get<Entry>((entity.EntryId).ToString());

            if (result != null)
            {
                _sqlLiteRepository.Add(entity); 
            }

            return (IEnumerable<Entry>)result;
        }

        public IEnumerable<Entry> Delete(Entry entity)
        {
            _cacheClient.Remove((entity.EntryId).ToString());
            var result = _cacheClient.Get<Entry>((entity.EntryId).ToString());

            if (result == null)
            {
                _sqlLiteRepository.Delete(entity);
                return (IEnumerable<Entry>)result;
            }
            else
            {
                return (IEnumerable<Entry>)new Entry();
            }
        }

        public IEnumerable<Entry> GetAllKeys()
        {
            var keys = _cacheClient.SearchKeys("*");
            var cachedEntries = _cacheClient.GetAll<Entry>(keys);
            
            return (IEnumerable<Entry>)cachedEntries.Values;
        }

        public IEnumerable<Entry> GetKey(string value)
        {
            var keys = _cacheClient.SearchKeys("*");
            var cachedEntries = _cacheClient.GetAll<Entry>(keys);

            foreach (KeyValuePair<string, Entry> cacheEntry in cachedEntries)
            {
                if (cacheEntry.Value.EntryValue == value)
                {
                    return (IEnumerable<Entry>)cacheEntry.Value;
                }
            }

            return (IEnumerable<Entry>)new Entry();
        }

        public IEnumerable<Entry> GetValue(string key)
        {
            var result = _cacheClient.Get<Entry>(key);
            return (IEnumerable<Entry>)result;
        }

        public IEnumerable<Entry> SaveAllKeys()
        {
            return _sqlLiteRepository.SaveAllKeys();
        }

        public IEnumerable<Entry> Update(Entry oldEntity, Entry newEntity)
        {
            _cacheClient.Remove((oldEntity.EntryId).ToString());
            _cacheClient.Add<Entry>((newEntity).ToString(), newEntity);
            var result = _cacheClient.Get<Entry>((newEntity.EntryId).ToString());
            if (result != null)
            {
                _sqlLiteRepository.Update(oldEntity, newEntity);
            }
            return (IEnumerable<Entry>)result;
        }
    }
}
