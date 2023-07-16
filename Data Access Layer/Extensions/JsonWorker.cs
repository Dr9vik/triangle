using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Extention_Layer
{
    public static class JsonWorker
    {
        public static T Get<T>(string path) where T : class
        {
            JsonSerializer _serializer = new JsonSerializer();
            using (JsonReader jr = new JsonTextReader(new StreamReader(path, Encoding.Default)))
                return _serializer.Deserialize<T>(jr);
        }

        public static async Task Set<T>(string path, T item) where T : class
        {
            string output = JsonConvert.SerializeObject(item);
            using (StreamWriter writer = new StreamWriter(path, false))
                await writer.WriteLineAsync(output);
        }
    }
}
