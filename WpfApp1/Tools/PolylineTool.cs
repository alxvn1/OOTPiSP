using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace WpfGraphicsApp.Tools
{
    public class PolylineTool : DrawingTool
    {
        private List<PolylineShape.PointModel> _points = new List<PolylineShape.PointModel>();
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
            _points.Add(new PolylineShape.PointModel { X = position.X, Y = position.Y });
            UpdatePreview();
        }

        public override void OnMouseMove(Point position, Shape previewShape)
        {
            if (_points.Count == 0) return;
            
            var tempPoints = new PointCollection();
            foreach (var point in _points)
            {
                tempPoints.Add(new Point(point.X, point.Y));
            }
            tempPoints.Add(position);
            _previewPolyline.Points = tempPoints;
        }

        public override ShapeBase OnMouseUp(Point position)
        {
            // Для полилинии все обрабатывается в других методах
            return null;
        }

        private void UpdatePreview()
        {
            var tempPoints = new PointCollection();
            foreach (var point in _points)
            {
                tempPoints.Add(new Point(point.X, point.Y));
            }
            _previewPolyline.Points = tempPoints;
        }

        public ShapeBase CompleteDrawing()
        {
            if (_points.Count > 1)
            {
                return new PolylineShape
                {
                    Points = _points.ToList(), // Исправлено преобразование типов
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
            _previewPolyline = null;
        }
    }
}