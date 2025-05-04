using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfGraphicsApp.Shapes;

namespace WpfGraphicsApp.Tools
{
    public class PolygonTool : DrawingTool
    {
        private PointCollection _points = new PointCollection();
        private Polygon _previewPolygon;
        private bool _isFirstPoint = true;

        public override Shape CreatePreviewShape()
        {
            _previewPolygon = new Polygon
            {
                Stroke = Stroke,
                Fill = Fill, // Явно применяем заливку
                StrokeThickness = StrokeThickness,
                Points = new PointCollection()
            };
            return _previewPolygon;
        }

        public override void OnMouseDown(Point position)
        {
            if (_isFirstPoint)
            {
                // Начало нового полигона
                _points.Clear();
                _points.Add(position);
                _isFirstPoint = false;
                StartPoint = position;
            }
            else
            {
                // Проверка на замыкание полигона
                if (ShouldCompletePolygon(position))
                {
                    CompletePolygon();
                    return;
                }

                // Добавление обычной точки
                _points.Add(position);
            }

            UpdatePreview(position);
        }

        public override void OnMouseMove(Point position, Shape previewShape)
        {
            if (_isFirstPoint) return;

            // Обновляем предпросмотр с текущей позицией мыши
            var tempPoints = new PointCollection(_points) { position };
            _previewPolygon.Points = tempPoints;

            // Подсветка для замыкания
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
            
            Vector delta = position - _points[0];
            return delta.Length < 15.0;
        }

        private void CompletePolygon()
        {
            if (_points.Count > 2)
            {
                var polygonShape = new PolygonShape
                {
                    Points = new PointCollection(_points) { _points[0] }, // Замыкаем фигуру
                    Stroke = Stroke,
                    Fill = Fill, // Явно применяем заливку
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

            var tempPoints = new PointCollection(_points) { currentPosition };
            _previewPolygon.Points = tempPoints;
        }

        public void CancelDrawing()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.DrawingCanvas.Children.Remove(_previewPolygon);
            ResetTool();
        }

        private void ResetTool()
        {
            _points.Clear();
            _isFirstPoint = true;
            _previewPolygon = null;
        }
    }
}