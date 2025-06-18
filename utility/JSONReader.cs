using System.IO;
using System.Text.Json;

namespace DeskTreadmillLogger.utility
{
    internal class JSONReader
    {
        public static List<T> Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, "[]");
                return new List<T>();
            }

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }

        public static void Add<T>(T newData, string filePath)
        {
            var dataList = Load<T>(filePath);
            dataList.Add(newData);
            Save(dataList, filePath);
        }

        private static void Save<T>(List<T> data, string filePath)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void Remove<T>(Predicate<T> match, string filePath)
        {
            var dataList = Load<T>(filePath);
            dataList.RemoveAll(match);
            Save(dataList, filePath);
        }
    }
}
