using System.Text;

namespace CoreFX.Abstractions.Serializers.Interfaces
{
    public interface ISerializer
    {
        T Deserialize<T>(string json, bool ignoreException);
        T Deserialize<T>(byte[] json);
        T Deserialize<T>(string json);
        string Serialize<T>(T obj);
        byte[] SerializeToBytes<T>(T obj);
        Encoding EncodingCharset { get; set; }
    }
}
