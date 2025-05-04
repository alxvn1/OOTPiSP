using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGraphicsApp.Shapes
{
    /*public class PolygonShape : ShapeBase
    {
        public PointCollection Points { get; set; } = new PointCollection();
    
        public override Shape Draw()
        {
            var path = new Path
            {
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness,
                Data = CreatePathGeometry()
            };
            return path;
        }
    
        private PathGeometry CreatePathGeometry()
        {
            var geometry = new PathGeometry();
            
            if (Points.Count < 2) 
                return geometry;
    
            var figure = new PathFigure { StartPoint = Points[0] };
    
            for (int i = 1; i < Points.Count; i++)
            {
                figure.Segments.Add(new LineSegment(Points[i], true));
            }
    
            // Замыкаем фигуру
            if (Points.Count > 2)
            {
                figure.IsClosed = true;
            }
    
            geometry.Figures.Add(figure);
            return geometry;
        }
    
        public override string GetShapeType() => "Polygon";
    }*/
    public class PolygonShape : ShapeBase
    {
        public PointCollection Points { get; set; } = new PointCollection();

        public override Shape Draw()
        {
            return new Polygon
            {
                Points = Points,
                Stroke = Stroke,
                Fill = Fill, // Сохраняем установленную заливку
                StrokeThickness = StrokeThickness
            };
        }

        public override string GetShapeType() => "Polygon";
    }
}