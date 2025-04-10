using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp
{
    public interface IPlugin
    {
        string Name { get; }
        ShapeBase CreateShape();
    }

    public class StarPlugin : IPlugin
    {
        public string Name => "Star";
        public ShapeBase CreateShape() => new StarShape();
    }

    public class StarShape : ShapeBase
    {
        public override Shape Draw() => new Path
        {
            Data = Geometry.Parse("M50,0 L61,35 L98,35 L68,57 L79,92 L50,70 L21,92 L32,57 L2,35 L39,35 Z"),
            Stroke = this.Stroke,
            Fill = this.Fill,
            StrokeThickness = this.StrokeThickness
        };

        public override string GetShapeType() => "Star";
    }
}