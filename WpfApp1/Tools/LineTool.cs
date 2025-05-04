using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp.Tools
{
    public class LineTool : DrawingTool
    {
        public override Shape CreatePreviewShape()
        {
            return new Line()
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };
        }

        public override void OnMouseMove(Point position, Shape previewShape)
        {
            if (previewShape is Line line)
            {
                line.X1 = StartPoint.X;
                line.Y1 = StartPoint.Y;
                line.X2 = position.X;
                line.Y2 = position.Y;
            }
        }

        public override ShapeBase OnMouseUp(Point position)
        {
            return new LineShape
            {
                X1 = StartPoint.X,
                Y1 = StartPoint.Y,
                X2 = position.X,
                Y2 = position.Y,
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };
        }
    }
}