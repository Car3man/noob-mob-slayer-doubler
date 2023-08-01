using System;
using System.Globalization;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;

namespace Game.Configs.JsonImpl
{
    public class BigIntegerConverter : JsonConverter<BigInteger>
    {
        public override void WriteJson(JsonWriter writer, BigInteger value, JsonSerializer serializer)
        {
            // Convert the BigInteger value to a byte array using UTF8 encoding
            byte[] bytes = Encoding.UTF8.GetBytes(value.ToString());

            // Write the byte array as a raw JSON numeric value (without quotes)
            writer.WriteRawValue(Encoding.UTF8.GetString(bytes));
        }

        public override BigInteger ReadJson(JsonReader reader, Type objectType, BigInteger existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                if (reader.Value != null)
                {
                    return (long)reader.Value;
                }
            }
            if (reader.TokenType == JsonToken.String)
            {
                if (BigInteger.TryParse((string)reader.Value ?? string.Empty, NumberStyles.None, CultureInfo.InvariantCulture, out var result))
                {
                    return result;
                }
            }
            throw new JsonException($"Could not convert \"{reader.Value}\" to BigInteger.");
        }
    }
}