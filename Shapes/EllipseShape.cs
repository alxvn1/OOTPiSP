using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfGraphicsApp.Shapes
{
    public class EllipseShape : ShapeBase
    {
        public double Width { get; set; } = 100;
        public double Height { get; set; } = 100;

        public override Shape Draw()
        {
            return new Ellipse
            {
                Width = Width,
                Height = Height,
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };
        }

        public override string GetShapeType() => "Ellipse";
    }
}