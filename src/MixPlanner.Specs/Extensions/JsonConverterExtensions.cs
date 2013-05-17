using System.IO;
using System.Text;
using MixPlanner.DomainModel;
using MixPlanner.Storage;

// ReSharper disable CheckNamespace
namespace Newtonsoft.Json
// ReSharper restore CheckNamespace
{
    public static class JsonConverterExtensions
    {
         public static string WriteJsonString(this JsonConverter converter, object obj)
         {
             using (var memoryStream = new MemoryStream())
             using (var textWriter = new StreamWriter(memoryStream))
             using (var jsonWriter = new JsonTextWriter(textWriter))
             {
                 converter.WriteJson(jsonWriter, obj, new JsonSerializer());

                 jsonWriter.Flush();
                 memoryStream.Seek(0, SeekOrigin.Begin);

                 using (var streamReader = new StreamReader(memoryStream))
                     return streamReader.ReadToEnd();
             }
         }

         public static T ReadJsonString<T>(this JsonConverter converter, string json)
         {
             return JsonConvert.DeserializeObject<T>(json, new HarmonicKeyJsonConverter());

             /*

             byte[] bytes = Encoding.Default.GetBytes(json);
             using (var memoryStream = new MemoryStream(bytes))
             using (var streamReader = new StreamReader(memoryStream))
             using (var jsonReader = new JsonTextReader(streamReader))
                 return (T)converter.ReadJson(jsonReader, typeof (T), null, new JsonSerializer());
              * */
         }
    }
}