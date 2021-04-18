using System.Text;
using CoreFX.Abstractions.Bases;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoreFX.Abstractions.Serializers
{
    public class DefaultJsonSerializer : FxObject
    {
        public static Encoding DefaultEncoding = Encoding.UTF8;

        public static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string Serialize<T>(T obj, JsonSerializerSettings settings = null, bool ignoreException = false)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, settings ?? DefaultSettings);
            }
            catch
            {
                if (ignoreException)
                {
                    return null;
                }

                throw;
            }
        }

        public static byte[] SerializeToBytes<T>(T target, JsonSerializerSettings settings = null, bool ignoreException = false)
        {
            try
            {
                return DefaultEncoding.GetBytes(Serialize(target, settings, ignoreException));
            }
            catch
            {
                if (ignoreException)
                {
                    return null;
                }

                throw;
            }
        }

        public static T Deserialize<T>(string json, JsonSerializerSettings settings = null, bool ignoreException = false)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, settings ?? DefaultSettings);
            }
            catch
            {
                if (ignoreException)
                {
                    return default;
                }

                throw;
            }
        }

        public static T Deserialize<T>(byte[] data, JsonSerializerSettings settings = null, bool ignoreException = false)
        {
            return Deserialize<T>(DefaultEncoding.GetString(data, 0, data.Length), settings, ignoreException);
        }
    }
}
