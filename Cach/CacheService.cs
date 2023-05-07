using Newtonsoft.Json;
using StackExchange.Redis;

namespace WebApplicationClient.Cach
{
    public class CacheService : ICacheService
    {
        IDatabase _db;
        public CacheService()
        {
            _db = ConnectionHelper.Connection.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var val = _db.StringGet(key);
            if (!string.IsNullOrEmpty(val))
            {
                return JsonConvert.DeserializeObject<T>(val);
            }
            return default;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset dateTimeOffset) => _db.StringSet(key, JsonConvert.SerializeObject(value), dateTimeOffset.DateTime.Subtract(DateTime.Now));

    }
}
