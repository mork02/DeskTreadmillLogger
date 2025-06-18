using System.IO;
using System.Text.Json;

namespace DeskTreadmillLogger.utility
{
    internal class JSONReader
    {
        public static List<T> Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
        // add a check if path exists
        public static void Save<T>(List<T> data, string filePath)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        // also make this method to remove a specific object in the json
        public static void Remove<T>(List<T> data, string filePath)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        //
    }
}
