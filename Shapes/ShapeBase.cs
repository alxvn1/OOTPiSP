using System.Windows.Shapes;
using System.Windows.Media;

namespace WpfGraphicsApp.Shapes
{
    public abstract class ShapeBase
    {
        public Brush Fill { get; set; }
        public Brush Stroke { get; set; }
        public double StrokeThickness { get; set; }

        public abstract Shape Draw();
        public abstract string GetShapeType(); // Абстрактный метод, который необходимо реализовать
    }
}