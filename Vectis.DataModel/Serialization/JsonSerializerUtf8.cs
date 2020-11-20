using System;
using System.Buffers;
using System.Text.Json;

namespace Vectis.DataModel
{
    public static class JsonSerializerUtf8
    {
        public static ReadOnlyMemory<byte> Serialize<T>(T data, JsonSerializerOptions options = null)
        {
            var buffer = new ArrayBufferWriter<byte>();
            using var wr = new Utf8JsonWriter(buffer);

            try
            {
                JsonSerializer.Serialize(wr, data, options);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            return buffer.WrittenMemory;
        }

        public static T Deserialize<T>(ReadOnlySpan<byte> utf8Json, JsonSerializerOptions options = null) => JsonSerializer.Deserialize<T>(utf8Json, options);
    }
}
