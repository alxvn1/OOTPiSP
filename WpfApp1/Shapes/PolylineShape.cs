using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfGraphicsApp.Shapes
{
    public class PolylineShape : Shape
    {
        public PointCollection Points { get; set; } = new PointCollection
        {
            new Point(100, 100),
            new Point(150, 50),
            new Point(200, 100),
            new Point(250, 50),
            new Point(300, 100)
        };

        public override System.Windows.Shapes.Shape Draw()
        {
            return new Polyline
            {
                Points = Points,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };
        }
    }
}