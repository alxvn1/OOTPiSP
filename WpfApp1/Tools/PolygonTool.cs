using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;
using System.Collections.Generic;

namespace WpfGraphicsApp.Tools
{
    public class PolygonTool : DrawingTool
    {
        private List<PolygonShape.PointModel> _points = new List<PolygonShape.PointModel>();
        private Polygon _previewPolygon;
        private bool _isFirstPoint = true;
        private Point _startPoint;

        public override Shape CreatePreviewShape()
        {
            _previewPolygon = new Polygon
            {
                Stroke = Stroke,
                Fill = Fill,
                StrokeThickness = StrokeThickness,
                Points = new PointCollection()
            };
            return _previewPolygon;
        }

        public override void OnMouseDown(Point position)
        {
            if (_isFirstPoint)
            {
                _points.Clear();
                _points.Add(new PolygonShape.PointModel { X = position.X, Y = position.Y });
                _isFirstPoint = false;
                _startPoint = position;
            }
            else
            {
                if (ShouldCompletePolygon(position))
                {
                    CompletePolygon();
                    return;
                }

                _points.Add(new PolygonShape.PointModel { X = position.X, Y = position.Y });
            }

            UpdatePreview(position);
        }

        public override void OnMouseMove(Point position, Shape previewShape)
        {
            if (_isFirstPoint) return;

            var tempPoints = new PointCollection();
            foreach (var point in _points)
            {
                tempPoints.Add(new Point(point.X, point.Y));
            }
            tempPoints.Add(position);
            _previewPolygon.Points = tempPoints;

            _previewPolygon.Stroke = ShouldCompletePolygon(position) 
                ? Brushes.Green 
                : Stroke;
        }

        public override ShapeBase OnMouseUp(Point position)
        {
            return null; // Для полигона все обрабатывается в OnMouseDown
        }

        private bool ShouldCompletePolygon(Point position)
        {
            if (_points.Count < 2) return false;
            
            Vector delta = position - _startPoint;
            return delta.Length < 15.0;
        }

        private void CompletePolygon()
        {
            if (_points.Count > 2)
            {
                var polygonShape = new PolygonShape
                {
                    Points = new List<PolygonShape.PointModel>(_points),
                    Stroke = Stroke,
                    Fill = Fill,
                    StrokeThickness = StrokeThickness
                };

                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.AddFinalShapeToCanvas(polygonShape);
            }
            ResetTool();
        }

        private void UpdatePreview(Point currentPosition)
        {
            if (_previewPolygon == null) return;

            var tempPoints = new PointCollection();
            foreach (var point in _points)
            {
                tempPoints.Add(new Point(point.X, point.Y));
            }
            tempPoints.Add(currentPosition);
            _previewPolygon.Points = tempPoints;
        }

        public void CancelDrawing()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.DrawingCanvas.Children.Remove(_previewPolygon);
            ResetTool();
            _previewPolygon = null;
        }

        private void ResetTool()
        {
            _points.Clear();
            _isFirstPoint = true;
            _previewPolygon = null;
        }
    }
}