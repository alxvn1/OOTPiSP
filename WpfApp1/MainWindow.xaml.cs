using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using System.Windows.Shapes;
using System.Windows.Controls;
using WpfGraphicsApp.Shapes;
using WpfGraphicsApp.UndoRedo;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WpfGraphicsApp.Tools;
using WindowStartupLocation = System.Windows.WindowStartupLocation;

namespace WpfGraphicsApp
{
    public partial class MainWindow : Window
    {
        private ShapeManager _shapeManager = new ShapeManager();
        private DrawingTool _currentTool;
        private Shape _previewShape;
        private bool _isDrawing = false;
        private Point _startPoint;
        private Color _fillColor = Colors.Transparent;
        private Color _strokeColor = Colors.Black;
        private double _strokeThickness = 2;

        public MainWindow()
        {
            InitializeComponent();
        
            // Подписка на события мыши
            DrawingCanvas.MouseLeftButtonDown += DrawingCanvas_MouseLeftButtonDown;
            DrawingCanvas.MouseMove += DrawingCanvas_MouseMove;
            DrawingCanvas.MouseLeftButtonUp += DrawingCanvas_MouseLeftButtonUp;
            
            this.Loaded += (s, e) => { this.Focus(); };
        }
        
        private void DrawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(DrawingCanvas);
    
            if (e.ClickCount == 2)
            {
                if (_currentTool is PolylineTool polylineTool)
                {
                    var shape = polylineTool.CompleteDrawing();
                    if (shape != null)
                    {
                        AddShapeToCanvas(shape);
                        DrawingCanvas.Children.Remove(_previewShape);
                        _previewShape = null;
                    }
                }
                return;
            }

            if (_currentTool == null) return;
    
            if (_previewShape == null)
            {
                _previewShape = _currentTool.CreatePreviewShape();
                DrawingCanvas.Children.Add(_previewShape);
            }
    
            _currentTool.OnMouseDown(position);
            _isDrawing = true;
        }
        
        
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing || _currentTool == null || _previewShape == null) return;
    
            var currentPoint = e.GetPosition(DrawingCanvas);
            _currentTool.OnMouseMove(currentPoint, _previewShape);
        }
        
        
        
        private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Для Polyline и Polygon ничего не делаем
            if (_currentTool is PolylineTool || _currentTool is PolygonTool)
            {
                return;
            }

            if (_isDrawing && _currentTool != null)
            {
                var endPoint = e.GetPosition(DrawingCanvas);
                var shape = _currentTool.OnMouseUp(endPoint);
        
                if (shape != null)
                {
                    AddShapeToCanvas(shape);
                }
        
                DrawingCanvas.Children.Remove(_previewShape);
                _previewShape = null;
            }
            _isDrawing = false;
        }
        
        // Tool selection handlers
        private void SelectLineTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new LineTool();
        }

        private void SelectRectangleTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new RectangleTool();
        }

        private void SelectEllipseTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new EllipseTool();
        }

        private void SelectPolylineTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new PolylineTool();
        }

        private void SelectPolygonTool_Click(object sender, RoutedEventArgs e)
        {
            _currentTool = new PolygonTool();
        }
        
        // File operations handlers
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON File (*.json)|*.json",
                DefaultExt = ".json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_shapeManager.Shapes.ToList(), options);
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JSON File (*.json)|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string json = File.ReadAllText(openFileDialog.FileName);
                var shapes = JsonSerializer.Deserialize<List<ShapeBase>>(json);
                _shapeManager.Clear();
                foreach (var shape in shapes)
                {
                    _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));
                }
                RedrawCanvas();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            _shapeManager.Clear();
            DrawingCanvas.Children.Clear();
        }
        
        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _strokeThickness = e.NewValue;
    
            // Обновляем текущий инструмент
            if (_currentTool != null)
            {
                _currentTool.StrokeThickness = _strokeThickness;
        
                // Если есть активное превью - обновляем его
                if (_previewShape != null)
                {
                    if (_previewShape is Line line)
                        line.StrokeThickness = _strokeThickness;
                    else if (_previewShape is Polyline polyline)
                        polyline.StrokeThickness = _strokeThickness;
                    else if (_previewShape is Polygon polygon)
                        polygon.StrokeThickness = _strokeThickness;
                    else if (_previewShape is Rectangle rect)
                        rect.StrokeThickness = _strokeThickness;
                    else if (_previewShape is Ellipse ellipse)
                        ellipse.StrokeThickness = _strokeThickness;
                }
            }
        }

        // Shape drawing handlers
        private void DrawPolygon_Click(object sender, RoutedEventArgs e)
        {
            var polygon = new PolygonShape();
            polygon.Points = new PointCollection(new List<Point>
            {
                new Point(100, 0),
                new Point(200, 50),
                new Point(150, 150),
                new Point(50, 150),
                new Point(0, 50)
            });
            AddShapeToCanvas(polygon);
        }

        private void DrawSquare_Click(object sender, RoutedEventArgs e)
        {
            AddShapeToCanvas(new RectangleShape { Width = 100, Height = 100 });
        }

        private void DrawLine_Click(object sender, RoutedEventArgs e)
        {
            AddShapeToCanvas(new LineShape
            {
                X1 = 50,
                Y1 = 50,
                X2 = 200,
                Y2 = 200
            });
        }

        private void DrawPolyline_Click(object sender, RoutedEventArgs e)
        {
            var polyline = new PolylineShape();
            polyline.Points = new PointCollection(new List<Point>
            {
                new Point(50, 50),
                new Point(100, 150),
                new Point(150, 50),
                new Point(200, 150),
                new Point(250, 50)
            });
            AddShapeToCanvas(polyline);
        }

        // Color selection methods
        private void SelectFillColor_Click(object sender, RoutedEventArgs e)
        {
            ShowColorPickerDialog("Выберите цвет заливки", (color) => 
            {
                _fillColor = color; // Обязательно сохраняем цвет
            }, _fillColor);
        }

        private void SelectStrokeColor_Click(object sender, RoutedEventArgs e)
        {
            ShowColorPickerDialog("Выберите цвет обводки", (color) => _strokeColor = color, _strokeColor);
        }

        private void ShowColorPickerDialog(string title, Action<Color> colorSetter, Color initialColor)
        {
            var dialog = new Window
            {
                Title = title,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            var colorPicker = new ColorPicker
            {
                SelectedColor = initialColor,
                ColorMode = ColorMode.ColorPalette,
                Margin = new Thickness(10),
                UsingAlphaChannel = false,
                AvailableColorsSortingMode = ColorSortingMode.HueSaturationBrightness
            };

            var okButton = new Button
            {
                Content = "OK",
                Margin = new Thickness(0,10,0,0),
                Width = 80,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            okButton.Click += (s, args) =>
            {
                if (colorPicker.SelectedColor.HasValue)
                {
                    colorSetter(colorPicker.SelectedColor.Value);
                    dialog.Close();
                }
            };

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(colorPicker);
            stackPanel.Children.Add(okButton);

            dialog.Content = stackPanel;
            dialog.ShowDialog();
        }

        // Helper methods
        private void AddShapeToCanvas(ShapeBase shape)
        {
            // Устанавливаем общие свойства для всех фигур
            shape.Stroke = new SolidColorBrush(_strokeColor);
            shape.StrokeThickness = _strokeThickness;
        
            // Особые правила для заливки
            if (shape is LineShape || shape is PolylineShape)
            {
                shape.Fill = Brushes.Transparent; // Без заливки
            }
            else
            {
                shape.Fill = new SolidColorBrush(_fillColor); // С заливкой
            }
        
            var uiElement = shape.Draw();
            DrawingCanvas.Children.Add(uiElement);
            _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));
        }

        public void AddFinalShapeToCanvas(ShapeBase shape)
        {
            // Удаляем превью
            DrawingCanvas.Children.Remove(_previewShape);
            _previewShape = null;
    
            // Добавляем финальную фигуру через менеджер команд
            _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));
    
            // Перерисовываем холст
            RedrawCanvas();
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (_currentTool is PolylineTool polylineTool)
                {
                    polylineTool.CancelDrawing();
                    DrawingCanvas.Children.Remove(_previewShape);
                    _previewShape = null;
                    _isDrawing = false;
                }
                else if (_currentTool is PolygonTool polygonTool)
                {
                    polygonTool.CancelDrawing();
                    DrawingCanvas.Children.Remove(_previewShape);
                    _previewShape = null;
                    _isDrawing = false;
                }
            }
            base.OnKeyDown(e);
        }

        private void DrawRectangle_Click(object sender, RoutedEventArgs e)
        {
            AddShapeToCanvas(new RectangleShape { Width = 150, Height = 100 });
        }

        private void DrawEllipse_Click(object sender, RoutedEventArgs e)
        {
            AddShapeToCanvas(new EllipseShape { Width = 100, Height = 100 });
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            _shapeManager.Undo();
            RedrawCanvas();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            _shapeManager.Redo();
            RedrawCanvas();
        }

        
        private void RedrawCanvas()
        {
            DrawingCanvas.Children.Clear();
    
            // Отрисовываем все фигуры из истории
            foreach (var shape in _shapeManager.Shapes)
            {
                var uiElement = shape.Draw();
                DrawingCanvas.Children.Add(uiElement);
            }
    
            // Восстанавливаем превью, если есть активное рисование
            if (_isDrawing && _previewShape != null)
            {
                DrawingCanvas.Children.Add(_previewShape);
            }
        }
        
    }
}