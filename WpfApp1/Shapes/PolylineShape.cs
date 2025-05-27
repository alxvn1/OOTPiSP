using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WpfGraphicsApp.Shapes
{
    public class PolylineShape : ShapeBase
    {
        [JsonProperty("Points")]
        public List<PointModel> Points { get; set; } = new List<PointModel>();

        [JsonIgnore]
        public PointCollection PointCollection 
        {
            get
            {
                var collection = new PointCollection();
                foreach (var point in Points)
                {
                    collection.Add(new Point(point.X, point.Y));
                }
                return collection;
            }
        }

        public override Shape Draw()
        {
            return new Polyline
            {
                Points = PointCollection,
                Stroke = Stroke,
                Fill = Brushes.Transparent, // Явно устанавливаем прозрачную заливку
                StrokeThickness = StrokeThickness
            };
        }

        public override string GetShapeType() => "Polyline";

        public override ShapeBase GetInstance()
        {
            return new PolylineShape
            {
                Points = new List<PointModel>(this.Points),
                Stroke = this.Stroke?.Clone(),
                StrokeThickness = this.StrokeThickness
                // Fill не копируем, так как он не используется
            };
        }

        public class PointModel
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }

    public static class PolylineExtensions
    {
        public static List<PolylineShape.PointModel> ToPointModelList(this PointCollection points)
        {
            var list = new List<PolylineShape.PointModel>();
            foreach (Point point in points)
            {
                list.Add(new PolylineShape.PointModel { X = point.X, Y = point.Y });
            }
            return list;
        }

        public static PointCollection ToPointCollection(this List<PolylineShape.PointModel> pointModels)
        {
            var collection = new PointCollection();
            foreach (var pm in pointModels)
            {
                collection.Add(new Point(pm.X, pm.Y));
            }
            return collection;
        }
    }
}