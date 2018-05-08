using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BinaryCook.Infrastructure.Caching.Configurations;
using ProtoBuf;
using ProtoBuf.Meta;
using StackExchange.Redis.Extensions.Core;

namespace BinaryCook.Infrastructure.Caching
{
    public class ProtobufSerializer : ISerializer
    {
        public byte[] Serialize(object item)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, item);
                return stream.ToArray();
            }
        }

        public Task<byte[]> SerializeAsync(object item) => Task.Factory.StartNew(() => Serialize(item));

        public object Deserialize(byte[] serializedObject) => Deserialize<object>(serializedObject);

        public Task<object> DeserializeAsync(byte[] serializedObject) => Task.Factory.StartNew(() => Deserialize(serializedObject));

        public T Deserialize<T>(byte[] serializedObject)
        {
            using (var ms = new MemoryStream(serializedObject))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }

        public Task<T> DeserializeAsync<T>(byte[] serializedObject) => Task.Factory.StartNew(() => Deserialize<T>(serializedObject));

        public static void Initialize(IList<ICacheConfiguration> configurations)
        {
            foreach (var configuration in configurations)
            {
                var meta = RuntimeTypeModel.Default.Add(configuration.Type, false);
                configuration.Initialize(meta);
            }
        }
    }
}