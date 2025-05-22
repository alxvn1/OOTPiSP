using Newtonsoft.Json;
using System;
using System.IO;

namespace WpfGraphicsApp
{
    public static class JSONSerializer
    {
        public static void SerializeToFile<T>(T obj, string filePath)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(obj, settings);
            File.WriteAllText(filePath, json);
        }

        public static T DeserializeFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            string json = File.ReadAllText(filePath);
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}