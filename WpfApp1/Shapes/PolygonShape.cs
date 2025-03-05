using System.Windows;
using System.Windows.Shapes;

namespace WpfGraphicsApp.Shapes
{
    public class PolygonShape : Shape
    {
        public int Sides { get; set; } = 3;

        public override System.Windows.Shapes.Shape Draw()
        {
            Polygon polygon = new Polygon
            {
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };

            double centerX = 200, centerY = 200, radius = 100;
            for (int i = 0; i < Sides; i++)
            {
                double angle = 2 * Math.PI * i / Sides;
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);
                polygon.Points.Add(new Point(x, y));
            }

            return polygon;
        }
    }
}