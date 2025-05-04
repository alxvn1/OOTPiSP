using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGraphicsApp.Shapes
{
    public abstract class ShapeBase
    {
        public Brush Stroke { get; set; }
        public Brush Fill { get; set; }
        public double StrokeThickness { get; set; }

        public abstract Shape Draw();
        public abstract string GetShapeType();
    }
}