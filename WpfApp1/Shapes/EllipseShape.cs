using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfGraphicsApp.Shapes
{
    public class EllipseShape : ShapeBase
    {
            public double X { get; set; }
            public double Y { get; set; }
            public double Width { get; set; } = 100;
            public double Height { get; set; } = 100;
            

        public override Shape Draw()
        {
            var ellipse = new Ellipse
            {
                Width = Width,
                Height = Height,
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };
    
            // Устанавливаем позицию на Canvas
            Canvas.SetLeft(ellipse, X);
            Canvas.SetTop(ellipse, Y);
    
            return ellipse;
        }

        public override string GetShapeType() => "Ellipse";
        public override ShapeBase GetInstance()
        {
            throw new NotImplementedException();
        }
    }
}