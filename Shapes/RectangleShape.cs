// Shapes/RectangleShape.cs
using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfGraphicsApp.Shapes
{
    public class RectangleShape : ShapeBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public override Shape Draw()
        {
            return new Rectangle
            {
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness,
                Width = Width,
                Height = Height
            };
        }

        public override string GetShapeType() => "Rectangle";
    }
}

