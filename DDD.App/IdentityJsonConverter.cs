using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using DDD.Core;

namespace DDD.App
{
    public class IdentityJsonConverter : JsonConverter<IIdentity>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.GetInterface(nameof(IIdentity)) != null;
        }

        public override IIdentity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var method = typeToConvert.GetMethod("FromData");
            return null;
        }

        public override void Write(Utf8JsonWriter writer, IIdentity value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Identity);
        }
    }
}