// WpfGraphicsApp/Interfaces/IShapePlugin.cs
using WpfGraphicsApp.Shapes;
using WpfGraphicsApp.Tools;

namespace WpfGraphicsApp 
{
    public interface IShapePlugin
    {
        string ShapeName { get; }
        ShapeBase CreateShape();
        DrawingTool CreateTool();
        string GetShapeType();
    }
}