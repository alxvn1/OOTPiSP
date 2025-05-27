// IShapePlugin.cs

using WpfGraphicsApp.Shapes;

public interface IShapePlugin
{
    ShapeBase CreateShape();
    string ShapeName { get; }
}