using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MixPlanner.Storage
{
    public static class GlobalJsonSettings
    {
        public const Formatting Formatting = Newtonsoft.Json.Formatting.Indented;

        public static readonly JsonSerializerSettings Settings
            = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
    }
}