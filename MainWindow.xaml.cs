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

namespace WpfGraphicsApp
{
    public partial class MainWindow : Window
    {
        private ShapeManager _shapeManager = new ShapeManager();
        private Color _fillColor = Colors.Transparent;
        private Color _strokeColor = Colors.Black;
        private double _strokeThickness = 2;

        public MainWindow()
        {
            InitializeComponent();
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
            ShowColorPickerDialog("Выберите цвет заливки", (color) => _fillColor = color, _fillColor);
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
                // WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Owner = this
            };

            var colorPicker = new ColorPicker
            {
                SelectedColor = initialColor,
                ColorMode = ColorMode.ColorPalette,
                Margin = new Thickness(10)
            };

            colorPicker.SelectedColorChanged += (s, args) =>
            {
                if (colorPicker.SelectedColor.HasValue)
                {
                    colorSetter(colorPicker.SelectedColor.Value);
                    dialog.Close();
                }
            };

            dialog.Content = colorPicker;
            dialog.ShowDialog();
        }

        // Helper methods
        private void AddShapeToCanvas(ShapeBase shape)
        {
            shape.Fill = new SolidColorBrush(_fillColor);
            shape.Stroke = new SolidColorBrush(_strokeColor);
            shape.StrokeThickness = _strokeThickness;

            DrawingCanvas.Children.Add(shape.Draw());
            _shapeManager.ExecuteCommand(new DrawCommand(_shapeManager, shape));
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
            foreach (var shape in _shapeManager.Shapes)
            {
                DrawingCanvas.Children.Add(shape.Draw());
            }
        }
    }
}