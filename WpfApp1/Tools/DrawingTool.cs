using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp.Tools
{
    public abstract class DrawingTool
    {
        public Point StartPoint { get; set; }
        public Brush Stroke { get; set; } = Brushes.Black;
        public Brush Fill { get; set; } = Brushes.Transparent;
        public double StrokeThickness { get; set; } = 1;
        public DateTime LastClickTime { get; set; }

        public virtual Shape CreatePreviewShape() => null;
        public virtual void OnMouseDown(Point position) => StartPoint = position;
        public abstract void OnMouseMove(Point position, Shape previewShape);
        public abstract ShapeBase OnMouseUp(Point position);
    
        public virtual bool IsDoubleClick(Point position)
        {
            var now = DateTime.Now;
            var isDouble = (now - LastClickTime).TotalMilliseconds < 500;
            LastClickTime = now;
    
            Vector vector = position - StartPoint;
            return isDouble && vector.Length < 5;
        }
    }
}