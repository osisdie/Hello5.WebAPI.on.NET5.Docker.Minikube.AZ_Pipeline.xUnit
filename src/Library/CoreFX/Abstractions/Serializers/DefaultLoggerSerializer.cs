using System.Text;
using CoreFX.Abstractions.Bases;
using CoreFX.Abstractions.Serializers.Resolvers;
using Newtonsoft.Json;

namespace CoreFX.Abstractions.Serializers
{
    public class DefaultLoggerSerializer : FxObject
    {
        public static Encoding DefaultEncoding = Encoding.UTF8;

        public static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver()
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
    }
}
