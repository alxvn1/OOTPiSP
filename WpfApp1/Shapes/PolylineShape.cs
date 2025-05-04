// Shapes/PolylineShape.cs
using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfGraphicsApp.Shapes
{
    public class PolylineShape : ShapeBase
    {
        public PointCollection Points { get; set; } = new PointCollection();

        public override Shape Draw()
        {
            return new Polyline
            {
                Stroke = Stroke,
                Fill = Brushes.Transparent,
                StrokeThickness = StrokeThickness,
                Points = Points,
            };
        }

        public override string GetShapeType() => "Polyline";
    }
}