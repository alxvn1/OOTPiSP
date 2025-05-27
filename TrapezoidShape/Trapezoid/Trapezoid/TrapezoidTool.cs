using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;
using WpfGraphicsApp.Tools;

namespace WpfGraphicsApp.Tools
{
    public class TrapeziumTool : DrawingTool
    {
        public override Shape CreatePreviewShape()
        {
            return new Polygon()
            {
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };
        }

        public override void OnMouseMove(Point position, Shape previewShape)
        {
            if (previewShape is Polygon polygon)
            {
                double width = Math.Abs(position.X - StartPoint.X);
                double height = Math.Abs(position.Y - StartPoint.Y);
                double topWidth = width * 0.6;

                double x = Math.Min(StartPoint.X, position.X);
                double y = Math.Min(StartPoint.Y, position.Y);
                double leftOffset = (width - topWidth) / 2;

                polygon.Points = new PointCollection
                {
                    new Point(x + leftOffset, y),
                    new Point(x + leftOffset + topWidth, y),
                    new Point(x + width, y + height),
                    new Point(x, y + height)
                };
            }
        }

        public override ShapeBase OnMouseUp(Point position)
        {
            double width = Math.Abs(position.X - StartPoint.X);
            double height = Math.Abs(position.Y - StartPoint.Y);
            double topWidth = width * 0.6;
            double x = Math.Min(StartPoint.X, position.X);
            double y = Math.Min(StartPoint.Y, position.Y);

            return new TrapeziumShape
            {
                X = x,
                Y = y,
                Width = width,
                Height = height,
                TopWidth = topWidth,
                Stroke = Stroke?.Clone() ?? new SolidColorBrush(Colors.Black),
                Fill = Fill?.Clone() ?? new SolidColorBrush(Colors.Transparent),
                StrokeThickness = StrokeThickness
            };
        }
    }
}
