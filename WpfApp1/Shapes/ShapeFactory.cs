using System;
using WpfGraphicsApp.Shapes; // Использование вашего класса Shape

namespace WpfGraphicsApp
{
    public static class ShapeFactory
    {
        public static ShapeBase CreateShape(string shapeType)
        {
            return shapeType switch
            {
                "Line" => new LineShape(),
                "Rectangle" => new RectangleShape(),
                "Ellipse" => new EllipseShape(),
                "Polygon" => new PolygonShape(),
                "Polyline" => new PolylineShape(),
                _ => throw new ArgumentException("Unknown shape type")
            };
        }
    }
}