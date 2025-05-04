using System.Windows.Controls;
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
            var rect = new Rectangle
            {
                Width = Width,
                Height = Height,
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };
    
            // Устанавливаем позицию на Canvas
            Canvas.SetLeft(rect, X);
            Canvas.SetTop(rect, Y);
    
            return rect;
        }

        public override string GetShapeType() => "Rectangle";
    }
}

