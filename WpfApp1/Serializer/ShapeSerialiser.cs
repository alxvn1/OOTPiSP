using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp
{
    public static class ShapeSerializer
    {
        public static void SaveShapesToFile(IEnumerable<ShapeBase> shapes, string filePath)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                string json = JsonConvert.SerializeObject(shapes, settings);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения: {ex.Message}");
            }
        }

        public static List<ShapeBase> LoadShapesFromFile(string filePath)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<ShapeBase>>(json, settings);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}