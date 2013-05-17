using System;
using MixPlanner.Converters;
using MixPlanner.DomainModel;
using Newtonsoft.Json;

namespace MixPlanner.Storage
{
    public class HarmonicKeyJsonConverter : JsonConverter
    {
        static readonly Id3v2TkeyHarmonicKeyConverter Converter
            = new Id3v2TkeyHarmonicKeyConverter();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            string str = (string)Converter.Convert(value, typeof (string), null, null);
            writer.WriteValue(str);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return HarmonicKey.Unknown;

            if (reader.TokenType != JsonToken.String)
                throw new FormatException(String.Format("Unexpected token parsing key. Expected String, got {0}.", reader.TokenType));

            var str = reader.Value.ToString();
            if (String.IsNullOrEmpty(str))
                return HarmonicKey.Unknown;

            return Converter.ConvertBack(str, typeof (HarmonicKey), null, null);
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == null) throw new ArgumentNullException("objectType");
            return typeof (HarmonicKey).IsAssignableFrom(objectType);
        }
    }
}