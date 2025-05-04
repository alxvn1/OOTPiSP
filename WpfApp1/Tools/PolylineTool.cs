using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp.Tools
{
    public class PolylineTool : DrawingTool
    {
        private PointCollection _points = new PointCollection();
        private Polyline _previewPolyline;

        public override Shape CreatePreviewShape()
        {
            _previewPolyline = new Polyline
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness,
                Points = new PointCollection()
            };
            return _previewPolyline;
        }

        public override void OnMouseDown(Point position)
        {
            _points.Add(position);
            _previewPolyline.Points = new PointCollection(_points);
        }

        public override void OnMouseMove(Point position, Shape previewShape)
        {
            if (_points.Count == 0) return;
            
            var tempPoints = new PointCollection(_points) { position };
            _previewPolyline.Points = tempPoints;
        }

        public override ShapeBase OnMouseUp(Point position)
        {
            return null; // Превью остается видимым
        }

        public ShapeBase CompleteDrawing()
        {
            if (_points.Count > 1)
            {
                return new PolylineShape
                {
                    Points = new PointCollection(_points),
                    Stroke = Stroke,
                    StrokeThickness = StrokeThickness
                };
            }
            return null;
        }

        public void CancelDrawing()
        {
            _points.Clear();
            if (_previewPolyline != null)
            {
                _previewPolyline.Points.Clear();
            }
        }
    }
}