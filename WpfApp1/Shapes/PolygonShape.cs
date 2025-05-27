using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WpfGraphicsApp.Shapes
{
    public class PolygonShape : ShapeBase
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
            return new Polygon
            {
                Points = PointCollection,
                Stroke = Stroke,
                Fill = Fill,  // Для полигона оставляем заливку
                StrokeThickness = StrokeThickness
            };
        }

        public override string GetShapeType() => "Polygon";

        public override ShapeBase GetInstance()
        {
            return new PolygonShape
            {
                Points = new List<PointModel>(this.Points),
                Stroke = this.Stroke?.Clone(),
                Fill = this.Fill?.Clone(),
                StrokeThickness = this.StrokeThickness
            };
        }

        public class PointModel
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }

    public static class PolygonExtensions
    {
        public static List<PolygonShape.PointModel> ToPointModelList(this PointCollection points)
        {
            var list = new List<PolygonShape.PointModel>();
            foreach (Point point in points)
            {
                list.Add(new PolygonShape.PointModel { X = point.X, Y = point.Y });
            }
            return list;
        }

        public static PointCollection ToPointCollection(this List<PolygonShape.PointModel> pointModels)
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