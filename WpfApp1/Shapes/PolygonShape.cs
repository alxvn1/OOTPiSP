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

        public Brush Fill { get; set; } = Brushes.Transparent;

        [JsonProperty("FillColor")]
        public string FillColor
        {
            get => (Fill as SolidColorBrush)?.Color.ToString() ?? Brushes.Transparent.ToString();
            set => Fill = string.IsNullOrEmpty(value) ? Brushes.Transparent : new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        }

        [JsonIgnore]
        public Brush Stroke { get; set; } = Brushes.Black;

        [JsonProperty("StrokeColor")]
        public string StrokeColorString
        {
            get => (Stroke as SolidColorBrush)?.Color.ToString() ?? Brushes.Black.ToString();
            set => Stroke = string.IsNullOrEmpty(value) ? Brushes.Black : new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        }

        public override Shape Draw()
        {
            var pointCollection = new PointCollection();
            foreach (var point in Points)
            {
                pointCollection.Add(new Point(point.X, point.Y));
            }
    
            return new Polygon
            {
                Points = pointCollection,
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };
        }

        public override string GetShapeType() => "Polygon";

        public class PointModel
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
    
    // public string StrokeColor
    // {
    //     get => Stroke?.ToString() ?? Brushes.Black.ToString();
    //     set => Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
    // }
    //
    // public string FillColor
    // {
    //     get => Fill?.ToString() ?? Brushes.Transparent.ToString();
    //     set => Fill = string.IsNullOrEmpty(value) 
    //         ? Brushes.Transparent 
    //         : new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
    // }

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