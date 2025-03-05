using System.Windows.Shapes;

namespace WpfGraphicsApp.Shapes
{
    public class EllipseShape : Shape
    {
        public double Width { get; set; } = 100;
        public double Height { get; set; } = 100;

        public override System.Windows.Shapes.Shape Draw()
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
    }
}