using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp.Tools
{
    public class RectangleTool : DrawingTool
    {
        public override Shape CreatePreviewShape()
        {
            return new Rectangle()
            {
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };
        }
        public override void OnMouseMove(Point position, Shape previewShape)
        {
            if (previewShape is Rectangle rect)
            {
                Canvas.SetLeft(rect, Math.Min(StartPoint.X, position.X));
                Canvas.SetTop(rect, Math.Min(StartPoint.Y, position.Y));
                rect.Width = Math.Abs(position.X - StartPoint.X);
                rect.Height = Math.Abs(position.Y - StartPoint.Y);
            }
        }

        public override ShapeBase OnMouseUp(Point position)
        {
            double x = Math.Min(StartPoint.X, position.X);
            double y = Math.Min(StartPoint.Y, position.Y);
            double width = Math.Abs(position.X - StartPoint.X);
            double height = Math.Abs(position.Y - StartPoint.Y);

            return new RectangleShape
            {
                X = x,  // Сохраняем позицию X
                Y = y,  // Сохраняем позицию Y
                Width = width,
                Height = height,
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };
        }
    }
}