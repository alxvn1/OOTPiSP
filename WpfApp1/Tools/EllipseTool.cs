using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp.Tools
{
    public class EllipseTool : DrawingTool
    {
        public override Shape CreatePreviewShape()
        {
            return new Ellipse
            {
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness
            };
        }

        public override void OnMouseMove(Point position, Shape previewShape)
        {
            if (previewShape is Ellipse ellipse)
            {
                double width = Math.Abs(position.X - StartPoint.X);
                double height = Math.Abs(position.Y - StartPoint.Y);
                
                Canvas.SetLeft(ellipse, Math.Min(StartPoint.X, position.X));
                Canvas.SetTop(ellipse, Math.Min(StartPoint.Y, position.Y));
                
                ellipse.Width = width;
                ellipse.Height = height;
            }
        }

        public override ShapeBase OnMouseUp(Point position)
        {
            double x = Math.Min(StartPoint.X, position.X);
            double y = Math.Min(StartPoint.Y, position.Y);
            double width = Math.Abs(position.X - StartPoint.X);
            double height = Math.Abs(position.Y - StartPoint.Y);

            return new EllipseShape
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