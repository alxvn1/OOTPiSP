using WpfGraphicsApp.Shapes;
using WpfGraphicsApp.Tools;
using WpfGraphicsApp; // Добавлено для доступа к IShapePlugin

namespace WpfGraphicsApp 
{
    public class TrapezoidPlugin : IShapePlugin
    {
        public string ShapeName => "Трапеция";
        public ShapeBase CreateShape() => new TrapeziumShape();
        public DrawingTool CreateTool() => new TrapeziumTool();
        public string GetShapeType() => "Trapezium";
    }
}