using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfGraphicsApp.Shapes
{
    public abstract class Shape
    {
        public Brush Stroke { get; set; } = Brushes.Black;
        public Brush Fill { get; set; } = Brushes.LightBlue;
        public double StrokeThickness { get; set; } = 2;

        public abstract System.Windows.Shapes.Shape Draw();
    }
}